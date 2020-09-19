using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UILobby : MonoBehaviour
{
    public Button StartGame;

    // Start is called before the first frame update
    void Start()
    {
        StartGame.onClick.AddListener(() => OnGameStart());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnGameStart()
    {
        SceneManager.LoadScene("Game");
    }
}
