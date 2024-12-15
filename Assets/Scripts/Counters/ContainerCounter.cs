using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter {

    public event EventHandler OnPlayerGrabbedObject;

    [SerializeField] private KitchenObjectSO kitchenObjectSO;


    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            // Player is not carring something
            KitchenObject.SpawnKitchenObject(kitchenObjectSO,player);
        }
        else
        {
            // if player is varring something do nothing
        }
        OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
    }


}
