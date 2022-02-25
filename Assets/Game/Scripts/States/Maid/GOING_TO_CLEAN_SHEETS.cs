using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace States.MaidState
{
    public class GOING_TO_CLEAN_SHEETS : State
    {
        private MaidAI maid;
        private Room room;
        private Transform destinationTransform ;
        private bool hasReachedStation = false;


        public GOING_TO_CLEAN_SHEETS(MaidAI maid) : base()
        {
            this.maid = maid;
        }

        public override bool CanEnter<Customer>(Customer customer)
        {
            return customer.State.GetType() == typeof(WAITING_SIGNAL);
        }

        public override void OnEnter()
        {
            destinationTransform = Hotel.Instance.CleanSheetStation.InteractionZone.Transform;
            maid.destination = destinationTransform;
            maid.Agent.ResetPath();
            maid.Agent.SetDestination(destinationTransform.position);
            maid.Animator.ResetTrigger("IDLE");
            maid.Animator.ResetTrigger("WALK");
            maid.Animator.SetTrigger("WALK");
        }

        public override void OnExit()
        {
            maid.Animator.ResetTrigger("IDLE");
            maid.Animator.ResetTrigger("WALK");
            maid.Animator.SetTrigger("IDLE");
            room = null;
            hasReachedStation = false;
        }

        public override void OnTriggerEnter(Collider other)
        {
        }

        public override void OnTriggerExit(Collider other)
        {
        }

        public override void OnUpdate()
        {
            float dist = maid.Agent.remainingDistance;
            if (!hasReachedStation && Vector3.Distance(maid.Transform.position, destinationTransform.position) < 0.2f && dist != Mathf.Infinity && maid.Agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathComplete && maid.Agent.remainingDistance == 0)
                ReachStation();
        }

        private void ReachStation()
        {
            hasReachedStation = true;
            maid.Transform.LeanRotateY(destinationTransform.rotation.eulerAngles.y, 0.2f);
            maid.TAKING_CLEAN_SHEETS.SetRoom(room);
            maid.ChangeState(maid.TAKING_CLEAN_SHEETS);

        }

        public void SetRoom(Room room)
        {
            this.room = room;
        }

    }
}
