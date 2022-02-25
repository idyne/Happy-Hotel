using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace States.CustomerState
{
    public class GOING_TO_ROOM : State
    {
        private Customer customer;
        private Room room = null;
        private Transform destinationTransform;
        private bool hasReachedRoom = false;

        public GOING_TO_ROOM(Customer customer) : base()
        {
            this.customer = customer;
        }

        public override bool CanEnter<Customer>(Customer customer)
        {
            return customer.State.GetType() == typeof(IN_RECEPTION);
        }

        public override void OnEnter()
        {
            if (!room) Debug.LogError("Customer state GOING_TO_ROOM has no room assigned!", customer);
            destinationTransform = room.CustomerStayTransform;
            customer.Agent.ResetPath();
            customer.Agent.SetDestination(destinationTransform.position);
            customer.Animator.ResetTrigger("IDLE");
            customer.Animator.ResetTrigger("WALK");
            customer.Animator.SetTrigger("WALK");
        }

        public override void OnExit()
        {
            room = null;
            hasReachedRoom = false;
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
            if (!hasReachedRoom && dist != Mathf.Infinity && Vector3.Distance(customer.Transform.position, destinationTransform.position) < 0.2f && customer.Agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathComplete && customer.Agent.remainingDistance == 0)
                ReachToRoom();
        }

        private void ReachToRoom()
        {
            hasReachedRoom = true;
            customer.Transform.LeanRotateY(destinationTransform.rotation.eulerAngles.y, 0.2f).setOnComplete(() =>
            {
                customer.Animator.ResetTrigger("IDLE");
                customer.Animator.ResetTrigger("WALK");
                //customer.Animator.SetTrigger("IDLE");
                customer.IN_ROOM.SetRoom(room);
                customer.ChangeState(customer.IN_ROOM);
            });

        }

        public void SetRoom(Room room)
        {
            this.room = room;
        }
    }

}
