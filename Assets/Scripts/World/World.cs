using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class World : Singleton<World>
{
    public const int SIZE = 8;
    public const int HSIZE = SIZE / 2;
    public const int SUBSIZE = SIZE - 1;

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
        _chunks = new OffsetArray3<Chunk>(-HSIZE, SIZE, -HSIZE, SIZE, -HSIZE, SIZE);
        Seed = 1231411241212412314;
    }

    private void Start()
    {
        int side = 40;
        for (int x = -side; x < side; x++)
        {
            for (int z = -side; z < side; z++)
            {
                int y = (int)MathF.Round(Mathf.PerlinNoise((float)x / SIZE, (float)z / SIZE), MidpointRounding.AwayFromZero);
                NewBlock(x, y, z);
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

    private void NewBlock(int x, int y, int z)
    {
        int chunkX = ToChunk(x, out int blockX);
        int chunkY = ToChunk(y, out int blockY);
        int chunkZ = ToChunk(z, out int blockZ);
        if (!TryGetChunk(chunkX, chunkY, chunkZ, out Chunk chunk))
            chunk = NewChunk(chunkX, chunkY, chunkZ);
        chunk.NewBlock(blockX, blockY, blockZ);

        static int ToChunk(int index, out int block)
        {
            index -= Chunk.START;
            index = Math.DivRem(index, Chunk.SIZE, out block);
            //print(block);
            block += index > 0 ? Chunk.START : -Chunk.START;
            print((index, block));
            return index;
        }
    }

    private void FillChunk(Chunk chunk)
    {
        for (int row = 0; row < Chunk.SIZE; row++)
            for (int col = 0; col < Chunk.SIZE; col++)
                chunk.NewBlock(row, 0, col);
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