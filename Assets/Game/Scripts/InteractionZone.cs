using UnityEngine;
using UnityEngine.Events;

public class InteractionZone : MonoBehaviour
{
    [SerializeField] private GameObject effect;
    private Transform _transform;

    public Transform Transform { get => _transform; }

    protected virtual void Awake()
    {
        _transform = transform;
    }

    public void DisableEffect()
    {
        effect.SetActive(false);
    }

    public void EnableEffect()
    {
        effect.SetActive(true);
    }
}
