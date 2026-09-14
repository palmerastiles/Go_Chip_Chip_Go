using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SocialPlatforms.Impl;
public class CameraController : MonoBehaviour
{
    [Header("GameObjects")]
    public GameObject MenuUI;
    public Camera cam;
    public GameObject player;
    public TextMeshProUGUI ScoreUI;

    GameManager gm;

    [Header("Floats")]
    public float CameraSpeed = 0.3f;
    private float TimeAlive;
    private float SpeedUp;
    public string Score;

    [Header("Transforms")]
    private Vector3 InitialPosition;
    public RectTransform Arrow;
    private Transform nextPlatform;

    [Header("Materials")]
    public Material material;
    private Vignette vignette;

    [Header("Danger Effect")]
    public Volume dangerVolume;
    public float dangerStart = 0.35f;
    public float maxDanger = 0.75f;
void Start()
    {

        gm = GameManager.Instance;

        InitialPosition = transform.position;
    
        
        TimeAlive = 1f;
        dangerVolume.profile.TryGet(out vignette);

      
    }

    void Update()
    {
        //Paralax Background on Y
        material.SetFloat("_PlayerY", transform.position.y);

       
        //Moves the paralax background on X by following the player
        Vector3 Playerpos = player.transform.position;
        Vector3 pos = transform.position;
        pos.x = Playerpos.x;
        transform.position = pos;

        UpdatePlatformWarning();

        // Tracks the player score
        Score = gm.PreatyScore();
        ScoreUI.text = "Score: " + Score;
        if (gm.IsPlaying == false)
            Arrow.gameObject.SetActive(false);
            
    }
    //Calculates the Camera Speeds
    private void SpeedCalculation()
    {
        SpeedUp = CameraSpeed * Mathf.Pow(TimeAlive, CameraSpeed);
    }
    private void LateUpdate()
    {
        if (gm.IsPlaying == true)
        {
            SpeedCalculation();

            Vector3 playerPos = player.transform.position;
            Vector3 cameraPos = transform.position;

            // Follows the player on the X axis
            cameraPos.x = playerPos.x;

            // Vertical distance between the camera and the player
            float playerRelativeY = playerPos.y - cameraPos.y;

            // Maximum Height for following
            float followHeight = 1.5f;

            if (playerRelativeY > followHeight)
            {
                // Moves upward the camera towards the player
                cameraPos.y = playerPos.y - followHeight;
            }
            else
            {
                //Camera keeps moving upwars
                cameraPos.y += SpeedUp * Time.deltaTime;
            }

            cameraPos.z = -10;

            transform.position = cameraPos;
        }
        UpdateDangerEfect();
        if(!gm.IsPlaying)
        {
            transform.position = InitialPosition;
        }
    }
    //Shows an arrow on the sides of the screen
    public void OffScreenArrow(Transform platform)
    {
        nextPlatform = platform;
     
        Vector3 platformScreenPos = cam.WorldToViewportPoint(platform.position);

        //Hides the arrow
        Arrow.gameObject.SetActive(false);

        // Shows the arrow for the platforms that spawns far to the left
        if (platformScreenPos.x < 0.15f)
        {
            Arrow.gameObject.SetActive(true);

            // Places the arrow on the left
            Arrow.position = new Vector3(
                50f,
                Screen.height / 2f,
                0f
            );

            // Rotates the arrow to the left
            Arrow.rotation = Quaternion.Euler(0, 0, 0);
        }

        // Shows the arrow for the platforms that spawns far to the right
        else if (platformScreenPos.x > 0.85f)
        {
            Arrow.gameObject.SetActive(true);

            // Places the arrow on the right
            Arrow.position = new Vector3(
                Screen.width - 50f,
                Screen.height / 2f,
                0f
            );

            // Rotates the arrow on the right
            Arrow.rotation = Quaternion.Euler(0, 0, 180);
        }



    }
    //Increce the vignette value near the bottom part of the screen
    public void UpdateDangerEfect()
    {
        Vector3 playerScreenPos = cam.WorldToViewportPoint(player.transform.position);

        float danger = 0f;

        if (playerScreenPos.y < dangerStart)
        {
            danger = Mathf.InverseLerp(dangerStart,0f,playerScreenPos.y);
        }

        float targetIntensity =
            Mathf.Lerp(0f, maxDanger, danger);

        vignette.intensity.value = Mathf.Lerp(
            vignette.intensity.value,
            targetIntensity,
            5f * Time.unscaledDeltaTime
        );
    }
    //Detects the platfors outside of the screen
    private void UpdatePlatformWarning()
    {
        if (nextPlatform == null)
        {
            Arrow.gameObject.SetActive(false);
            return;
        }

        Vector3 platformScreenPos =
            cam.WorldToViewportPoint(nextPlatform.position);

        // Platfor is on the left
        if (platformScreenPos.x < 0.15f)
        {
            Arrow.gameObject.SetActive(true);

            Arrow.position = new Vector3(
                50f,
                Screen.height / 2f,
                0f
            );

            Arrow.rotation =
                Quaternion.Euler(0, 0, 0);
        }

        // Platform is on the right
        else if (platformScreenPos.x > 0.85f)
        {
            Arrow.gameObject.SetActive(true);

            Arrow.position = new Vector3(
                Screen.width - 50f,
                Screen.height / 2f,
                0f
            );

            Arrow.rotation =
                Quaternion.Euler(0, 0, 180);
        }

        // The platfor is centered
        else
        {
            Arrow.gameObject.SetActive(false);
        }
    }

}
