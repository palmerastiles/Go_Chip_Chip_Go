using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewBehaviourScript : MonoBehaviour
{
    [Header("GameObjects")]
    public GameObject DerrotaUI;
    public GameObject WinUI;

    [Header("References")]
    public SpriteRenderer Renderer;
    public Animator Animador;
    public Rigidbody2D rigidBody;

    [Header("Integers")]
    private int Jumps = 2;
    public int Vida;
    public int Puntos;

    [Header("Booleans")]
    private bool Victoria;

    [Header("UI")]
    public TextMeshProUGUI pointsUI;
    public TextMeshProUGUI VidasUI;

    [Header("Audio")]
    public AudioSource Juming;
    public AudioSource PickupSound;
    public AudioSource EndSound;
    public AudioSource HurtSound;
    public AudioSource DeathSound;

    [Header("Materials")]
    public Material material;
     


    Vector3 InitialPosition;

    private void Start()
    {
        InitialPosition = transform.position;
        Vida = 3;
    }
    void Update()
    {
        material.SetFloat("_PlayerX", transform.position.x);

        rigidBody.velocity = new Vector2(Input.GetAxis("Horizontal") * 5, rigidBody.velocity.y);

        if (Input.GetAxis("Horizontal") > 0)
            Renderer.flipX =false;
      
 
        else if (Input.GetAxis("Horizontal") < 0)
            Renderer.flipX = true;
        




        if (Input.GetAxis("Horizontal") !=0)
            Animador.SetBool("Is Running", false);

        else
            Animador.SetBool("Is Running", true);

        if (Input.GetKeyDown(KeyCode.Space) && Jumps > 0)
        {
            Juming.Play();
            rigidBody.velocity = Vector2.zero;
            rigidBody.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
            Jumps--;
            
        }

        if(transform.position.y < -6)
        {
            transform.position = InitialPosition;
            rigidBody.velocity = Vector3.zero;
            HurtSound.Play();
            Vida --;
            VidasUI.text = "Vidas: " + Vida;
            print (Vida);
        }
        if (Vida == 0)
        {
           
            DerrotaUI.SetActive(true);
            Time.timeScale = 0;
            
        }
       else if (Vida !=0 && !Victoria)
        {
            Time.timeScale = 1;
        }
       

        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Moneda>())
        {
            Puntos += 10;
            PickupSound.Play();
            Destroy(collision.gameObject);
            pointsUI.text = "Coin: " + Puntos;
        }
        if(collision.gameObject.GetComponent<ZonaFinal>() && Puntos == 100)
        {
            Victoria = true;
            EndSound.Play();
            WinUI.SetActive(true);
            Time.timeScale = 0;
            
        }
       
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Jumps = 2;

        if (collision.gameObject.GetComponent<PlataformaMovil>())
        {
            transform.parent = collision.transform;
        }

        if (collision.gameObject.GetComponent<BloqueDaño>())
        {
            transform.position = InitialPosition;
            rigidBody.velocity = Vector3.zero;
            HurtSound.Play();
            Vida--;
            VidasUI.text = "Vidas: " + Vida;
            Time.timeScale = 0;
        }

    }
   

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<PlataformaMovil>())
            transform.parent = null;

    }
}
