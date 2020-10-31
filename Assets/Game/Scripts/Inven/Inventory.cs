using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    private List<Item> itemList= new List<Item>();
    
    enum ItemType {UP,DOWN,SIDE};

    public struct Item
    {
        Sprite itemImg;
        Sprite bgImg;
        int id;
        ItemType itemType;
    }

    public void AddItem(Item item)
    {
        itemList.Add(item);
    }

    public void Clear()
    {
        itemList.Clear();
    }
   
}
