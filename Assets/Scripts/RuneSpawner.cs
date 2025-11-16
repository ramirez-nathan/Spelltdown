using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class RuneSpawner : MonoBehaviour
{
    [Header("Rune Prefab")]
    public GameObject RunePrefab;
    //public Rune runeScript; 
    // spawn fruit object with wholelayer


    [Header("Spawn Settings")]
    public Transform spawnPoint;
    public Vector3 velocityToSet;
    public GestureEventProcessor gestureEventProcessor;

    private Coroutine spawnCoroutine;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartSpawning();
    }

    public void StartSpawning()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
        }
        spawnCoroutine = StartCoroutine(SpawnRuneCoroutine());
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    private void OnEnable() // turn on when entering play state
    {
        StartSpawning();
    }

    private IEnumerator SpawnRuneCoroutine()
    {
        while (true)
        {
            // wait for a rand interval between min and max
            float waitTime = 4;
            yield return new WaitForSeconds(waitTime);

            // Generate a random integer between 1 and 100 (inclusive)
            int randomNumberInRange = Random.Range(0,3);
            RuneType runeType;
            if (randomNumberInRange == 0)
            { // circle 
                runeType = RuneType.Circle;
            }
            else if (randomNumberInRange == 1)
            { // triangle
                runeType = RuneType.Triangle;
            }
            else
            { // zigzag
                runeType = RuneType.ZigZag;
            }
            SpawnRune(runeType);
        }
    }

    public void SpawnRune(RuneType runeType)
    {
        // Check if we have fruit prefabs
        if (RunePrefab == null)
        {
            Debug.LogWarning("No rune prefabs assigned to FruitSpawner!");
            return;
        }
        if (spawnPoint == null)
        {
            Debug.LogWarning("No spawn points assigned!");
            return;
        }

        GameObject rune = Instantiate(RunePrefab, spawnPoint.position, transform.rotation);
        rune.GetComponent<Rune>().SetRuneType(runeType);
        Debug.Log($"I am born as rune! {rune.GetComponent<Rune>().type}");
        gestureEventProcessor.AddRuneToQueue(rune);
        Rigidbody rb = rune.GetComponent<Rigidbody>();

        // adjust runes gravity
        if (rb != null)
        {
            Debug.Log("got rigidbody");
        }
    }


    // Update is called once per frame
    void Update()
    {
        /*if (OVRInput.GetDown(OVRInput.Button.One))
        {
            SpawnFruit();
        }*/

    }
}
