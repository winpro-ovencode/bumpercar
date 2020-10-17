using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ArrangeSlot : MonoBehaviour
{
    public GameObject Slot;
    public int row=5;
    // Start is called before the first frame update
    void Start()
    {
        int col = gameObject.GetComponent<GridLayoutGroup>().constraintCount;

        for (int i = 0; i < row * col; i++)
        {
            GameObject slot = Instantiate(Slot);
            slot.transform.parent = transform;
        }
    }

    void Init()
    {

    }

    private void MakeSlots()
    {

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
