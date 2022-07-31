using System;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Block : MonoBehaviour
{
    public const float SIDE = 1f;
    public const float HSIDE = SIDE / 2;
    public const int TRIANGLE_SIDES = 3;
    public const int TRIANGLES_IN_SQUARE = 2;
    public const int SQUARE_VERTICES = 4;
    public const int SIDES = 6;
    public const int SQUARE_TOTAL_LINES = TRIANGLES_IN_SQUARE * TRIANGLE_SIDES;
    public const int VERTICES = SIDES * SQUARE_VERTICES;
    public const int TRIANGLES = SIDES * TRIANGLES_IN_SQUARE;
    public const int TRIANGLES_SIDES = TRIANGLES * TRIANGLE_SIDES;

    public static Block Empty { get; private set; }
    private static Vector3[] _verticies;
    private static int[][] _triangles;

    private MeshRenderer _meshRenderer;
    private MeshFilter _meshFilter;
    [SerializeField] private Direction _faces;

    public Direction Faces
    {
        get => _faces;
        set
        {
            _faces = value;
            _meshFilter.mesh.triangles = _triangles[(int)value];
        }
    }

    public static void Initialize()
    {
        _verticies = new Vector3[VERTICES]
        {
            new( HSIDE, -HSIDE,  HSIDE), new( HSIDE,  HSIDE,  HSIDE), new(-HSIDE, -HSIDE,  HSIDE), new(-HSIDE,  HSIDE,  HSIDE),
            new(-HSIDE, -HSIDE, -HSIDE), new(-HSIDE,  HSIDE, -HSIDE), new( HSIDE, -HSIDE, -HSIDE), new( HSIDE,  HSIDE, -HSIDE),

            new( HSIDE, -HSIDE, -HSIDE), new( HSIDE,  HSIDE, -HSIDE), new( HSIDE, -HSIDE,  HSIDE), new( HSIDE,  HSIDE,  HSIDE),
            new(-HSIDE, -HSIDE,  HSIDE), new(-HSIDE,  HSIDE,  HSIDE), new(-HSIDE, -HSIDE, -HSIDE), new(-HSIDE,  HSIDE, -HSIDE),

            new(-HSIDE,  HSIDE, -HSIDE), new(-HSIDE,  HSIDE,  HSIDE), new( HSIDE,  HSIDE, -HSIDE), new( HSIDE,  HSIDE,  HSIDE),
            new(-HSIDE, -HSIDE,  HSIDE), new(-HSIDE, -HSIDE, -HSIDE), new( HSIDE, -HSIDE,  HSIDE), new( HSIDE, -HSIDE, -HSIDE)
        };

        _triangles = new int[Const.DIRECTIONS_ALL_COUNT][];
        _triangles[0] = Array.Empty<int>();

        for (int dir = 1; dir < Const.DIRECTIONS_ALL_COUNT; dir++)
        {
            _triangles[dir] = new int[dir.CountBits() * SQUARE_TOTAL_LINES];
        }

        for (int side = 0; side < SIDES; side++)
        {
            var trig = side * SQUARE_TOTAL_LINES;
            var vert = side * SQUARE_VERTICES;

            _triangles[Const.DIRECTIONS_ALL_INT][trig + 0] = vert + 0;
            _triangles[Const.DIRECTIONS_ALL_INT][trig + 1] = vert + 1;
            _triangles[Const.DIRECTIONS_ALL_INT][trig + 2] = vert + 2;

            _triangles[Const.DIRECTIONS_ALL_INT][trig + 3] = vert + 3;
            _triangles[Const.DIRECTIONS_ALL_INT][trig + 4] = vert + 2;
            _triangles[Const.DIRECTIONS_ALL_INT][trig + 5] = vert + 1;
        }

        for (int dir = 1; dir < Const.DIRECTIONS_ALL_INT; dir++)
        {
            var bits = dir.SeparateBits(stackalloc int[Const.INT_BITS]);
            int bitCount = 0;
            for (int bit = 0; bit < bits.Length; bit++)
            {
                if (bits[bit] == 0) continue;

                int sideTo = bitCount * SQUARE_TOTAL_LINES;
                int sideFrom = bit * SQUARE_TOTAL_LINES;
                bitCount++;
                for (int vert = 0; vert < SQUARE_TOTAL_LINES; vert++)
                {
                    _triangles[dir][sideTo + vert] = _triangles[Const.DIRECTIONS_ALL_INT][sideFrom + vert];
                }
            }
        }

        Empty = new GameObject(nameof(Block)).AddComponent<Block>();
        Destroy(Empty.gameObject);
    }

    private void Awake()
    {
        _meshFilter = GetComponent<MeshFilter>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshFilter.mesh = new()
        {
            vertices = _verticies,
            triangles = _triangles[0]
        };
    }
}