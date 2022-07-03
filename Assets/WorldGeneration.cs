using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class WorldGeneration : MonoBehaviour
{
    private static readonly Vector3[] _verticies;
    private static readonly Vector3[] _verticiesReversed;
    private static readonly int[] _triangles = { 0, 1, 2 };
    private Mesh _mesh;

    static WorldGeneration()
    {
        float sqrt3 = Mathf.Sqrt(3);
        float Sqrt3Div3 = sqrt3 / 3;
        float sqrt3Div6 = sqrt3 / 6;
        _verticies = new Vector3[] { new(-0.5f, 0, -sqrt3Div6), new(0, 0, Sqrt3Div3), new(0.5f, 0, -sqrt3Div6) };
        _verticiesReversed = new Vector3[] { new(0.5f, 0, sqrt3Div6), new(0, 0, -Sqrt3Div3), new(-0.5f, 0, sqrt3Div6) };
    }

    private void Awake()
    {
        _mesh = new()
        {
            vertices = _verticies,
            triangles = _triangles
        };

        GameObject @object = new();
        @object.transform.parent = transform;
        var meshFilter = @object.AddComponent<MeshFilter>();
        meshFilter.mesh = _mesh;
        var meshRenderer = @object.AddComponent<MeshRenderer>();
    }
}
