using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace States.CustomerState
{

    public class IN_ROOM : State
    {
        private Customer customer;
        private Room room;
        public IN_ROOM(Customer customer) : base()
        {
            this.customer = customer;
        }

        public override bool CanEnter<Customer>(Customer customer)
        {
            return customer.State.GetType() == typeof(GOING_TO_ROOM);
        }

        public override void OnEnter()
        {
            customer.StartCoroutine(StayInRoomCoroutine());
        }

        private IEnumerator StayInRoomCoroutine()
        {
            customer.Animator.SetTrigger("LIE_DOWN");
            yield return new WaitForSeconds(customer.StayInRoomDuration - 2.9f);
            customer.Animator.SetTrigger("GET_UP");
            yield return new WaitForSeconds(2.9f);
            GoToCheckout();
        }

        public override void OnExit()
        {
            room = null;
        }

        public override void OnTriggerEnter(Collider other)
        {
        }

        public override void OnTriggerExit(Collider other)
        {
        }

        public override void OnUpdate()
        {
        }

        public void SetRoom(Room room)
        {
            this.room = room;
        }

        public void GoToCheckout()
        {
            room.Empty();
            customer.ChangeState(customer.IN_CHECKOUT);
            customer.IN_CHECKOUT.SetPlaceInLine(Hotel.Instance.EnqueueToCheckoutLine(customer));
        }
    }
}
