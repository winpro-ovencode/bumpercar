using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct MakePanelInfo
{
    public string name;
    public int level;
    public string description;
    public Part[] parts;
}

public struct Part
{
    Sprite bgImg;
    Sprite itemImg;
    int count;
}

public class MakePanelManager : MonoBehaviour
{
    private MakePanelInfo makePanelInfo;
    public GameObject MakePanel;

    void OpenPanel(MakePanelInfo panelInfo)
    {
        makePanelInfo = panelInfo;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
