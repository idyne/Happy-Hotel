using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Maid))]
public class Cleaner : Employee
{
    private Maid maid;

    private void Awake()
    {
        maid = GetComponent<Maid>();
    }
    public override void Interact()
    {
        DirtyRoomInteractionZone dirtyRoomInteractionZone = currentInteractionZone as DirtyRoomInteractionZone;
        Room room = dirtyRoomInteractionZone.Room;
        CleanSheet cleanSheet = maid.PopCleanSheet();
        room.Clean(cleanSheet);
        //Debug.Log("Cleaned " + room + ".", this);
    }

    public override bool CanInteract()
    {
        if (maid.IsLocked) return false;
        DirtyRoomInteractionZone dirtyRoomInteractionZone = currentInteractionZone as DirtyRoomInteractionZone;
        Room room = dirtyRoomInteractionZone.Room;
        if (room.IsClean) return false;
        return maid.HasCleanSheets;
    }
}
