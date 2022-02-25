using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtyRoomInteractionZone : InteractionZone
{
    [SerializeField] private Room room;

    public Room Room { get => room; }
}
