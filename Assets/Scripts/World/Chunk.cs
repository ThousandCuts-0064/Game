using System;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public const int SIZE = 32;
    public const int HSIZE = SIZE / 2;
    public const int SUBSIZE = SIZE - 1;
    public const int START = -HSIZE;
    public const int END = START + SIZE - 1;

    private Block[,,] _blocks;
    public Chunk Forth { get; private set; }
    public Chunk Back { get; private set; }
    public Chunk Right { get; private set; }
    public Chunk Left { get; private set; }
    public Chunk Up { get; private set; }
    public Chunk Down { get; private set; }

    public static void Initialize()
    {
        World.ChunkCreated += (chunk, x, y, z) =>
        {
            if (World.TryGetChunk(x, y, z + 1, out Chunk neighbor))
            {
                neighbor.Back = chunk;
                chunk.Forth = neighbor;
            }

            if (World.TryGetChunk(x, y, z - 1, out neighbor))
            {
                neighbor.Forth = chunk;
                chunk.Back = neighbor;
            }

            if (World.TryGetChunk(x + 1, y, z, out neighbor))
            {
                neighbor.Left = chunk;
                chunk.Right = neighbor;
            }

            if (World.TryGetChunk(x - 1, y, z, out neighbor))
            {
                neighbor.Right = chunk;
                chunk.Left = neighbor;
            }

            if (World.TryGetChunk(x, y + 1, z, out neighbor))
            {
                neighbor.Down = chunk;
                chunk.Up = neighbor;
            }

            if (World.TryGetChunk(x, y - 1, z, out neighbor))
            {
                neighbor.Up = chunk;
                chunk.Down = neighbor;
            }
        };
    }

    private void Awake()
    {
        _blocks = new Block[SIZE, SIZE, SIZE];
    }

    public Block NewInvisBlock(int x, int y, int z)
    {
        var block = Instantiate(World.Get.Block, transform)
            .SetLocalPosition(new(x, y, z))
            .ReplaceClone((x, y, z).ToString());
        try { _blocks[x - START, y - START, z - START] = block; }
        catch { print((x - START, y - START, z - START)); }
        return block;
    }

    public Block NewBlock(int x, int y, int z)
    {
        var block = NewInvisBlock(x, y, z);
        x -= START;
        y -= START;
        z -= START;
        block.Faces = CalcFace(x, y, z);
        UpdateBlockNeighbors(x, y, z);

        return block;
    }

    private Direction CalcFace(int x, int y, int z)
    {
        Direction faces = Direction.None;
        int next;

        next = z + 1;
        if (next < SIZE ? !_blocks[x, y, next]
            : !Forth || !Forth._blocks[x, y, 0])
            faces |= Direction.Forth;

        next = z - 1;
        if (next >= 0 ? !_blocks[x, y, next]
            : !Back || !Back._blocks[x, y, SUBSIZE])
            faces |= Direction.Back;

        next = x + 1;
        if (next < SIZE ? !_blocks[next, y, z]
            : !Right || !Right._blocks[0, y, z])
            faces |= Direction.Right;

        next = x - 1;
        if (next >= 0 ? !_blocks[next, y, z]
            : !Left || !Left._blocks[SUBSIZE, y, z])
            faces |= Direction.Left;

        next = y + 1;
        if (next < SIZE ? !_blocks[x, next, z]
            : !Up || Up._blocks[x, 0, z])
            faces |= Direction.Up;

        next = y - 1;
        if (next >= 0 ? !_blocks[x, next, z]
            : !Down || !Down._blocks[x, SUBSIZE, z])
            faces |= Direction.Down;

        return faces;
    }

    private void UpdateBlockNeighbors(int x, int y, int z)
    {
        Block neighbor;
        int next;

        next = z + 1;
        neighbor = next < SIZE ? _blocks[x, y, next]
            : Forth ? Forth._blocks[x, y, 0] : null;
        if (neighbor) neighbor.Faces &= ~Direction.Back;

        next = z - 1;
        neighbor = next >= 0 ? _blocks[x, y, next]
            : Back ? Back._blocks[x, y, SUBSIZE] : null;
        if (neighbor) neighbor.Faces &= ~Direction.Forth;

        next = x + 1;
        neighbor = next < SIZE ? _blocks[next, y, z]
            : Right ? Right._blocks[0, y, z] : null;
        if (neighbor) neighbor.Faces &= ~Direction.Left;

        next = x - 1;
        neighbor = next >= 0 ? _blocks[next, y, z] :
            Left ? Left._blocks[SUBSIZE, y, z] : null;
        if (neighbor) neighbor.Faces &= ~Direction.Right;

        next = y + 1;
        neighbor = next < SIZE ? _blocks[x, next, z] :
            Up ? Up._blocks[x, 0, z] : null;
        if (neighbor) neighbor.Faces &= ~Direction.Down;

        next = y - 1;
        neighbor = next >= 0 ? _blocks[x, next, z] :
            Down ? Down._blocks[x, SUBSIZE, z] : null;
        if (neighbor) neighbor.Faces &= ~Direction.Up;
    }
}
