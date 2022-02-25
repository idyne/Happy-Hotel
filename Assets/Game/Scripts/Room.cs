using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private GameObject cleanBedObject, dirtyBedObject;
    [SerializeField] private ParticleSystem cleanEffect;
    [SerializeField] private DirtyRoomInteractionZone dirtyRoomInteractionZone;
    [SerializeField] private Transform customerStayTransform;
    [SerializeField] private Door door;
    private Customer customer;
    private Transform _transform;
    private bool isClean = true;
    private MaidAI maid;

    public Customer Customer { get => customer; }

    public bool IsEmpty { get => customer == null; }
    public bool IsAvaliable { get => customer == null && isClean; }
    public Transform Transform { get => _transform; }
    public bool IsClean { get => isClean; }
    public DirtyRoomInteractionZone DirtyRoomInteractionZone { get => dirtyRoomInteractionZone; }
    public Transform CustomerStayTransform { get => customerStayTransform; }
    public MaidAI Maid { get => maid; }
    public Door Door { get => door;  }

    private void Awake()
    {
        _transform = transform;
    }

    public void SetCustomer(Customer customer)
    {
        if (this.customer != null) Debug.LogError("Someone is already staying in this room!", this);
        this.customer = customer;
    }

    public void Empty()
    {
        customer = null;
        isClean = false;
        dirtyBedObject.SetActive(true);
        cleanBedObject.SetActive(false);
        dirtyRoomInteractionZone.gameObject.SetActive(true);
        Hotel.Instance.AssignMaidToRoom(this);
    }

    public void Clean(CleanSheet cleanSheet)
    {
        cleanSheet.Transform.LeanMove(_transform.position, 0.19f);
        cleanSheet.MeshTransform.LeanScale(Vector3.zero, 0.2f).setOnComplete(() =>
        {
            cleanSheet.gameObject.SetActive(false);
        });
        isClean = true;
        dirtyRoomInteractionZone.gameObject.SetActive(false);
        Hotel.Instance.RemoveRoomToBeCleaned(this);
        if (maid)
            maid.CancelRoomAssignment();
        maid = null;
        dirtyBedObject.SetActive(false);
        cleanBedObject.SetActive(true);
        cleanEffect.Play();
    }

    public void AssignMaid(MaidAI maid)
    {
        this.maid = maid;
    }
}
