using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace States.MaidState
{
    public class WAITING_SIGNAL : State
    {
        private MaidAI maid;

        public WAITING_SIGNAL(MaidAI maid) : base()
        {
            this.maid = maid;
        }

        public override bool CanEnter<Customer>(Customer customer)
        {
            return customer.State.GetType() == typeof(GOING_TO_BASE);
        }

        public override void OnEnter()
        {
            maid.IsAssignmentCancelled = false;
            CleanRoom(Hotel.Instance.GetRoomToBeCleaned());
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

        public void CleanRoom(Room room)
        {
            if (!room) return;
            if (room.Maid != null) Debug.LogError("This room already has an assigned maid!", room);
            room.AssignMaid(maid);
            if (maid.Maid.HasCleanSheets)
            {
                maid.GOING_TO_ROOM.SetRoom(room);
                maid.ChangeState(maid.GOING_TO_ROOM);
            }
            else
            {
                maid.GOING_TO_CLEAN_SHEETS.SetRoom(room);
                maid.ChangeState(maid.GOING_TO_CLEAN_SHEETS);
            }
        }

    }
}
