using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuPLataformero : MonoBehaviour
{
    public Button Level1;
    public Button Level2;
    public Button Level3;

    public Button Endless;
    public Button Salir;


    // Start is called before the first frame update
    void Start()
    {
        Level1.onClick.AddListener(LoadLevel1);
        Level2.onClick.AddListener(LoadLevel2);
        Level3.onClick.AddListener(LoadLevel3);
        Endless.onClick.AddListener(LoadLevelEndless);
        Salir.onClick.AddListener(SalirJuego);


    }
    void LoadLevel1()
    {
        SceneManager.LoadScene("Level 1");
    }
    void LoadLevel2()
    {
        SceneManager.LoadScene("Level 2");
    }
    void LoadLevel3()
    {
        SceneManager.LoadScene("Level 3");
    }
    void LoadLevelEndless()
    {
        SceneManager.LoadScene("Endless");
    }
    
    void SalirJuego()
    {
        Application.Quit();
    }


}
