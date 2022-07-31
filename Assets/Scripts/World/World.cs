using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class World : MonoBehaviour
{
    private static Dictionary<Vector3Int, Chunk> _chunks;
    [SerializeField] private Material _material;

    public static event Action<Chunk, Vector3Int> ChunkCreated;

    private void Awake()
    {
        Chunk.Initialize();
        Block.Initialize();
        _chunks = new();
    }

    private void Start()
    {
        Vector3Int indecies = new(0, 0, 0);
        var chunk = NewChunkAtIndecies(indecies);
        ChunkCreated(chunk, indecies);
        FillChunk(chunk);
    }

    private Chunk NewChunkAtIndecies(Vector3Int indecies)
    {
        var chunk = Instantiate(Chunk.Empty, indecies * Chunk.SIZE, Quaternion.identity, transform)
            .ReplaceClone(indecies.ToString());
        try
        {
            _chunks.Add(indecies, chunk);
        }
        catch (ArgumentException e) when (e.StackTrace.Contains(typeof(Dictionary<,>).Name))
        {
            throw new ChunkAlreadyExistsException($"{nameof(Chunk)} at {indecies} already exists");
        }

        return chunk;
    }

    private void FillChunk(Chunk chunk)
    {
        for (int row = 0; row < Chunk.SIZE; row++)
            for (int col = 0; col < Chunk.SIZE; col++)
                chunk.NewBlockAt(row, 0, col);
    }

    public static Chunk GetChunkAtIndices(int x, int y, int z) => _chunks[new(x, y, z)];
    public static Chunk GetChunkAtIndices(Vector3Int indices) => _chunks[indices];
    public static Chunk GetChunkAtWorld(Vector3Int position) => _chunks[position / Chunk.SIZE];
    public static bool TryGetChunkAtIndices(int x, int y, int z, out Chunk chunk) => _chunks.TryGetValue(new(x, y, z), out chunk);
    public static bool TryGetChunkAtIndices(Vector3Int indices, out Chunk chunk) => _chunks.TryGetValue(indices, out chunk);
    public static bool TryGetChunkAtWorld(Vector3Int position, out Chunk chunk) => _chunks.TryGetValue(position / Chunk.SIZE, out chunk);

#if UNITY_EDITOR
    [CustomEditor(typeof(World))]
    public class Editor : UnityEditor.Editor
    {
        private const string GENERATE = "Generate";
        private const string CHUNK_AT_INDECES = "Chunk At Indices";
        private const string GENERATE_CHUNK_AT_INDICES = GENERATE + " " + CHUNK_AT_INDECES;
        private Vector3Int _chunkIndeces = Vector3Int.zero;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            World world = (World)target;

            EditorGUILayout.LabelField(GENERATE_CHUNK_AT_INDICES);
            EditorGUILayout.BeginHorizontal();
            _chunkIndeces = EditorGUILayout.Vector3IntField(GUIContent.none, _chunkIndeces);
            if (GUILayout.Button(GENERATE))
                world.FillChunk(world.NewChunkAtIndecies(_chunkIndeces));
            EditorGUILayout.EndHorizontal();
        }
    }
#endif
}