using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    PlayerEndless Plyr;
    GameManager gm;
    public float Milktimer=5 ;

    public float Pilltimer = 5;
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
      
        milkActive = true;

        // Impulso inicial
        Plyr.rigidBody.velocity = Vector2.zero;
        Plyr.rigidBody.AddForce(Vector2.up * 10f, ForceMode2D.Impulse);

        // Gravedad reducida
        Plyr.rigidBody.gravityScale = 0.1f;

        // Ignorar colisión con plataformas
        Physics2D.IgnoreLayerCollision(
            Plyr.gameObject.layer,
            LayerMask.NameToLayer("Platform"),
            true
        );

        // Guardar los colliders
        Collider2D[] colliders = Plyr.GetComponents<Collider2D>();

        // Desactivar colliders
        foreach (Collider2D col in colliders)
        {
            col.enabled = false;
        }

        // Duración del Milk
        yield return new WaitForSeconds(Milktimer);

        // Restaurar gravedad
        Plyr.rigidBody.gravityScale = 1f;

        // IMPORTANTE:
        // Primero mantenemos la colisión ignorada
        // y buscamos sacar al jugador de la plataforma.
        int intentos = 0;

        while (IsInObstale() && intentos < 30)
        {
            Plyr.rigidBody.position += Vector2.up * 0.2f;

            Physics2D.SyncTransforms();

            intentos++;

            yield return new WaitForFixedUpdate();
        }

        // Volver a activar los colliders
        foreach (Collider2D col in colliders)
        {
            col.enabled = true;
        }

        // Dejamos de ignorar las plataformas
        Physics2D.IgnoreLayerCollision(
            Plyr.gameObject.layer,
            LayerMask.NameToLayer("Platform"),
            false
        );

        milkActive = false;
    }
    private bool IsInObstale()
    {
        Collider2D playerCollider = Plyr.colider;

        Collider2D[] objects = Physics2D.OverlapCircleAll(
            playerCollider.bounds.center,
            playerCollider.bounds.extents.x
        );

        foreach (Collider2D obj in objects)
        {
            if (obj.CompareTag("Platform"))
            {
                return true;
            }
        }

        return false;
    }
    public void Pill()
    {
        if (pillActive) return;
            StartCoroutine(PillEffect());
    }
    private IEnumerator PillEffect()
    {
       
        pillActive = true;

        Plyr.JumpForce *= 2;

        yield return new WaitForSeconds(Pilltimer);

        Plyr.JumpForce /= 2;

        pillActive = false;
    }
}
