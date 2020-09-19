using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour
{
    public GameObject Handle;
    public Vector3 Dir;
    public bool IsDrag;
    public float radius;

    public void MouseDown(BaseEventData _data)
    {
        Vector3 pos = ((PointerEventData)_data).position;
        transform.position = pos;
        Handle.transform.position = pos;
        Debug.Log(pos);
        gameObject.SetActive(true);
    }

    public void MouseDrag(BaseEventData _data)
    {
        IsDrag = true;
        Vector3 inputPos = ((PointerEventData)_data).position;
        Vector3 dir = inputPos - transform.position;
        dir.z = 0;
        Dir = dir.normalized;        
        Handle.transform.position = transform.position + Dir * Mathf.Min(dir.magnitude,radius);

    }

    public void MouseUp(BaseEventData _data)
    {
        IsDrag = false;
        gameObject.SetActive(false);
    }
}
