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
    private List<Transform> generatedItems = new List<Transform>();

    [Header("GameObjects")]
    public GameObject player;
    public CameraController cameraController;
    private GameManager gm;
    public Transform ObjectManager;

    [Header("Numbers")]
    private const float PlayerDistance = 10f;
    private Vector3 lastEndposition;
    private Vector3 LastSpawnerPosition;
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
        digit = Random.Range(0, 101);
        //Searches the end position of the last platform 
        lastEndposition = LevelStartPosition.Find("LevelpartEnd").position;

        
        SpawnlevelPart();
        //Spawns some platforms for the start of the game
        int StartingParts = 5;

        for (int i = 0; i < StartingParts; i++)
        {
            SpawnlevelPart();
            print(digit);
        }

       
    }

    //Spawns the platforms from an array
    private void SpawnlevelPart()
    {
        //Selects a random platform
        Transform ChosenPrefab =prefab[Random.Range(0, prefab.Count)];

   

        //Spawns the next platform on the end point of the last platform
        Transform LastLevelPart = SpawnlevelPart(ChosenPrefab,lastEndposition);


        

        lastEndposition =LastLevelPart.Find("LevelpartEnd").position;

        ItemSpawnerPosition = LastLevelPart.Find("ItemSpawner");


        ItemSpawner = ItemSpawnerPosition.position;


        //finds the end position of each platform

        generatedPlatforms.Add(LastLevelPart);

        Transform ChosenPowerUp;

        if (digit >= 5)
        {
            ChosenPowerUp = ItemPrefab[0];
        }
        else
        {
            ChosenPowerUp = ItemPrefab[
                Random.Range(1, ItemPrefab.Count)
            ];
        }

        Transform spawnedItem = Instantiate(
        ChosenPowerUp,
        ItemSpawner,
        Quaternion.identity,
        Grid
    );
        generatedItems.Add(spawnedItem);

        //Calls the camera for the last part spawned
        cameraController.OffScreenArrow(LastLevelPart);

       // print(ItemSpawner);
       
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
        foreach (Transform item in generatedItems)
        {
            if (item != null)
            {
                
                Destroy(item.gameObject);
            }
        }

        generatedPlatforms.Clear();
        generatedItems.Clear();
        
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