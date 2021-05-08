using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ItemType { TOP, BOTTOM, SIDE };

public struct receipe
{

}

public struct Item
{
    public int Index;
    public ItemType itemType;
    public Sprite image;
}

public class InventoryManager : MonoBehaviour
{
    public GameObject ScrollViewContent;
    public GameObject Slot;
    public GameObject MakePanel;
    public MakePanelManager makePanelManager;
    public EquipManager equipManager;

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
            slotObj.transform.SetParent(ScrollViewContent.transform);
            Slot slot = slotObj.GetComponent<Slot>();
            slot.EnrollInventory(this);
            slot.SetIndex(i);
            slotList.Add(slot);
        }
    }

    public void OpenSlotPanel(int index) //Slot에서 index를 가지고 호출함
    {
        for(int i=0; i < slotList.Count; i++)
        {
            Debug.Log(i == index);
            slotList[i].OpenPanel(i == index);
        }
    }

    public void OpenMakePanel(int index)
    {
        MakePanelInfo makePanelInfo = new MakePanelInfo() { //개체 이니셜라이저
            title = "가시범퍼",
            level = 3,
            description = "공격력 3%향상",
            count = new int[] { 1, 2, 3 }
        };
        makePanelManager.OpenPanel(makePanelInfo);
    }

    public void Equip(int index)
    {
        Image image = slotList[index].gameObject.transform.Find("bgImg/Canvas/ItemImg").GetComponent<Image>();
        Item item = new Item() {
            Index = index,
            itemType = ItemType.TOP,
            image = image.sprite,
        };
        
        //equipManager.ChangeItem(itemList[index]);
        equipManager.ChangeItem(item);
    }

}
