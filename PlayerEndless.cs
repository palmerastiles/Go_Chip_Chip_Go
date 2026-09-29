using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerEndless : MonoBehaviour
{


    [Header("References")]
    GameManager gm;
    ObjectManager om;
    public SpriteRenderer Renderer;
    public Animator Animador;
    public Rigidbody2D rigidBody;

    public CircleCollider2D colider;

    [Header("Numbers")]
    private int Jumps = 2;
    public int Coin = 0;

    Vector3 InitialPosition;


    [Header("Booleans")]
    private bool CanMove;

    [Header("UI")]
    public TextMeshProUGUI pointsUI;



    [Header("Audio")]
    public AudioSource Juming;
    public AudioSource PickupSound;
    public AudioSource EndSound;
    public AudioSource HurtSound;
    public AudioSource DeathSound;

    [Header("Materials")]
    public Material material;


    private void Start()
    {
        om = FindObjectOfType<ObjectManager>();
        gm = GameManager.Instance;

        Jumps = 0;
        CanMove = false;

        InitialPosition = transform.position;
        gm.OnStart.AddListener(RevivePlayer);

        ResetCoins();
    }
    //Reactivates the player on game over
    private void RevivePlayer()
    {
        gameObject.SetActive(true);
    }
    void Update()
    {
        //if not playing the player cant move 
        if (gm.IsPlaying)
        {
            //Moves The Background Relative to the player on X axis
            material.SetFloat("_PlayerX", transform.position.x);
            //Apply movement
            Movement();
            CanMove = true;
        }

        //Block the rotation of the sprite with the game paused
        if (CanMove == false)
            Animador.SetBool("Is Running", true);


        else if (Input.GetAxis("Horizontal") != 0)
            Animador.SetBool("Is Running", false);

        else
            Animador.SetBool("Is Running", true);





    }
    public void Movement()
    {
        //Moves the player left to right
        rigidBody.velocity = new Vector2(Input.GetAxis("Horizontal") * 5, rigidBody.velocity.y);

        if (Input.GetAxis("Horizontal") > 0)
            Renderer.flipX = false;


        else if (Input.GetAxis("Horizontal") < 0)
            Renderer.flipX = true;

        if (Input.GetKeyDown(KeyCode.Space) && Jumps > 0)
        {

            Juming.Play();
            Animador.SetTrigger("Jump");
            rigidBody.velocity = Vector2.zero;
            rigidBody.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
            Jumps--;

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Detects the coins collected by the player
        if (collision.gameObject.GetComponent<Moneda>())
        {
            Coin += 10;
            PickupSound.Play();
            Destroy(collision.gameObject);
            pointsUI.text = "Coins: " + Coin;
        }
        //Detects if the player falls on the killzone
        if (collision.gameObject.GetComponent<Killzone>())
        {
            gameObject.SetActive(false);
            gm.Gameover();
            transform.position = InitialPosition;


        }
        if (collision.gameObject.GetComponent<PickableObject>())
        {
            om.milk();
        }




    }



    //Detects if the player Can jump again 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Jumps = 2;
        //parents the player to the moving platfor for preventing visual glitches
        if (collision.gameObject.GetComponent<PlataformaMovil>())
        {
            transform.parent = collision.transform;
        }

    }

    //Remove the player from the parent platform
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlataformaMovil>())
            transform.parent = null;

    }

    public void ResetCoins()
    {
        pointsUI.text = "Coins: " + 0;
        Coin = 0;
    }

    
}
