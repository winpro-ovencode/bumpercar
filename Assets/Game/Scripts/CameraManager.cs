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
        //transform.position = new Vector3(cameraPos.position.x,
        //                                30.9f,
        //                                cameraPos.position.z - 13.8f);

        Vector3 vec = new Vector3(MyPos.position.x + Init_CameraPos.x,
                                   Init_CameraPos.y,
                                   MyPos.position.z + Init_CameraPos.z);
        transform.position = Vector3.Lerp(transform.position, vec, Time.deltaTime * 1f);


        //transform.Translate(new Vector3(cameraPos.position.x,
        //                                0,
        //                                cameraPos.position.z) * Time.deltaTime, Space.World);
        //transform.position += new Vector3(cameraPos.position.x,
        //                                  0,
        //                                  cameraPos.position.z) * Time.deltaTime;

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

        //Vector3 vec = new Vector3(0, 30.9f, -16.32f);

        //Vector3 vec = new Vector3(MyPos.position.x, 30.9f, MyPos.position.z - 16.32f);

        //while (true)
        //{
        //    transform.localPosition = vec + new Vector3(Random.Range(0, 11f), 0, Random.Range(0, 11f)) * 10;
        //    //transform.position = MyPos.position;
        //    //Debug.Log(new Vector3(Random.Range(0, 11f), 0, Random.Range(0, 11f)) * 10);
        //    //Debug.Log(transform.position);
        //    //timer += Time.deltaTime;
        //    //Debug.Log(timer);
        //    if (timer >= 60f)
        //    {
        //        Debug.Log("ddd");
        //        transform.position = vec;
        //        yield break;
        //    }
        //}

        while (timer > 0f)
        {
            timer -= 0.05f;
            transform.localPosition = vec + new Vector3(Random.Range(0, 1f), 0, Random.Range(0, 1f)) * timer * 2f;
            yield return null;
        }
    }
}
