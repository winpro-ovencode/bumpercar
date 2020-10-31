using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ItemType { UP, DOWN, SIDE };

public struct Item
{
    public int Index;
    public ItemType itemType;
    public Sprite Image;
}

public class Inventory : MonoBehaviour
{
    public GameObject ScrollViewContent;
    public GameObject Slot;
    public GameObject MakePanel;

    public List<Slot> slotList = new List<Slot>();
    public List<Item> itemList = new List<Item>();
    public int rowNum = 5; // 한 행의 갯수

    public void Start()
    {
        MakeSlots();
    }

    public void MakeSlots()
    {
        int col = ScrollViewContent.GetComponent<GridLayoutGroup>().constraintCount;

        for (int i = 0; i < rowNum * col; i++)
        {
            GameObject slotObj = Instantiate(Slot);
            slotObj.transform.parent = ScrollViewContent.transform;
            Slot slot = slotObj.GetComponent<Slot>();
            slot.EnrollInventory(this);
            slot.SetIndex(i);
            slotList.Add(slot);
        }

    }
    public void OpenSlotPanel(int index)
    {
        for(int i=0; i<slotList.Count;i++)
            slotList[i].OpenPanel(i == index);
        
    }

    public void OpenMakePanel(int index)
    {
        MakePanel.SetActive(true);
    }
}
