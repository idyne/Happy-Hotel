using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using States.MaidState;

[RequireComponent(typeof(Maid))]
[RequireComponent(typeof(NavMeshAgent))]
public class MaidAI : StateMachine
{
    private Maid maid;
    private NavMeshAgent agent;
    public Transform basePointTransform;
    [SerializeField] private Animator animator;
    public bool IsAssignmentCancelled = false;
    private Transform _transform;
    public Transform destination;
    public Maid Maid { get => maid; }
    public NavMeshAgent Agent { get => agent; }
    public Transform Transform { get => _transform; }
    public Animator Animator { get => animator; }

    #region States
    private WAITING_SIGNAL waiting_signal;
    private GOING_TO_CLEAN_SHEETS going_to_clean_sheets;
    private TAKING_CLEAN_SHEETS taking_clean_sheets;
    private GOING_TO_ROOM going_to_room;
    private CLEANING_ROOM cleaning_room;
    private GOING_TO_BASE going_to_base;

    public WAITING_SIGNAL WAITING_SIGNAL { get => waiting_signal; }
    public GOING_TO_CLEAN_SHEETS GOING_TO_CLEAN_SHEETS { get => going_to_clean_sheets; }
    public TAKING_CLEAN_SHEETS TAKING_CLEAN_SHEETS { get => taking_clean_sheets; }
    public GOING_TO_ROOM GOING_TO_ROOM { get => going_to_room; }
    public CLEANING_ROOM CLEANING_ROOM { get => cleaning_room; }
    public GOING_TO_BASE GOING_TO_BASE { get => going_to_base; }



    #endregion

    protected override void Awake()
    {
        base.Awake();
        maid = GetComponent<Maid>();
        agent = GetComponent<NavMeshAgent>();
        _transform = transform;
        state = WAITING_SIGNAL;
    }

    private void Start()
    {
        maid.SheetTaker.SetCallback(() =>
        {
            if (!maid.CanTakeCleanSheet)
            {
                if (!IsAssignmentCancelled)
                {
                    GOING_TO_ROOM.SetRoom(TAKING_CLEAN_SHEETS.Room);
                    ChangeState(GOING_TO_ROOM);
                }
                else ChangeState(GOING_TO_BASE);
            }
        });
    }

    public override void ChangeState(State newState)
    {
        base.ChangeState(newState);
        animator.SetBool("CARRYING", maid.HasCleanSheets);
    }

    protected override void InitializeStates()
    {
        waiting_signal = new WAITING_SIGNAL(this);
        going_to_clean_sheets = new GOING_TO_CLEAN_SHEETS(this);
        taking_clean_sheets = new TAKING_CLEAN_SHEETS(this);
        going_to_room = new GOING_TO_ROOM(this);
        cleaning_room = new CLEANING_ROOM(this);
        going_to_base = new GOING_TO_BASE(this);
    }

    public void CancelRoomAssignment()
    {
        if (IsAssignmentCancelled) return;
        if (state == WAITING_SIGNAL) Debug.LogError("Already waiting for signal!", this);
        else if (state == GOING_TO_BASE) Debug.LogError("Already going to base!", this);
        else
        {
            IsAssignmentCancelled = true;
            if (state == GOING_TO_ROOM) ChangeState(GOING_TO_BASE);
            else if (state == CLEANING_ROOM)
            {
                maid.Cleaner.CancelCasting();
                ChangeState(GOING_TO_BASE);
            }
        }
    }
}
