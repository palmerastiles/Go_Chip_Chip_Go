using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class LevelGenerator : MonoBehaviour
{
    [Header("Transforms")]
    public Transform LevelStartPosition;
    public Transform Grid;
    public List<Transform> prefab;
    public List<Transform> ItemPrefab;
    private List<Transform> generatedPlatforms = new List<Transform>();

    [Header("GameObjects")]
    public GameObject player;
    public CameraController cameraController;
    private GameManager gm;
    public Transform ObjectManager;

    [Header("Numbers")]
    private const float PlayerDistance = 10f;
    private Vector3 lastEndposition;
    private Transform ItemSpawnerPosition;
    private Vector3 ItemSpawner;
    public int digit;

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
                digit = Random.Range(0, 101);
                SpawnlevelPart();
            print(digit);
                
            }
    }

    // Spawns some platforms for starting
    private void Awake()
    {
        //Searches the end position of the last platform 
        lastEndposition = LevelStartPosition.Find("LevelpartEnd").position;

        
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
        Transform ChosenPrefab =prefab[Random.Range(0, prefab.Count)];

       Transform ChosenPowerUp = ItemPrefab[Random.Range(1, ItemPrefab.Count)];

        //Spawns the next platform on the end point of the last platform
        Transform LastLevelPart =
            SpawnlevelPart(
                ChosenPrefab,
                lastEndposition
            );

        Transform ItemSpawnerPosition = LastLevelPart.Find("ItemSpawner");
        if (ItemSpawnerPosition == null)
        {
            Debug.LogError(
                "No se encontró ItemSpawner dentro de: " +
                LastLevelPart.name
            );

            return;
        }
        ItemSpawner = ItemSpawnerPosition.position;



        //finds the end position of each platform
        lastEndposition =
            LastLevelPart.Find("LevelpartEnd").position;

        generatedPlatforms.Add(LastLevelPart);
        //Calls the camera for the last part spawned
        cameraController.OffScreenArrow(
            LastLevelPart
        );

       // print(ItemSpawner);
        if(digit >= 6)
        {  
            Instantiate( ItemPrefab[0],ItemSpawner,Quaternion.identity);
        }
        else if(digit<=5)
        { 
            Instantiate(ChosenPowerUp,ItemSpawner,Quaternion.identity);
        }
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