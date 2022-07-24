using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;

public class Chunk : MonoBehaviour
{
    public const int SIZE = 32;
    public static Chunk Empty { get; private set; }
    private Block[,,] _blocks;

    public static void Initialize()
    {
        Empty = new GameObject().AddComponent<Chunk>();
        Destroy(Empty);
    }

    private void Awake()
    {
        _blocks = new Block[SIZE, SIZE, SIZE];
    }

    public void GenerateBlockAt(int x, int y, int z)
    {
        _blocks[x, y, z] = Instantiate(Block.Empty, new(x, y, z), Quaternion.identity, transform);
    }
}
