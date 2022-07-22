using System;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Block : MonoBehaviour
{
    private Direction _faces;
    private MeshFilter _meshFilter;
    private MeshRenderer _meshRenderer;
    public Direction Faces
    {
        get => _faces;
        set
        {
            _faces = value;
            _meshFilter.mesh = null;
        }
    }

    public static Block New()
    {
        var block = Instantiate<Block>(null);
        return block;
    }

    private void Awake()
    {
        _meshFilter = GetComponent<MeshFilter>();
        _meshRenderer = GetComponent<MeshRenderer>();
    }
}