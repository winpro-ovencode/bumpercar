using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    enum ItemType {UP,DOWN,SIDE};

    struct Item
    {
        Sprite itemImg;
        Sprite bgImg;
        int id;
    }

    void OnClickItem()
    {

    }
}
