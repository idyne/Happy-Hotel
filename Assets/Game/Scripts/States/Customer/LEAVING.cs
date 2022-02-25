using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace States.CustomerState
{

    public class LEAVING : State
    {
        private Customer customer;
        private bool hasReachedExit = false;
        private Transform destinationTransform;
        public LEAVING(Customer customer) : base()
        {
            this.customer = customer;
        }

        public override bool CanEnter<Customer>(Customer customer)
        {
            return customer.State.GetType() == typeof(IN_CHECKOUT);
        }

        public override void OnEnter()
        {
            destinationTransform = Hotel.Instance.CustomerLeavePointTransform;
            customer.Agent.ResetPath();
            customer.Agent.SetDestination(destinationTransform.position);
            customer.Animator.ResetTrigger("IDLE");
            customer.Animator.ResetTrigger("WALK");
            customer.Animator.SetTrigger("WALK");
        }

        public override void OnExit()
        {
            hasReachedExit = false;
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
            float dist = customer.Agent.remainingDistance;
            if (!hasReachedExit && dist != Mathf.Infinity && Vector3.Distance(customer.Transform.position, destinationTransform.position) < 0.2f && customer.Agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathComplete && customer.Agent.remainingDistance == 0)
                ReachToExit();
        }

        private void ReachToExit()
        {
            customer.Animator.ResetTrigger("IDLE");
            customer.Animator.ResetTrigger("WALK");
            customer.Animator.SetTrigger("IDLE");
            hasReachedExit = true;
            customer.ChangeState(customer.INACTIVE);
        }
    }
}
