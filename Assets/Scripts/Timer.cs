using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    private float elapsedTime = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        DisplayTime(elapsedTime);
    }
    void DisplayTime(float time)
    {
        float seconds = Mathf.FloorToInt(time);
        GameManager.instance.convertScore(Mathf.FloorToInt(time));
        timerText.text = $"{seconds}";
    }
}
