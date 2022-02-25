using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace States.MaidState
{
    public class GOING_TO_BASE : State
    {
        private MaidAI maid;
        private Transform destinationTransform;
        private bool hasReachedBase = false;

        public GOING_TO_BASE(MaidAI maid) : base()
        {
            this.maid = maid;
        }

        public override bool CanEnter<Customer>(Customer customer)
        {
            return customer.State.GetType() == typeof(CLEANING_ROOM) ||
                customer.State.GetType() == typeof(GOING_TO_ROOM) ||
                customer.State.GetType() == typeof(TAKING_CLEAN_SHEETS);
        }

        public override void OnEnter()
        {
            destinationTransform = maid.basePointTransform;
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
            hasReachedBase = false;
        }

        public override void OnTriggerEnter(Collider other)
        {
        }

        public override void OnTriggerExit(Collider other)
        {

        }

        public override void OnUpdate()
        {
            if (Hotel.Instance.ThereAreRoomsToBeCleaned)
            {

                maid.ChangeState(maid.WAITING_SIGNAL);
                return;
            }
            float dist = maid.Agent.remainingDistance;
            if (!hasReachedBase && dist != Mathf.Infinity && Vector3.Distance(maid.Transform.position, destinationTransform.position) < 0.2f && maid.Agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathComplete && maid.Agent.remainingDistance == 0)
                ReachBase();
        }

        private void ReachBase()
        {
            hasReachedBase = true;
            maid.Transform.LeanRotateY(destinationTransform.rotation.eulerAngles.y, 0.2f);
            maid.ChangeState(maid.WAITING_SIGNAL);
        }
    }
}
