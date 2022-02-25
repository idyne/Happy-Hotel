using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace States.CustomerState
{

    public class INACTIVE : State
    {
        private Customer customer;
        public INACTIVE(Customer customer) : base()
        {
            this.customer = customer;
        }

        public override bool CanEnter<Customer>(Customer customer)
        {
            return customer.State.GetType() == typeof(LEAVING);
        }

        public override void OnEnter()
        {
            customer.gameObject.SetActive(false);
        }

        public override void OnExit()
        {

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
    }
}
