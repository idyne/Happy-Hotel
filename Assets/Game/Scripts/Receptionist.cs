using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Receptionist : Employee
{
    public override bool CanInteract()
    {
        return Hotel.Instance.CanGiveCustomerRoom();
    }

    public override void Interact()
    {
        Hotel.Instance.GiveCustomerRoom();
    }

}
