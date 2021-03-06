﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToolTipPanel : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
        {
            gameObject.SetActive(ClickingSelfOrChild());
        }
    }
    private bool ClickingSelfOrChild()
    {
        RectTransform[] rectTransforms = GetComponentsInChildren<RectTransform>();
        foreach (RectTransform rectTransform in rectTransforms)
        {
            if (EventSystem.current.currentSelectedGameObject == rectTransform.gameObject)
            {
                return true;
            };
        }
        return false;
    }
}
