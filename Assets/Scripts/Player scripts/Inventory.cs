using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    
    List<Item> item;
    public GameObject cellContainer;
    public GameObject InventoryAll;
    public KeyCode ShowInventory;
    void Start()
    {
        item = new List<Item>();

        InventoryAll.SetActive(false);

        for (int i = 0; i < cellContainer.transform.childCount; i++)
        {
            item.Add(new Item());

        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(ShowInventory))
        {
            if (InventoryAll.activeSelf)
            {
                InventoryAll.SetActive(false);
            }
            else
            {
                InventoryAll.SetActive(true);
            }
        }
    }
}
