using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class LevelGenerator : MonoBehaviour
{
    [Header("Transforms")]
    public Transform LevelStartPosition;
    public Transform Grid;
    public List<Transform> prefab;

    private List<Transform> generatedPlatforms = new List<Transform>();

    [Header("GameObjects")]
    public GameObject player;
    public CameraController cameraController;
    private GameManager gm;
    [Header("Numbers")]
    private const float PlayerDistance = 10f;
    private Vector3 lastEndposition;

    private void Start()
    {
        gm = GameManager.Instance;
    }

    
    private void Update()
    {
        
        
        
            //Spawns Platforms Relative to the player
            if (Vector3.Distance(
                player.transform.position,
                lastEndposition) < PlayerDistance)
            {
            
                SpawnlevelPart();
            }
    }

    // Spawns some platforms for starting
    private void Awake()
    {
        //Searches the end position of the last platform 
        lastEndposition =
            LevelStartPosition.Find("LevelpartEnd").position;

        SpawnlevelPart();
        //Spawns some platforms for the start of the game
        int StartingParts = 5;

        for (int i = 0; i < StartingParts; i++)
        {
            SpawnlevelPart();
        }

       
    }

    //Spawns the platforms from an array
    private void SpawnlevelPart()
    {
        //Selects a random platform
        Transform ChosenPrefab =
            prefab[Random.Range(0, prefab.Count)];

        //Spawns the next platform on the end point of the last platform
        Transform LastLevelPart =
            SpawnlevelPart(
                ChosenPrefab,
                lastEndposition
            );

        
        //finds the end position of each platform
        lastEndposition =
            LastLevelPart.Find("LevelpartEnd").position;

           generatedPlatforms.Add(LastLevelPart);
        //Calls the camera for the last part spawned
        cameraController.OffScreenArrow(
            LastLevelPart
        );
    }

    //Spawns the platforms on the grid
    private Transform SpawnlevelPart(Transform prefab,Vector3 SpawnPosition)
    {
        Transform LevelPartTransform =
            Instantiate(
                prefab,
                SpawnPosition,
                Quaternion.identity,
                Grid
            );

        return LevelPartTransform;
    }
    //Destroys the platfors created on game over
    public void OnGameOver()
    {
        foreach (Transform platform in generatedPlatforms)
        {
            if (platform != null)
            {
                Destroy(platform.gameObject);
            }
        }

        generatedPlatforms.Clear();
    }
    
    //Restarts the platform generation
    public void RestartLevel()
    {
        OnGameOver();

        lastEndposition =
            LevelStartPosition.Find("LevelpartEnd").position;

        for (int i = 0; i < 5; i++)
        {
            SpawnlevelPart();
        }
    }
}