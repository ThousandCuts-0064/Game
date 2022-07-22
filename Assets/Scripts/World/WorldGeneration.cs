using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class WorldGeneration : MonoBehaviour
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

    private static List<Chunk> _southEastUp;
    private static Vector3[] _verticies;
    private static int[] _triangles;
    private static GameObject _gameObject;
    [SerializeField] private Material _material;

    private void Awake()
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

        _gameObject = new();
        Mesh mesh = new()
        {
            vertices = _verticies,
            triangles = _triangles
        };
        mesh.RecalculateNormals();
        _gameObject.AddComponent<MeshFilter>().mesh = mesh;
        _gameObject.AddComponent<MeshRenderer>().material = _material;
        Destroy(_gameObject);
    }

    private void Start()
    {

    }
}
