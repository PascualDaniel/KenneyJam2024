using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : PlacedObject
{
    
    [SerializeField] private int itemCount;

    public int GetItemCount()
    {
        return itemCount;
    }

    public void SetItemCount(int itemCount)
    {
        this.itemCount = itemCount;
    }
   
}
