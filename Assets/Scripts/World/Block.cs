using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Block : MonoBehaviour
{
    public const float SIDE = 1f;
    public const float HSIDE = SIDE / 2;
    public const int SIDES = 6;
    public const int TRIANGLES = SIDES * Square.TRIANGLES;
    public const int VERTICES_SIDES = SIDES * Square.VERTICES;

    private static IReadOnlyList<Material> _materials;
    private static Vector3[] _verticies;
    private static int[][] _triangles;

    private MeshRenderer _meshRenderer;
    private MeshFilter _meshFilter;
    [SerializeField] private Direction _faces;

    public Chunk Chunk { get; private set; }
    public Direction Faces
    {
        get => _faces;
        set
        {
            _faces = value;
            _meshFilter.mesh.triangles = _triangles[(int)value];
            _meshFilter.mesh.RecalculateNormals(
                UnityEngine.Rendering.MeshUpdateFlags.DontValidateIndices
                | UnityEngine.Rendering.MeshUpdateFlags.DontResetBoneBounds
                | UnityEngine.Rendering.MeshUpdateFlags.DontNotifyMeshUsers
                | UnityEngine.Rendering.MeshUpdateFlags.DontRecalculateBounds);
        }
    }

    public int X => (int)transform.position.x;
    public int Y => (int)transform.position.y;
    public int Z => (int)transform.position.z;

    public int X0 => X - Chunk.START;
    public int Y0 => Y - Chunk.START;
    public int Z0 => Z - Chunk.START;

    public static void Initialize(IReadOnlyList<Material> materials)
    {
        _materials = materials;

        _verticies = new Vector3[VERTICES_SIDES]
        {
            new( HSIDE, -HSIDE,  HSIDE), new( HSIDE,  HSIDE,  HSIDE), new(-HSIDE, -HSIDE,  HSIDE), new(-HSIDE,  HSIDE,  HSIDE),
            new(-HSIDE, -HSIDE, -HSIDE), new(-HSIDE,  HSIDE, -HSIDE), new( HSIDE, -HSIDE, -HSIDE), new( HSIDE,  HSIDE, -HSIDE),

            new( HSIDE, -HSIDE, -HSIDE), new( HSIDE,  HSIDE, -HSIDE), new( HSIDE, -HSIDE,  HSIDE), new( HSIDE,  HSIDE,  HSIDE),
            new(-HSIDE, -HSIDE,  HSIDE), new(-HSIDE,  HSIDE,  HSIDE), new(-HSIDE, -HSIDE, -HSIDE), new(-HSIDE,  HSIDE, -HSIDE),

            new(-HSIDE,  HSIDE, -HSIDE), new(-HSIDE,  HSIDE,  HSIDE), new( HSIDE,  HSIDE, -HSIDE), new( HSIDE,  HSIDE,  HSIDE),
            new(-HSIDE, -HSIDE,  HSIDE), new(-HSIDE, -HSIDE, -HSIDE), new( HSIDE, -HSIDE,  HSIDE), new( HSIDE, -HSIDE, -HSIDE)
        };

        _triangles = new int[Const.DIRECTIONS_COUNT][];
        _triangles[0] = Array.Empty<int>();

        for (int dir = 1; dir < Const.DIRECTIONS_COUNT; dir++)
        {
            _triangles[dir] = new int[MathX.CountBits(dir) * Square.VERTICES_TRIANGLES];
        }

        for (int side = 0; side < SIDES; side++)
        {
            int trig = side * Square.VERTICES_TRIANGLES;
            int vert = side * Square.VERTICES;

            _triangles[Const.DIRECTIONS_ALL][trig + 0] = vert + 0;
            _triangles[Const.DIRECTIONS_ALL][trig + 1] = vert + 1;
            _triangles[Const.DIRECTIONS_ALL][trig + 2] = vert + 2;

            _triangles[Const.DIRECTIONS_ALL][trig + 3] = vert + 3;
            _triangles[Const.DIRECTIONS_ALL][trig + 4] = vert + 2;
            _triangles[Const.DIRECTIONS_ALL][trig + 5] = vert + 1;
        }

        for (int dir = 1; dir < Const.DIRECTIONS_ALL; dir++)
        {
            var bits = MathX.SeparateBits(dir, stackalloc int[Const.INT_BITS]);
            int bitCount = 0;
            for (int bit = 0; bit < bits.Length; bit++)
            {
                if (bits[bit] == 0) continue;

                int sideTo = bitCount * Square.VERTICES_TRIANGLES;
                int sideFrom = bit * Square.VERTICES_TRIANGLES;
                bitCount++;
                for (int vert = 0; vert < Square.VERTICES_TRIANGLES; vert++)
                {
                    _triangles[dir][sideTo + vert] = _triangles[Const.DIRECTIONS_ALL][sideFrom + vert];
                }
            }
        }
    }

    private void Awake()
    {
        UpdateChunk();
        _meshFilter = GetComponent<MeshFilter>();
        _meshRenderer = GetComponent<MeshRenderer>();
        
        _meshFilter.mesh = new()
        {
            vertices = _verticies,
            triangles = _triangles[0]
        };
        _meshRenderer.material = _materials[0];
    }

    public void UpdateChunk() =>
        Chunk = transform.parent.GetComponent<Chunk>();
}