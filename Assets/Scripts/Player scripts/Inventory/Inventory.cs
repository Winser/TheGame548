using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{

    List<Item> item;
    public GameObject cellContainer;
    public GameObject InventoryAll;
    public KeyCode ShowInventory;
    public KeyCode takeButton;
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
        ToggleInventory();
        if (Input.GetKeyDown(takeButton))
        {
            //Изменить луч
            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));

            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1));
            //

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 20f))
            {
                if (hit.collider.GetComponent<Item>())
                {
                    for (int i = 0; i < item.Count; i++)
                    {
                        if (item[i].id == 0)
                        {
                            item[i] = hit.collider.GetComponent<Item>();
                            item[i].countItem = 1;
                            DisplayItems();
                            Destroy(hit.collider.GetComponent<Item>().gameObject);
                            break;
                        }
                    }
                }
            }
        }
    }
    //void AddStackableItem(Item currentItem)
    //{
    //    for (int i = 0; i < item.Count; i++)
    //    {
        //        if (item[i].id == currentItem.id)
        //    {
        //        item[i].countItem++;
        //        DisplayItems();
        //        Destroy(currentItem.gameObject);
        //    }
         //}
    //}
    //void AddUnstackableItem()
    //{
    //    for (int i = 0; i < item.Count; i++)
    //    {
    //        if (item[i].id == 0)
    //        {
    //            item[i] = hit.collider.GetComponent<Item>();
    //            item[i].countItem = 1;
    //            DisplayItems();
    //            Destroy(hit.collider.GetComponent<Item>().gameObject);
    //            break;
    //        }
    //    }
    //}
    void ToggleInventory()
    {
        if (Input.GetKeyDown(ShowInventory))
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
    void DisplayItems()
    {
        for (int i = 0; i < item.Count; i++)
        {
            Transform cell = cellContainer.transform.GetChild(i);
            Transform icon = cell.GetChild(0);
            Transform count = icon.GetChild(0);


            Text txt = count.GetComponent<Text>();
            Image img = icon.GetComponent<Image>();
            if (item[i].id != 0)
            {
                img.enabled = true;
                img.sprite = Resources.Load<Sprite>(item[i].pathIcon);
                txt.text = item[i].countItem.ToString();
            }
            else
            {
                img.enabled = false;
                img.sprite = null;
            }
        }
    }
}
