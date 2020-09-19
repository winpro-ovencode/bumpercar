using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlayer : MonoBehaviour
{
    public Joystick Virtual_Joystick;
    //public PlayerController PC;
    public Transform MyDirection;
    //public Vector3 Init_MyPos;

    Vector3 _MyDirection;
    public float _speed = 3f;
    public float _rot_speed = 1f;

    float angle = 0f;    
    int sign = 0;

    void Awake()
    {
    }
    
    void Update()
    {
        _MyDirection = MyDirection.position - transform.position;

        if (Virtual_Joystick.IsDrag)
        {
            angle = Vector3.Angle(_MyDirection, new Vector3(Virtual_Joystick.Dir.x, 0, Virtual_Joystick.Dir.y));
            sign = (Vector3.Cross(_MyDirection, new Vector3(Virtual_Joystick.Dir.x, 0, Virtual_Joystick.Dir.y)).y > 0) ? 1 : -1;
            transform.Rotate(0, sign * angle * _rot_speed * Time.deltaTime, 0);
            //Debug.Log(Vector3.Angle(Vector3.forward, _MyDirection));
        }
        transform.position += (_MyDirection) * _speed * Time.deltaTime;
    }
}
