using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour
{
    private Inventory inventory;
    public GameObject SlotPanel;
    public Sprite itemImg;
    public Sprite bgImg;
    bool panelState = false;
    private int index;

    public void EnrollInventory(Inventory inventory)
    {
        this.inventory = inventory;
    }

    public void SetIndex(int index){
        this.index = index;
    }
    public void OnSlotClick()
    { 
        Debug.Log("clicked" + index);
        panelState = !panelState;
        inventory.OpenSlotPanel(index);
    }
    public void OpenPanel(bool state)
    {
        SlotPanel.SetActive(state);
    }

    public void OnMakePanelClick() {
        inventory.OpenMakePanel(index);
    }

}
