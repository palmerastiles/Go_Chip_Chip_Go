using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class UIManager : MonoBehaviour
{

    public GameObject LossUI;
    public GameObject StartUI;

    public GameObject OptionsUI;

    public GameObject PauseUI;

    public AudioMixer audioMixer;
    CameraController Cam;
    PlayerEndless player;
    GameManager gm;

    public TextMeshProUGUI FinalscoreUI;
    public TextMeshProUGUI HighscoreUI;
    public TextMeshProUGUI CoinUI;

    private void Start()
    {   
        Cam = FindAnyObjectByType<CameraController>();
        player = FindAnyObjectByType<PlayerEndless>();
        gm = GameManager.Instance;
        gm.OnGameover.AddListener(DeathMenu);

    }
    public void PlayButtonHandler()
    {
        player.transform.position = player.InitialPosition;
        Cam.transform.position = Cam.InitialPosition;
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
        player.transform.position = player.InitialPosition;
        Cam.transform.position = Cam.InitialPosition;
    }
    public void DeathMenu()
    {
        LossUI.SetActive(true);
        FinalscoreUI.text = "Score: " + gm.PreatyScore() + " meters";
        HighscoreUI.text = "Highscore: " + gm.PreatyHighScore() + " meters";
        CoinUI.text = "Coins:" + gm.Coins(); 
    }
    public void OptionsHandler()
    {

    }
    public void PauseButtonHandler()
    {
        PauseUI.SetActive(true);
        Time.timeScale = 0f;
    }
    public void ResumeButtonHandler()
    {
        PauseUI.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ExitButtonHandler()
    {
        Application.Quit();
    }
    public void SetVolume(float Volume)
    {
        audioMixer.SetFloat("Volume", Volume);
    }
    public void SetScreenMode(bool IsFullscreen)
    {
        Screen.fullScreen = IsFullscreen;
    }

}
