using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BumperCar
{
    public enum ItemType
    {
        Item_1,
        Item_2,
        Item_3
    }

    public class ItemInfo : MonoBehaviour
    {
        public ItemType ItemType;

        void Start()
        {
            // ItemType = ItemType.Item_1;
        }


    }
}