using UnityEditor;
using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameState state;
    public Score scoreDisplay;
    public GameObject menuCanvas;
    public GameObject playCanvas;
    public GameObject gameOverCanvas;
    public GameObject runeSpawner;

    public int score = 0;
    public int highScore = 0;
    public float timeWindow = 5.0f;
    public float elapsedTime = 0.0f;
    private void Awake()
    {
        instance = this;
        GoToMenu();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is create
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        scoreDisplay.displayScore();
        
        if (score > highScore)
        {
            highScore = score;
        }
        if (elapsedTime > timeWindow)
        {
            elapsedTime = 0.0f;
            GoToGameOver();
        }
    }

    public void GoToMenu()
    {
        state = GameState.MENU;
        runeSpawner.SetActive(true);
        score = 0;
        menuCanvas.SetActive(true);
        playCanvas.SetActive(false);
        gameOverCanvas.SetActive(false);
    }

    public void GoToPlay()
    {
        state = GameState.PLAY;
        menuCanvas.SetActive(false);
        playCanvas.SetActive(true);
        gameOverCanvas.SetActive(false);
    }

    public void GoToGameOver()
    {
        state = GameState.GAME_OVER;
        menuCanvas.SetActive(false);
        playCanvas.SetActive(false);
        gameOverCanvas.SetActive(true);
    }
}

public enum GameState {
    MENU,
    PLAY,
    GAME_OVER
}