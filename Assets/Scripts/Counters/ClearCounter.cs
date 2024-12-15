using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{

    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public override void Interact(Player player)
    {

        if (!HasKitchenObject())
        {
            // There is no KitchenObject
            if ( player.HasKitchenObject())
            {
                 player.GetKitchenObject().SetKitchenObjectParent(this);
            }
        }
        else {
            // There is a kitche object
            if (player.HasKitchenObject()) {
                // Player is carrying something
            }
            else
            {
                // Player is not carrying anything
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }

    }

    public void TestFunc()
    {
        Debug.Log("test");
    }

    
}
