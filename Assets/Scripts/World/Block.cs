using System;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Block : MonoBehaviour
{
    public const float SIDE = 1f;
    public const float HSIDE = SIDE / 2;
    public const int TRIANGLE_SIDES = 3;
    public const int TRIANGLES_IN_SQUARE = 2;
    public const int SQUARE_VERTICES = 4;
    public const int CUBE_SIDES = 6;
    public const int SQUARE_TOTAL_VERTICIES = TRIANGLES_IN_SQUARE * TRIANGLE_SIDES;
    public const int CUBE_VERTICES = CUBE_SIDES * SQUARE_VERTICES;
    public const int CUBE_TRIANGLES = CUBE_SIDES * TRIANGLES_IN_SQUARE;
    public const int CUBE_TRIANGLES_SIDES = CUBE_TRIANGLES * TRIANGLE_SIDES;

    public static Block Empty { get; private set; }
    private static Vector3[] _verticies;
    private static int[] _triangles;

    private MeshRenderer _meshRenderer;
    private MeshFilter _meshFilter;
    private Direction _faces;

    public Direction Faces
    {
        get => _faces;
        set
        {
            _faces = value;
            _meshFilter.mesh = null;
        }
    }

    public static void Initialize()
    {
        _verticies = new Vector3[CUBE_VERTICES]
        {
            new( HSIDE, -HSIDE,  HSIDE), new( HSIDE,  HSIDE,  HSIDE), new(-HSIDE, -HSIDE,  HSIDE), new(-HSIDE,  HSIDE,  HSIDE),
            new(-HSIDE, -HSIDE, -HSIDE), new(-HSIDE,  HSIDE, -HSIDE), new( HSIDE, -HSIDE, -HSIDE), new( HSIDE,  HSIDE, -HSIDE),

            new( HSIDE, -HSIDE, -HSIDE), new( HSIDE,  HSIDE, -HSIDE), new( HSIDE, -HSIDE,  HSIDE), new( HSIDE,  HSIDE,  HSIDE),
            new(-HSIDE, -HSIDE,  HSIDE), new(-HSIDE,  HSIDE,  HSIDE), new(-HSIDE, -HSIDE, -HSIDE), new(-HSIDE,  HSIDE, -HSIDE),

            new(-HSIDE,  HSIDE, -HSIDE), new(-HSIDE,  HSIDE,  HSIDE), new( HSIDE,  HSIDE, -HSIDE), new( HSIDE,  HSIDE,  HSIDE),
            new(-HSIDE, -HSIDE,  HSIDE), new(-HSIDE, -HSIDE, -HSIDE), new( HSIDE, -HSIDE,  HSIDE), new( HSIDE, -HSIDE, -HSIDE)
        };

        _triangles = new int[CUBE_TRIANGLES_SIDES];
        for (int side = 0; side < CUBE_SIDES; side++)
        {
            var trig = side * SQUARE_TOTAL_VERTICIES;
            var vert = side * SQUARE_VERTICES;

            _triangles[trig + 0] = vert + 0;
            _triangles[trig + 1] = vert + 1;
            _triangles[trig + 2] = vert + 2;

            _triangles[trig + 3] = vert + 3;
            _triangles[trig + 4] = vert + 2;
            _triangles[trig + 5] = vert + 1;
        }

        Empty = new GameObject().AddComponent<Block>();
        Destroy(Empty);
    }

    private void Awake()
    {
        _meshFilter = GetComponent<MeshFilter>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshFilter.mesh = new()
        {
            vertices = _verticies,
            triangles = _triangles
        };
    }
}