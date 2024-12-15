using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCounter : BaseCounter
{
    public override void Interact(Player player)
    {
        if(player.HasKitchenObject())
            {
            // Player carrying something
            player.GetKitchenObject().DestroySelf();
            }
        else
        {
            // Player carrying is not something
        }
    }


}
