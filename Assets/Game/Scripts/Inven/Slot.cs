using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour
{
    public GameObject SlotPanel;

    bool panelState = false;
    private int index;


    public void SetIndex(int index){
        this.index = index;
    }
    public void OnClick()
    { 
        Debug.Log("clicked");
        panelState = !panelState;
        SlotPanel.SetActive(panelState);
    }

}
