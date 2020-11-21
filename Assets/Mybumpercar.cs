using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BumperCar
{
    public class Mybumpercar : MonoBehaviour
    {
        float item_1_f;
        float item_2_f;
        float item_3_f;

        float crashPower;

        public Text item_1_txt;
        public Text item_2_txt;
        public Text item_3_txt;

        public Transform MyPlayerTr;

        private void Start()
        {
            SetItemUI();
            crashPower = 500f;
        }

        private void OnTriggerEnter(Collider col)
        {
            ItemInfo info = col.gameObject.GetComponent<ItemInfo>();
            switch (info.ItemType)
            {
                case ItemType.Item_1:
                    item_1_f++;
                    break;
                case ItemType.Item_2:
                    item_2_f++;
                    break;
                case ItemType.Item_3:
                    item_3_f++;
                    break;
            }

            SetItemUI();
        }


        private void OnCollisionEnter(Collision col)
        {
            //if (col.gameObject.CompareTag(GameObjType.Enemy.ToString()))
            if (col.gameObject.CompareTag("Enemy"))
            {
                //CameraManager.Instance.ShakeCamera();
                Rigidbody rb = col.gameObject.GetComponent<Rigidbody>();
                rb.AddForce(crashPower * (col.transform.position - transform.position).normalized);
            }
            // SetMyPlalyerPos();
        }

        //private void OnCollisionExit(Collision col)
        //{
        //    // SetMyPlalyerPos();
        //}

        //private void SetMyPlalyerPos() {
        //    MyPlayerTr.position = new Vector3(transform.position.x, 0, transform.position.z);
        //    transform.localPosition = Vector3.zero;
        //}

        void SetItemUI()
        {
            item_1_txt.text = item_1_f.ToString();
            item_2_txt.text = item_2_f.ToString();
            item_3_txt.text = item_3_f.ToString();
        }
    }
}