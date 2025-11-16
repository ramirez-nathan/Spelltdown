using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    public TextMeshProUGUI score;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void displayScore()
    {
        score.text = "Score: " + GameManager.instance.score.ToString() + "\nBest: " + GameManager.instance.highScore.ToString();
    }

}
