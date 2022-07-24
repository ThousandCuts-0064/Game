using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class WorldGeneration : MonoBehaviour
{
    private static Dictionary<Vector3Int, Chunk> _chunks;
    [SerializeField] private Material _material;

    private void Awake()
    {
        Chunk.Initialize();
        Block.Initialize();
        _chunks = new();
    }

    private void Start()
    {
        Vector3Int position = new(0, 0, 0);
        var chunk = Instantiate(Chunk.Empty, position, Quaternion.identity, transform);
        try 
        {
            _chunks.Add(position / Chunk.SIZE, chunk);
        }
        catch (ArgumentException e) when (e.StackTrace.Contains(typeof(Dictionary<,>).Name))
        {
            throw new ChunkAlreadyExistsException($"{nameof(Chunk)} at {position} already exists"); 
        }

        for (int row = 0; row < Chunk.SIZE; row++)
        {
            for (int col = 0; col < Chunk.SIZE; col++)
            {
                chunk.GenerateBlockAt(row, 0, col);
            }
        }
    }

    public static Chunk ChunkAtWorld(Vector3Int position) => _chunks[position];
    public static Chunk ChunkAtIndices(Vector3Int position) => _chunks[position / Chunk.SIZE];
}
