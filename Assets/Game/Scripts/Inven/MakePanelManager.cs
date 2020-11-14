using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct MakePanelInfo
{
    public string title;
    public int level;
    public string description;
    public int[] count;
}

public class Part
{
    public Sprite bgImg;
    public Sprite itemImg;
    public int count;
}

[System.Serializable]
public class MakePanelUI
{
    public Text title;
    public Text level;
    public Text description;
    public Text count1;
    public Text count2;
    public Text count3;
}

public class MakePanelManager : MonoBehaviour
{
    public GameObject MakePanel;
    public MakePanelUI makePanelUI;
    private MakePanelInfo makePanelInfo;

    public void OpenPanel(MakePanelInfo panelInfo)
    {
        Debug.Log(panelInfo);
        makePanelInfo = panelInfo;
        PrintUI(makePanelInfo);
        MakePanel.SetActive(true);
    }
    public void PrintUI(MakePanelInfo panelInfo) {
        makePanelUI.title.text = panelInfo.title;
        makePanelUI.level.text = "LV"+ panelInfo.level.ToString();
        makePanelUI.description.text =  panelInfo.description.ToString();
        makePanelUI.count1.text = panelInfo.count[0].ToString();
        makePanelUI.count2.text = panelInfo.count[1].ToString();
        makePanelUI.count3.text = panelInfo.count[2].ToString();
    }
    public void Start()
    {
    }
}
