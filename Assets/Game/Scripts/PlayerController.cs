using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Vector3 AngleToDirection(Vector3 direction, float angle)
    {
        // Vector3 direction = /* 기준이 되는 direction */;
        // 정면을 기준으로 한다면 transform.forward; 를 입렵하면 된다.
        //angle = Mathf.Deg2Rad * angle;
        //Debug.Log(angle);
        var quaternion = Quaternion.Euler(0, angle, 0);
        Vector3 newDirection = quaternion * direction;

        return newDirection;
    }

}
