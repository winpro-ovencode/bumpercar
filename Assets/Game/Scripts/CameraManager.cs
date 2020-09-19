using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform MyPos;
    Vector3 Init_CameraPos;

    void Start()
    {
        Init_CameraPos = new Vector3(0, 30.9f, - 16.32f);
    }

    private void Update()
    {
        Vector3 vec = new Vector3(MyPos.position.x + Init_CameraPos.x,
                                   Init_CameraPos.y,
                                   MyPos.position.z + Init_CameraPos.z);

        transform.position = vec;

    }

    public void test() {
        StartCoroutine(CO_ShackCamera());
    }


    IEnumerator CO_ShackCamera()
    {
        float timer = 1f;
        Vector3 vec = new Vector3(MyPos.position.x + Init_CameraPos.x,
                                  Init_CameraPos.y,
                                  MyPos.position.z + Init_CameraPos.z);
        while (timer > 0f)
        {
            timer -= 0.05f;
            transform.localPosition = vec + new Vector3(Random.Range(0, 1f), 0, Random.Range(0, 1f)) * timer * 2f;
            yield return null;
        }
    }
}
