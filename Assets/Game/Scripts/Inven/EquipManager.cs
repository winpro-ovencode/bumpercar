using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EquipManager : MonoBehaviour
{
    [System.Serializable]
    public class MakePanelUI
    {
        public Sprite left;
        public Sprite right;
        public Sprite top;
        public Sprite bottom;
    }

    public struct EquipmentPanelInfo
    {
        Image itemImg;
    }

    public void changeItem(Item item)
    {

        if(item.itemType == ItemType.DOWN) { }
        if (item.itemType == ItemType.UP) { }
        if (item.itemType == ItemType.SIDE) { }

    }
}
