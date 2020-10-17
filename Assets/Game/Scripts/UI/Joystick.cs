using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour
{
    //private static Joystick instance;
    //public static Joystick Instance {
    //    get {
    //        //if (instance == null)
    //        //{
    //        //    instance = GameObject.FindObjectOfType<Joystick>(); ;
    //        //}
    //        //if (instance == null)
    //        //{
    //        //    Debug.LogError("Can't find Joystick in Game Scene");
    //        //}
    //        return instance;
    //    }
    //}

    //private void Awake()
    //{
    //    instance = this;
    //    accSpeed = 1f;
    //    radius_initTemp = radius;
    //}

    // public GameObject myPlayer;
    public GameObject Handle;
    public Vector3 Dir;
    public bool IsDrag;
    public float radius;
    private float radius_initTemp;

    [SerializeField]
    public float accSpeed;
    public float accSpeed_initMax;

    private void Awake()
    {
        radius_initTemp = radius;
        accSpeed = 1f;
    }

    public void MouseDown(BaseEventData _data)
    {
        Vector3 pos = ((PointerEventData)_data).position;
        transform.position = pos;
        Handle.transform.position = pos;
        //Debug.Log(pos);
        gameObject.SetActive(true);
        // setAccel(2f);
    }

    public void MouseDrag(BaseEventData _data)
    {
        IsDrag = true;
        Vector3 inputPos = ((PointerEventData)_data).position;
        Vector3 dir = inputPos - transform.position;
        dir.z = 0;
        float fingerRad = dir.magnitude;

        Dir = dir.normalized;

        if (MyPlayer.Instance.StTr.localScale.x == 1 && fingerRad > radius)
        {
            radius = radius_initTemp * accSpeed_initMax;
        }
        else
        {
            radius = radius_initTemp;
        }

        if (fingerRad < radius)
        {
            accSpeed = 1f;
        }
        else
        {
            accSpeed = 2f;
            MyPlayer.Instance.curSt--;
        }

        Handle.transform.position = transform.position + Dir * Mathf.Min(fingerRad, radius);

    }

    public void MouseUp(BaseEventData _data)
    {
        accSpeed = 1f;
        IsDrag = false;        
        gameObject.SetActive(false);
        //setAccel(1f);
    }

    //private void setAccel(float _accSpeed) {
    //    accSpeed = _accSpeed;
    //    if (accSpeed > 1f)
    //    {
    //        radius = radius_initTemp * 2f;
    //    }
    //    else
    //    {
    //        radius = radius_initTemp;
    //    }
    //}

    //void OnDestroy()
    //{
    //    instance = null;
    //}
}
