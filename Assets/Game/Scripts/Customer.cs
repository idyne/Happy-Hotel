using System.Collections;
using System.Collections.Generic;
using FateGames;
using UnityEngine;
using UnityEngine.AI;
using States.CustomerState;

[RequireComponent(typeof(NavMeshAgent))]
public class Customer : StateMachine, IPooledObject
{
    #region States
    private INACTIVE inactive;
    private IN_RECEPTION in_reception;
    private GOING_TO_ROOM going_to_room;
    private IN_ROOM in_room;
    private IN_CHECKOUT in_checkout;
    private LEAVING leaving;

    public INACTIVE INACTIVE { get => inactive; }
    public IN_RECEPTION IN_RECEPTION { get => in_reception; }
    public GOING_TO_ROOM GOING_TO_ROOM { get => going_to_room; }
    public IN_ROOM IN_ROOM { get => in_room; }
    public IN_CHECKOUT IN_CHECKOUT { get => in_checkout; }
    public LEAVING LEAVING { get => leaving; }
    #endregion

    [SerializeField] private float stayInRoomDuration = 5;
    [SerializeField] private Animator animator;
    private NavMeshAgent agent;
    private Transform _transform;
    public NavMeshAgent Agent { get => agent; }

    public Transform Transform { get => _transform; }

    public Animator Animator { get => animator; }
    public float StayInRoomDuration { get => stayInRoomDuration; }

    public void OnObjectSpawn()
    {
        InitializeStates();
        state = inactive;
    }

    protected override void InitializeStates()
    {
        inactive = new INACTIVE(this);
        in_reception = new IN_RECEPTION(this);
        going_to_room = new GOING_TO_ROOM(this);
        in_room = new IN_ROOM(this);
        in_checkout = new IN_CHECKOUT(this);
        leaving = new LEAVING(this);
    }

    protected override void Awake()
    {
        base.Awake();
        agent = GetComponent<NavMeshAgent>();
        _transform = transform;
        OnObjectSpawn();
    }

}
