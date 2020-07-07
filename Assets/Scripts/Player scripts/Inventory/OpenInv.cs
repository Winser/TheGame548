using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenInv : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject MyInventoryAll;
    public void OpenMyInv()
    {
        MyInventoryAll.SetActive(true);
    }
    public void CloseMyInv()
    {
        MyInventoryAll.SetActive(false);
    }
}
