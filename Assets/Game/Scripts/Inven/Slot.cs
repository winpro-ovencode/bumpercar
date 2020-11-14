using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour
{
    private InventoryManager inventoryManager;
    public GameObject SlotPanel;
    public Sprite itemImg;
    public Sprite bgImg;
    bool panelState = false;
    private int index;

    public void EnrollInventory(InventoryManager inventoryManager)
    {
        this.inventoryManager = inventoryManager;
    }

    public void SetIndex(int index) {
        this.index = index;
    }
    public void OnSlotClick()
    {
        Debug.Log("clicked" + index);
        panelState = !panelState;
        inventoryManager.OpenSlotPanel(index);
    }
    public void OpenPanel(bool state)  //Make, Equip 버튼 패널 열기, Slot 프리팹 하위에 있음
    {
        SlotPanel.SetActive(state);
    }
    public void OnMakePanelClick() { // Make 패널 열기
        inventoryManager.OpenMakePanel(index);
    }

    public void Equip()
    {
        Debug.Log("장비 착용");
    }

}
