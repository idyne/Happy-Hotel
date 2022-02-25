using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SalesWoman : Employee
{
    public override bool CanInteract()
    {
        return Hotel.Instance.CanCustomerCheckout();
    }

    public override void Interact()
    {
        Hotel.Instance.CheckoutCustomer();
    }

}
