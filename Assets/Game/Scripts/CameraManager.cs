using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private static CameraManager instance;
    public static CameraManager Instance {
        get { return instance; }
    }

    private void Awake()
    {
        instance = this;
        MainCameraPos = Camera.main.transform;
    }

    private void OnDestroy()
    {
        instance = null;
    }

    public Transform MyPos;
    Transform MainCameraPos;

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

        // transform.position = vec;
        MainCameraPos.position = vec;
    }

    //public void ShakeCamera() {
    //   StartCoroutine(CO_ShackCamera());
    //}


    IEnumerator CO_ShackCamera()
    {
        float timer = 1f;
        Vector3 vec = new Vector3(MyPos.position.x + Init_CameraPos.x,
                                  Init_CameraPos.y,
                                  MyPos.position.z + Init_CameraPos.z);
        while (timer > 0f)
        {
            timer -= 0.05f;
            // transform.localPosition = vec + new Vector3(Random.Range(0, 1f), 0, Random.Range(0, 1f)) * timer * 2f;
            MainCameraPos.localPosition = vec + new Vector3(Random.Range(0, 1f), 0, Random.Range(0, 1f)) * timer * 2f;
            yield return null;
        }
    }
}
