using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace States.MaidState
{
    public class GOING_TO_ROOM : State
    {
        private MaidAI maid;
        private Room room;
        private Transform destinationTransform;
        private bool hasReachedRoom = false;

        public GOING_TO_ROOM(MaidAI maid) : base()
        {
            this.maid = maid;
        }

        public override bool CanEnter<Customer>(Customer customer)
        {
            return customer.State.GetType() == typeof(WAITING_SIGNAL) ||
                customer.State.GetType() == typeof(TAKING_CLEAN_SHEETS);
        }

        public override void OnEnter()
        {
            destinationTransform = room.DirtyRoomInteractionZone.Transform;
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
            hasReachedRoom = false;
            destinationTransform = null;
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
            if (!hasReachedRoom && Vector3.Distance(maid.Transform.position, destinationTransform.position) < 0.2f && dist != Mathf.Infinity && maid.Agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathComplete && maid.Agent.remainingDistance == 0)
                ReachRoom();
        }

        private void ReachRoom()
        {
            hasReachedRoom = true;
            maid.Transform.LeanRotateY(destinationTransform.rotation.eulerAngles.y, 0.2f);
            maid.CLEANING_ROOM.SetRoom(room);
            maid.ChangeState(maid.CLEANING_ROOM);

        }

        public void SetRoom(Room room)
        {
            this.room = room;
        }
    }
}
