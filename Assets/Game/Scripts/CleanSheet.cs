using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames;

public class CleanSheet : MonoBehaviour, IPooledObject
{
    [SerializeField] private Transform meshTransform;
    private Transform _transform;

    public Transform Transform { get => _transform; }
    public Transform MeshTransform { get => meshTransform; }

    public void OnObjectSpawn()
    {
        meshTransform.localScale = Vector3.one;
    }

    private void Awake()
    {
        _transform = transform;
    }
}
