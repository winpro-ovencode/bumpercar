using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class EquipPanelUI
{
    public Image left;
    public Image right;
    public Image top;
    public Image bottom;
}

public class EquipManager : MonoBehaviour
{
    public EquipPanelUI equipPanelUI;


    public void ChangeItem(Item item)
    {
        if (item.itemType == ItemType.BOTTOM)
            equipPanelUI.bottom.sprite = item.image;
        if (item.itemType == ItemType.TOP) 
            equipPanelUI.top.sprite = item.image;
        if (item.itemType == ItemType.SIDE){
            equipPanelUI.left.sprite = item.image;
            equipPanelUI.right.sprite = item.image;
        }

    }
}
