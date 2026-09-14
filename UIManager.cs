using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject LossUI;
    public GameObject StartUI;
    
    GameManager gm;

    public TextMeshProUGUI FinalscoreUI;
    public TextMeshProUGUI HighscoreUI;
    public TextMeshProUGUI CoinUI;

    private void Start()
    {
        
        gm = GameManager.Instance;
        gm.OnGameover.AddListener(DeathMenu);

    }
    public void PlayButtonHandler()
    {
        gm.GameStart();
        StartUI.SetActive(false);
    }
    public void StartMenu()
    {
        StartUI.SetActive(true);
       
    }
    public void RestartButtonHandler()
    {
        LossUI.SetActive(false);
        gm.GameStart();
        
    }
    public void DeathMenu()
    {
        LossUI.SetActive(true);
        FinalscoreUI.text = "Score: " + gm.PreatyScore() + " meters";
        HighscoreUI.text = "Highscore: " + gm.PreatyHighScore() + " meters";
        CoinUI.text = "Coins:" + gm.Coins(); 
    }

    public void ExitButtonHandler()
    {
        Application.Quit();
    }
   
}
