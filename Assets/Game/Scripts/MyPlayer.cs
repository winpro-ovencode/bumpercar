using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MyPlayer : MonoBehaviour
{
    private static MyPlayer instance;
    public static MyPlayer Instance {
        get
        {
            //if (instance == null)
            //{
            //    instance = GameObject.FindObjectOfType<MyPlayer>(); ;
            //}
            //if (instance == null)
            //{
            //    Debug.LogError("Can't find MyPlayer in Game Scene");
            //}
            return instance;
        }
    }
    public Joystick Virtual_Joystick;

    //public PlayerController PC;
    public Transform MyDirectionTr;
    public Transform MyBumpercarTr;
    //public Vector3 Init_MyPos;

    Vector3 _MyDirection;
    public float _speed = 3f;
    public float _rot_speed = 1f;
    private float _accSpeed = 1f;

    public Transform HpTr;
    public float curHp = 1000;
    public float maxHp = 1000;

    public Transform StTr;
    public float curSt = 0;
    public float maxSt = 250;

    float angle = 0f;    
    int sign = 0;

    private void Init()
    {
        instance = this;
    }

    private void Awake()
    {
        Init();
        Virtual_Joystick.accSpeed_initMax = 2;
    }

    private void Start()
    {
        curSt = 0;
        SetHpUI();
        SetStUI();

        StartCoroutine("CoRecoveryEnergy");
    }

    public void DropSt() {
        curSt--;
        if (curSt <= 0)
        {
            _accSpeed = 1f;
        }
        SetStUI();
    }

    public void SetAcc(float _accSpeed) {
        this._accSpeed = _accSpeed;
        Debug.Log(_accSpeed);
    }

    void Update()
    {
        // MyBumpercarTr.localPosition = Vector3.zero;
        _MyDirection = MyDirectionTr.position - transform.position;
        if (_accSpeed > 1)
        {
            DropSt();
        }
        if (Virtual_Joystick.IsDrag)
        {
            angle = Vector3.Angle(_MyDirection, new Vector3(Virtual_Joystick.Dir.x, 0, Virtual_Joystick.Dir.y));
            sign = (Vector3.Cross(_MyDirection, new Vector3(Virtual_Joystick.Dir.x, 0, Virtual_Joystick.Dir.y)).y > 0) ? 1 : -1;
            transform.Rotate(0, sign * angle * _rot_speed * Time.deltaTime, 0);
            //Debug.Log(Vector3.Angle(Vector3.forward, _MyDirection));
            // _accSpeed = Virtual_Joystick.accSpeed;
        }
        transform.position += (_MyDirection) * _speed * Time.deltaTime * _accSpeed;
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

    void OnDestroy()
    {
        instance = null;
    }
}
