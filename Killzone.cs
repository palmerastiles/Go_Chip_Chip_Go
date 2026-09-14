using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Killzone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
       //Deload the platforms bellow the screen
        if (collision.CompareTag("Platfom"))
        {
            Destroy(collision.gameObject);
        }

        
    }
}
