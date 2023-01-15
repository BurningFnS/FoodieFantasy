using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private Vector3 startingLine;
    IEnumerator isDropperRunning; //This variable can change name. It is necessary to "pause" the loop because if u directly pause it, it will start from initial.
    [SerializeField]
    private GameObject[] groceriesList;

    [HideInInspector]
    public bool endGame = false;

    public float timeBetweenFood = 1.0f;

    public float timeLeft = 30f;
    public Text timerText;

    float GameHeight;
    float GameWidth;
    private float maxX;
    private float maxY;

    public GameObject dropperCamera;
    public GameObject platformerCamera;
    public GameObject dropperCanvas;
    public GameObject platformerCanvas;

    [HideInInspector] public float platformLength;
    public int maxPlatforms = 30;
    public GameObject platform;

    public float horizontalMin = 4f;
    public float horizontalMax = 8f;
    public float verticalMin = 0f;
    public float verticalMax = 4f;

    [HideInInspector]
    public Vector2 lastPlatformPosition;
    public GameObject player;
    private Vector2 playerPosition;
    private Vector2 randomPosition;
    [HideInInspector]
    public GameObject[] platformSpawn;
    public List <Vector2> platformList;

    public Basket basket;
    [HideInInspector]
    public bool isBasketFull = false;

    void Awake()
    {
        startingLine = new Vector3(Random.Range(-maxX, maxX), 6.2f, 0f);
    }

    // Start is called before the first frame update
    void Start()
    {
        isDropperRunning = GameLoop();
        StartCoroutine(isDropperRunning);

        CalculateGameDimensions();

        

        dropperCamera.SetActive(true);
        platformerCamera.SetActive(false);
        dropperCanvas.SetActive(true);
        platformerCanvas.SetActive(false);

        lastPlatformPosition = new Vector2(0, 45.76f);
        playerPosition = player.transform.position;
        platformLength = platform.GetComponent<SpriteRenderer>().bounds.size.x / 2;
        Spawn();

        player.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        startingLine = new Vector3(Random.Range(-maxX, maxX), maxY + 0.5f, 0f);
        isBasketFull = basket.isBasketFull;

        if (isBasketFull == true)
        {

            StopCoroutine(isDropperRunning);

            platformerCamera.SetActive(true);
            dropperCamera.SetActive(false);
            dropperCanvas.SetActive(false);
            platformerCanvas.SetActive(true);
            player.SetActive(true);
        }

        //DisplayTime(timeLeft);

        playerPosition = player.transform.position;
        if (playerPosition.y > (platformSpawn[maxPlatforms - 10].transform.position.y)) 
        {
            lastPlatformPosition = randomPosition;
            Spawn();
        }
    }

    IEnumerator GameLoop()
    {
        while (endGame == false)
        {
            GameObject grocery = Instantiate(groceriesList[Random.Range(0, groceriesList.Length)], startingLine, Quaternion.identity) as GameObject;
            grocery.name = grocery.name.Replace("(Clone)", "");
            yield return new WaitForSeconds(timeBetweenFood); // DELAY FOR EACH INSTANTIATE 


            Debug.Log("Game is running");
            yield return new WaitForSeconds(timeBetweenFood);
        }
    }


    public void CalculateGameDimensions()
    {
        GameHeight = Camera.main.orthographicSize * 2f;
        GameWidth = Camera.main.aspect * GameHeight;

        maxX = GameWidth / 2 - 1f;
        maxY = GameHeight / 2;
    }
    
    public void DisplayTime(float displayedTimeRemaining)
    {
        float seconds = Mathf.FloorToInt(displayedTimeRemaining);
        timerText.text = "Time Left: "+ seconds.ToString();
    }

    void Spawn()
    {
        platformSpawn = new GameObject[maxPlatforms];

        for (int i = 0; i < maxPlatforms; i++)
        {
            //randomPosition = new Vector2(lastPlatformPosition.x + platformLength, lastPlatformPosition.y) + new Vector2(Random.Range(horizontalMin, horizontalMax), Random.Range(verticalMin, verticalMax));
            //platformSpawn[i] = Instantiate(platform, randomPosition, Quaternion.identity);
            //lastPlatformPosition = randomPosition;
            randomPosition = lastPlatformPosition;
            randomPosition.x += Random.Range(horizontalMin, horizontalMax);
            randomPosition.y += Random.Range(verticalMin, verticalMax);
            platformSpawn[i] = Instantiate(platform, randomPosition, Quaternion.identity);
            lastPlatformPosition = randomPosition;
            platformList.Add(lastPlatformPosition);

        }
    }
}
