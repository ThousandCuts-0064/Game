using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class WorldGeneration : MonoBehaviour
{
    private const float SQRT_3 = 1.732050807568877293527446341505872366942805253810380628055806f;
    private const float SIDE = 1;
    private const float HALF_SIDE = SIDE / 2;
    private const float R_IN = SIDE * SQRT_3 / 6;
    private const float R_CIRCUM = SIDE * SQRT_3 / 3;
    private const float H = SIDE * SQRT_3 / 2;

    private const int SIDE_UNITS = 147;
    private const float SIDE_LENGTH = SIDE * SIDE_UNITS;
    private const float HALF_SIDE_LENGTH = SIDE_LENGTH / 2;
    private const int VERTICIES_COUNT = 3 * SIDE_UNITS * (SIDE_UNITS + 1) + 1;
    private const int TRIANGLES_TOTAL = 6 * SIDE_UNITS * SIDE_UNITS;

    private static readonly Vector3[] _verticies = new Vector3[VERTICIES_COUNT];
    private static readonly int[] _triangles = new int[TRIANGLES_TOTAL];
    private static GameObject _gameObject;
    [SerializeField] private Material _material;

    private void Awake()
    {
        //_gameObject = new();
        //_gameObject.AddComponent<MeshFilter>().mesh = new()
        //{
        //    vertices = _verticies,
        //    triangles = _triangles
        //};
        //_gameObject.AddComponent<MeshRenderer>();
        //_gameObject.transform.parent = transform;

        int vertIndex = 0;
        int offset = 1;
        for (int row = 0; row < 2 * SIDE_UNITS + 1; row++)
        {
            for (int col = 0; col < SIDE_UNITS + offset; col++)
                _verticies[vertIndex++] = new(-HALF_SIDE_LENGTH - offset * HALF_SIDE + col * SIDE, 0, -H * SIDE_UNITS + row * H);
            offset += row < SIDE_UNITS ? 1 : -1;
        }

        for (int i = 0; i < VERTICIES_COUNT; i++)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            go.transform.parent = transform;
            go.transform.localPosition = _verticies[i];
            //go.transform.localScale = new(0.1f, 0.1f, 0.1f);
        }
    }
}
