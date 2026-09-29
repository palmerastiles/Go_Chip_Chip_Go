using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    PlayerEndless Plyr;
    GameManager gm;
    public float Milktimer=5 ;

    public float Pilltimer = 10;
    private bool milkActive = false;
    private bool pillActive = false;

  

    private void Start()
    {
        Plyr = FindObjectOfType<PlayerEndless>();
    }
    public void milk()
    {
        if (milkActive) return;

        StartCoroutine(MilkEffect());
    }
    private IEnumerator MilkEffect()
    {
        print("Got milk");
        milkActive = true;

        Plyr.rigidBody.AddForce(Vector2.up * 10f, ForceMode2D.Impulse);


        Plyr.rigidBody.gravityScale = 0.1f;

        Collider2D[] colliders =
       Plyr.GetComponents<Collider2D>();

        foreach (Collider2D col in colliders)
        {
            col.enabled = false;
        }

        yield return new WaitForSeconds(Milktimer);

        Plyr.rigidBody.gravityScale = 1f;
        foreach (Collider2D col in colliders)
        {
            col.enabled = true;
        }

        milkActive = false;
    }
    public void Pill()
    {
        if (pillActive) return;
            StartCoroutine(PillEffect());
    }
    private IEnumerator PillEffect()
    {
        print("got pill");
        pillActive = true;

        Plyr.rigidBody.AddForce(Vector2.up * 20, ForceMode2D.Impulse);

        yield return new WaitForSeconds(Pilltimer);

        pillActive = false;
    }
}
