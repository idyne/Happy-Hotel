using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace States.MaidState
{
    public class TAKING_CLEAN_SHEETS : State
    {
        private MaidAI maid;
        private Room room;

        public TAKING_CLEAN_SHEETS(MaidAI maid) : base()
        {
            this.maid = maid;
        }

        public Room Room { get => room; }

        public override bool CanEnter<Customer>(Customer customer)
        {
            return customer.State.GetType() == typeof(GOING_TO_CLEAN_SHEETS);
        }

        public override void OnEnter()
        {
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

    }
}
