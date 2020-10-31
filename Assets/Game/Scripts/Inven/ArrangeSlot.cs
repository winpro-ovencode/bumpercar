using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ArrangeSlot : MonoBehaviour
{
    public GameObject Slot;
    public Inventory inventory;
    public int row=5;

    // Start is called before the first frame update
    void Start()
    {
        MakeSlots();
    }

    void Init()
    {

    }

    private void MakeSlots()
    {
        int col = gameObject.GetComponent<GridLayoutGroup>().constraintCount;

        for (int i = 0; i < row * col; i++)
        {
            GameObject slotObj = Instantiate(Slot);
            slotObj.transform.parent = transform;
            Slot slot = slotObj.GetComponent<Slot>();
            slot.SetIndex(i);
            inventory.slotList.Add(slot);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
