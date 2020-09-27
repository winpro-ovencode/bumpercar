using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MyPlayer : MonoBehaviour
{
    public Joystick Virtual_Joystick;
    //public PlayerController PC;
    public Transform MyDirectionTr;
    public Transform MyBumpercarTr;
    //public Vector3 Init_MyPos;

    Vector3 _MyDirection;
    public float _speed = 3f;
    public float _rot_speed = 1f;

    public Transform HpTr;
    public float curHp = 1000;
    public float maxHp = 1000;

    public Transform StTr;
    public float curSt = 0;
    public float maxSt = 50;

    float angle = 0f;    
    int sign = 0;

    private void Start()
    {
        curSt = 0;
        SetHpUI();
        SetStUI();

        StartCoroutine("CoRecoveryEnergy");
    }


    void Update()
    {
        // MyBumpercarTr.localPosition = Vector3.zero;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AccBumperCar();
        }

        _MyDirection = MyDirectionTr.position - transform.position;

        if (Virtual_Joystick.IsDrag)
        {
            angle = Vector3.Angle(_MyDirection, new Vector3(Virtual_Joystick.Dir.x, 0, Virtual_Joystick.Dir.y));
            sign = (Vector3.Cross(_MyDirection, new Vector3(Virtual_Joystick.Dir.x, 0, Virtual_Joystick.Dir.y)).y > 0) ? 1 : -1;
            transform.Rotate(0, sign * angle * _rot_speed * Time.deltaTime, 0);
            //Debug.Log(Vector3.Angle(Vector3.forward, _MyDirection));
        }
        transform.position += (_MyDirection) * _speed * Time.deltaTime;
    }

    //IEnumerator CoRecoveryEnergy(float _recoveryTime) {
    IEnumerator CoRecoveryEnergy()
    {
        float _recoveryTime = 0.1f;
        float timer = 0;
        while (true)
        {
            yield return new WaitForFixedUpdate();
            timer+= Time.fixedDeltaTime;
            if (timer >= _recoveryTime)
            {
                timer -= _recoveryTime;
                if (curSt < maxSt)
                {
                    curSt++;
                    SetStUI();
                }
            }
        }
    }

    void SetHpUI() {
        HpTr.localScale = new Vector3(curHp /maxHp, 1, 1);
    }

    void SetStUI()
    {
        StTr.localScale = new Vector3(curSt / maxSt, 1, 1);
    }

    public void AccBumperCar() {
        _speed = 6;
        Debug.Log("Ddd");
    }

}
