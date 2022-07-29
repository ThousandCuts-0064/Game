using System;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public const int SIZE = 32;
    public const int HSIZE = SIZE / 2;
    public static Chunk Empty { get; private set; }
    private Block[,,] _blocks;

    public static void Initialize()
    {
        Empty = new GameObject(nameof(Chunk)).AddComponent<Chunk>();
        Destroy(Empty.gameObject);
    }

    private void Awake()
    {
        _blocks = new Block[SIZE, SIZE, SIZE];
    }

    public Block NewInvisBlockAt(int x, int y, int z)
    {
        var block = Instantiate(Block.Empty, new(x - HSIZE, y - HSIZE, z - HSIZE), Quaternion.identity, transform)
            .ReplaceClone((x, y, z).ToString());
        _blocks[x, y, z] = block;
        return block;
    }

    public Block NewBlockAt(int x, int y, int z)
    {
        var block = NewInvisBlockAt(x, y, z);
        block.Faces = CalcFace(x, y, z);
        UpdateBlockNeighbors(x, y, z);

        return block;
    }

    private Direction CalcFace(int x, int y, int z)
    {
        Direction faces = Direction.None;
        int next;

        next = z + 1;
        if (next < SIZE && !_blocks[x, y, next]) faces ^= Direction.Forth;

        next = z - 1;
        if (next > 0 && !_blocks[x, y, next]) faces ^= Direction.Back;

        next = x + 1;
        if (next < SIZE && !_blocks[next, y, z]) faces ^= Direction.Right;

        next = x - 1;
        if (next > 0 && !_blocks[next, y, z]) faces ^= Direction.Left;

        next = y + 1;
        if (next < SIZE && !_blocks[x, next, z]) faces ^= Direction.Up;

        next = y - 1;
        if (next > 0 && !_blocks[x, next, z]) faces ^= Direction.Down;

        return faces;
    }

    private void UpdateBlockNeighbors(int x, int y, int z)
    {
        Block neighbor;
        int next;

        next = z + 1;
        if (next < SIZE)
        {
            neighbor = _blocks[x, y, next];
            if (neighbor) neighbor.Faces &= ~Direction.Back;
        }

        next = z - 1;
        if (next >= 0)
        {
            neighbor = _blocks[x, y, next];
            if (neighbor) neighbor.Faces &= ~Direction.Forth;
        }

        next = x + 1;
        if (next < SIZE)
        {
            neighbor = _blocks[next, y, z];
            if (neighbor) neighbor.Faces &= ~Direction.Left;
        }

        next = x - 1;
        if (next >= 0)
        {
            neighbor = _blocks[next, y, z];
            if (neighbor) neighbor.Faces &= ~Direction.Right;
        }

        next = y + 1;
        if (next < SIZE)
        {
            neighbor = _blocks[x, next, z];
            if (neighbor) neighbor.Faces &= ~Direction.Down;
        }

        next = y - 1;
        if (next >= 0)
        {
            neighbor = _blocks[x, next, z];
            if (neighbor) neighbor.Faces &= ~Direction.Up;
        }
    }
}
