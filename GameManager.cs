using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    #region Singleton 
    public static GameManager Instance;


    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
    #endregion

    public float Currentscore = 0f;
    public int CurrentCoins;
    private SavedData data;
    public CameraController cam;
    public PlayerEndless player;
    public UnityEvent OnGameover = new UnityEvent();
    public UnityEvent OnStart = new UnityEvent();

    public float Milktimr = 5;

    public bool IsPlaying = false;

    private void Start()
    {
        Milktimr=5;
        string LoadedData = SaveSystem.Load("save");

        if(LoadedData != null)
        {
            data = JsonUtility.FromJson<SavedData>(LoadedData);
        }
        else
        data = new SavedData();

        
    }
    public void GameStart()
    {
        OnStart.Invoke();
        IsPlaying = true;
        Currentscore = 0;
        CurrentCoins = 0;
        player.ResetCoins();
    }

    private void Update()
    {
        if (IsPlaying)
        {
            Currentscore = cam.transform.position.y;
            CurrentCoins = player.Coin;
        }


    }
    public string PreatyScore() 
    { 
      return Mathf.RoundToInt(Currentscore).ToString();
    }
    public string PreatyHighScore()
    {
        return Mathf.RoundToInt(data.HighScore).ToString();
    }
    public string Coins()
    {
       return data.Coins.ToString();
    }
    public void PowerUpgrade()
    {

    }
    public void Gameover()
    {
        
        if(data.HighScore < Currentscore)
            data.HighScore = Currentscore;
        
        data.Coins += CurrentCoins;
        string SaveString = JsonUtility.ToJson(data);
        SaveSystem.Save("save", SaveString);

        IsPlaying = false;
        OnGameover.Invoke();
    }
}
