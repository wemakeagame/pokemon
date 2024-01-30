using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ITEM_TYPE
{
    GENERAL,
    COMSUMABLE,
    POKEBALL,
}

public class Item : Interactible
{

    public string itemName;
    public int quantity = 1;
    public ITEM_TYPE itemType;
    public Sprite icon;


    private Inventory inventory;
    private UIDialog dialog;

    private void Start()
    {
        inventory = FindObjectOfType<Inventory>();
        dialog = FindObjectOfType<UIDialog>();
    }


    protected override void OnInteract()
    {
        inventory.AddItem(this);
        gameObject.SetActive(false);
        dialog.Report("You found " + itemName, false);
    }

    protected override void OnStopInteract()
    {

    }
    
}
