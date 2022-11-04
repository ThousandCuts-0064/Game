using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class World : Singleton<World>
{
    public const int SIZE = 512;
    public const int HSIZE = SIZE / 2;
    public const int START = -HSIZE;
    public const int END = START + SIZE - 1;

    private static OffsetArray3<Chunk> _chunks;
    [SerializeField] private long _seed;
    [SerializeField] private float _xSeed;
    [SerializeField] private float _zSeed;
    public long Seed
    {
        get => _seed;
        private set
        {
            _seed = value;
            int x = 0;
            int z = 0;
            for (int i = 0; i < Const.FLOAT_FRACTION_BITS * 2; i++)
            {
                int bit = 1 << i;
                if ((_seed & bit) != 0)
                    x |= bit;

                i++;
                bit = 1 << i;
                if ((_seed & bit) != 0)
                    z |= bit;
            }
            _xSeed = BitConverter.Int32BitsToSingle(x);
            _zSeed = BitConverter.Int32BitsToSingle(z);
        }
    }
    [SerializeField] private Material[] _materials;
    [field: SerializeField] public Player Player { get; private set; }
    [field: SerializeField] public Chunk Chunk { get; private set; }
    [field: SerializeField] public Block Block { get; private set; }

    public static event ChunkIndices ChunkCreated;

    protected override void Awake()
    {
        base.Awake();
        Chunk.Initialize();
        Block.Initialize(_materials);
        _chunks = new OffsetArray3<Chunk>(START, SIZE, START, SIZE, START, SIZE);
        Seed = 1231411241212412314;
    }

    private void Start()
    {
        StartCoroutine(ChunkCreator(() =>
        {
            int yMax = 0;
            for (int yc = START; yc <= END; yc++)
                if (_chunks[0, yc, 0])
                    for (int yb = 0; yb < Chunk.SIZE; yb++)
                        if (_chunks[0, yc, 0][0, yb, 0])
                            yMax = yb;

            Instantiate(Player, new Vector3(0, yMax - Chunk.START + Block.HSIDE + Player.LowestPoint, 0), Quaternion.identity);
        }));
    }

    private IEnumerator ChunkCreator(Action onEnd)
    {
        int size = 5;
        for (int x = -size; x < size; x++)
        {
            for (int z = -size; z < size; z++)
            {
                CreateChunkPillar(x, z);
                yield return new WaitUntil(() => 1 / Time.deltaTime > 1);
            }
        }
        onEnd?.Invoke();
    }

    public static Chunk GetChunk(int x, int y, int z) => _chunks[x, y, z];

    public static bool TryGetChunk(int x, int y, int z, out Chunk chunk) =>
        _chunks.TryGetValue(x, y, z, out chunk);

    private Chunk NewChunk(int x, int y, int z)
    {
        var chunk = Instantiate(Chunk, new Vector3(x, y, z) * Chunk.SIZE, Quaternion.identity, transform)
            .ReplaceClone((x, y, z).ToString());
        _chunks[x, y, z] = chunk;
        ChunkCreated(chunk, x, y, z);

        return chunk;
    }

    private bool CreateBlock(int x, int y, int z)
    {
        bool isNewChunk;
        int chunkX = ToChunkFromWorld(x, out int blockX);
        int chunkY = ToChunkFromWorld(y, out int blockY);
        int chunkZ = ToChunkFromWorld(z, out int blockZ);
        isNewChunk = !TryGetChunk(chunkX, chunkY, chunkZ, out Chunk chunk);
        if (isNewChunk) chunk = NewChunk(chunkX, chunkY, chunkZ);
        chunk.NewBlockInWorld(blockX, blockY, blockZ);
        return isNewChunk;

    }

    private void CreateChunkPillar(int xc, int zc)
    {
        int[,] heightMap = new int[Chunk.SIZE, Chunk.SIZE];
        float scale = 0.5f;
        int heightTotalChunks = 4;
        int heightTotalBlocks = Chunk.SIZE * heightTotalChunks - 1;

        for (int yc = 0; yc < heightTotalChunks; yc++)
            NewChunk(xc, yc, zc);

        //Only highest points
        for (int xb = 0; xb < Chunk.SIZE; xb++)
            for (int zb = 0; zb < Chunk.SIZE; zb++)
            {
                int yb = GenY(xc, zc, xb, zb);
                NewBlockInPillar(xb, yb, zb);
                heightMap[xb, zb] = yb;
            }

        //Fill gaps in corners
        FillGap(0, 0,
            GenY(xc - 1, zc, Chunk.SUBSIZE, 0),
            heightMap[1, 0],
            GenY(xc, zc - 1, 0, Chunk.SUBSIZE),
            heightMap[0, 1]);

        FillGap(0, Chunk.SUBSIZE,
            GenY(xc - 1, zc, Chunk.SUBSIZE, Chunk.SUBSIZE),
            heightMap[1, Chunk.SUBSIZE],
            heightMap[0, Chunk.SUBSIZE - 1],
            GenY(xc, zc + 1, 0, 0));

        FillGap(Chunk.SUBSIZE, 0,
            heightMap[Chunk.SUBSIZE - 1, 0],
            GenY(xc + 1, zc, 0, 0),
            GenY(xc, zc - 1, Chunk.SUBSIZE, Chunk.SUBSIZE),
            heightMap[Chunk.SUBSIZE, 1]);

        FillGap(Chunk.SUBSIZE, Chunk.SUBSIZE,
            heightMap[Chunk.SUBSIZE - 1, Chunk.SUBSIZE],
            GenY(xc + 1, zc, 0, Chunk.SUBSIZE),
            heightMap[Chunk.SUBSIZE, Chunk.SUBSIZE - 1],
            GenY(xc, zc + 1, Chunk.SUBSIZE, 0));

        //Fill gaps in sides
        for (int off = 1; off < Chunk.SUBSIZE; off++)
        {
            FillGap(0, off,
                GenY(xc - 1, zc, Chunk.SUBSIZE, off),
                heightMap[1, off],
                heightMap[0, off - 1],
                heightMap[0, off + 1]);

            FillGap(Chunk.SUBSIZE, off,
                heightMap[Chunk.SUBSIZE - 1, off],
                GenY(xc + 1, zc, 0, off),
                heightMap[Chunk.SUBSIZE, off - 1],
                heightMap[Chunk.SUBSIZE, off + 1]);

            FillGap(off, 0,
                heightMap[off - 1, 0],
                heightMap[off + 1, 0],
                GenY(xc, zc - 1, off, Chunk.SUBSIZE),
                heightMap[off, 1]);

            FillGap(off, Chunk.SUBSIZE,
                heightMap[off - 1, Chunk.SUBSIZE],
                heightMap[off + 1, Chunk.SUBSIZE],
                heightMap[off, Chunk.SUBSIZE - 1],
                GenY(xc, zc + 1, off, 0));
        }

        //Fill gaps in center
        for (int xb = 1; xb < Chunk.SUBSIZE; xb++)
            for (int zb = 1; zb < Chunk.SUBSIZE; zb++)
                FillGap(xb, zb,
                    heightMap[xb - 1, zb],
                    heightMap[xb + 1, zb],
                    heightMap[xb, zb - 1],
                    heightMap[xb, zb + 1]);

        int GenY(int xc, int zc, int xb, int zb) =>
            (int)MathF.Round(Mathf.Clamp01(
                Mathf.PerlinNoise(
                    (xc - HSIZE + xb / (float)Chunk.SIZE) * scale,
                    (zc - HSIZE + zb / (float)Chunk.SIZE) * scale))
                * heightTotalBlocks, MidpointRounding.AwayFromZero);

        void FillGap(int xb, int zb, int side0, int side1, int side2, int side3)
        {
            int yCurr = heightMap[xb, zb];
            int yMin = Math.Min(
                Math.Min(side0, side1),
                Math.Min(side2, side3));

            yCurr--;
            while (yCurr > yMin)
            {
                NewBlockInPillar(xb, yCurr, zb);
                yCurr--;
            }
        }

        Block NewBlockInPillar(int xb, int yb, int zb) =>
            _chunks[xc, ToChunkFromArray(yb, out yb), zc].NewBlockInArray(xb, yb, zb);
    }

    private int ToChunkFromWorld(int index, out int block)
    {
        index -= Chunk.START;
        index = Math.DivRem(index, Chunk.SIZE, out block);
        if (block < 0)
        {
            index--;
            block += Chunk.SIZE;
        }
        block += Chunk.START;
        return index;
    }

    private int ToChunkFromArray(int index, out int block)
    {
        index = Math.DivRem(index, Chunk.SIZE, out block);
        if (block < 0)
        {
            index--;
            block += Chunk.SIZE;
        }
        return index;
    }

    private void FillChunk(Chunk chunk)
    {
        for (int row = 0; row < Chunk.SIZE; row++)
            for (int col = 0; col < Chunk.SIZE; col++)
                chunk.NewBlockInWorld(row, 0, col);
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(World))]
    public class Editor : UnityEditor.Editor
    {
        private const string GENERATE = "Generate";
        private const string CHUNK_AT_INDICES = nameof(Chunk) + " At Indices";
        private const string GENERATE_CHUNK_AT_INDICES = GENERATE + " " + CHUNK_AT_INDICES + ":";
        private Vector3Int _chunkIndeces = Vector3Int.zero;

        public override void OnInspectorGUI()
        {
            World world = (World)target;

            EditorGUILayout.LabelField(GENERATE_CHUNK_AT_INDICES);
            EditorGUILayout.BeginHorizontal();
            _chunkIndeces = EditorGUILayout.Vector3IntField(GUIContent.none, _chunkIndeces);
            if (GUILayout.Button(GENERATE))
                world.FillChunk(world.NewChunk(_chunkIndeces.x, _chunkIndeces.y, _chunkIndeces.z));
            EditorGUILayout.EndHorizontal();

            base.OnInspectorGUI();
        }
    }
#endif
}