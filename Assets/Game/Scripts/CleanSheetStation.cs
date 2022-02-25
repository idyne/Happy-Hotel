using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanSheetStation : MonoBehaviour
{
    private static CleanSheetStation instance;
    [SerializeField] private InteractionZone interactionZone;
    private Transform _transform;
    [SerializeField] private Transform cleanSheetSpawnPointTransform;

    public static CleanSheetStation Instance { get => instance; }
    public Transform Transform { get => _transform; }
    public InteractionZone InteractionZone { get => interactionZone; }
    public Transform CleanSheetSpawnPointTransform { get => cleanSheetSpawnPointTransform; }

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else
        {
            DestroyImmediate(gameObject);
            return;
        }
        _transform = transform;
    }
}
