using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
    public Button Menu;
    public Button Reinicio;

    void Start()
    {
        Menu.onClick.AddListener(GoMenu);
        Reinicio.onClick.AddListener(Reload);

    }

    void GoMenu()
    {
        
        
        
    }
    void Reload()
    {
        
        
    }
}
