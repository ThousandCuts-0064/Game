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
        StartCoroutine(ChunkCreator());
    }

    private IEnumerator ChunkCreator()
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

    private void CreateChunkPillar(int x, int z)
    {
        Block[,] blocks2D = new Block[Chunk.SIZE, Chunk.SIZE];
        float scale = 0.5f;
        int heightTotalChunks = 4;
        int heightTotalBlocks = Chunk.SIZE * heightTotalChunks - 1;

        for (int y = 0; y < heightTotalChunks; y++)
            NewChunk(x, y, z);

        for (int xb = 0; xb < Chunk.SIZE; xb++)
        {
            for (int zb = 0; zb < Chunk.SIZE; zb++)
            {
                int height = (int)MathF.Round(
                    Mathf.PerlinNoise(
                    (x - HSIZE + xb / (float)Chunk.SIZE) * scale,
                    (z - HSIZE + zb / (float)Chunk.SIZE) * scale)
                    * heightTotalBlocks, MidpointRounding.AwayFromZero);
                blocks2D[xb, zb] = _chunks[x, ToChunkFromArray(height, out int block), z].NewBlockInArray(xb, block, zb);
            }
        }

        for (int xb = 1; xb < Chunk.SUBSIZE; xb++)
        {
            for (int zb = 1; zb < Chunk.SUBSIZE; zb++)
            {
                int yDiff = (int)Math.Round(blocks2D[xb, zb].transform.position.y - blocks2D[xb - 1, zb].transform.position.y, MidpointRounding.AwayFromZero);
                for (int y = 0; y < yDiff; y++)
                {

                }
            }
        }
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