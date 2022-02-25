using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace States.CustomerState
{

    public class IN_CHECKOUT : State
    {
        private Customer customer;
        private Transform destinationTransform;
        private bool hasReachedPlaceInLine = false;

        public bool HasReachedPlaceInLine { get => hasReachedPlaceInLine; }

        public IN_CHECKOUT(Customer customer) : base()
        {
            this.customer = customer;
        }

        public override bool CanEnter<Customer>(Customer customer)
        {
            return customer.State.GetType() == typeof(IN_ROOM);
        }

        public override void OnEnter()
        {
        }

        public override void OnExit()
        {
            destinationTransform = null;
            hasReachedPlaceInLine = false;
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
            if (!hasReachedPlaceInLine && Vector3.Distance(customer.Transform.position, destinationTransform.position) < 0.2f && dist != Mathf.Infinity && customer.Agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathComplete && customer.Agent.remainingDistance == 0)
                ReachToPlaceInLine();
        }

        private void ReachToPlaceInLine()
        {
            hasReachedPlaceInLine = true;
            customer.Animator.ResetTrigger("IDLE");
            customer.Animator.ResetTrigger("WALK");
            customer.Animator.SetTrigger("IDLE");
            customer.Transform.LeanRotateY(destinationTransform.rotation.eulerAngles.y, 0.2f);
        }

        public void SetPlaceInLine(Transform destinationTransform)
        {
            hasReachedPlaceInLine = false;
            this.destinationTransform = destinationTransform;
            customer.Agent.ResetPath();
            customer.Agent.SetDestination(destinationTransform.position);
            customer.Animator.ResetTrigger("IDLE");
            customer.Animator.ResetTrigger("WALK");
            customer.Animator.SetTrigger("WALK");
        }

        public void PayForRoom()
        {
            for (int i = 0; i < 2; i++)
                Hotel.Instance.MoneyPile.AddMoney(customer.Transform.position + Vector3.up * 1 + customer.Transform.forward * 0.4f);
        }
    }
}
