using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    public List<Item> items;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void AddItem(Item newItem)
    {
        Item existingItem = items.Find(item => item.itemName == newItem.itemName);
   
        if(existingItem != null)
        {
            existingItem.quantity += 1;
            Destroy(newItem.gameObject);
        } else
        {
            items.Add(newItem);
        }

    }
}
