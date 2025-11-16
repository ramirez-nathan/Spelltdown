using UnityEngine;
using Oculus.Interaction;
using Unity.VisualScripting;
using System.Collections;
using System.Collections.Generic;

public class GestureEventProcessor : MonoBehaviour
{
    public bool recordingGesture = false;
    public Mivry mivry;
    Queue<GameObject> runeQueue = new Queue<GameObject>();
    public GameObject runeParticlePrefab;
    public bool isTrackingRight;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mivry = GetComponent<Mivry>();
    }

    // Update is called once per frame
    void Update()
    {
        bool lefttriggerDown = OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch);
        bool lefttriggerUp = OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch);
        bool righttriggerDown = OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch);
        bool righttriggerUp = OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch);

        if (righttriggerDown && !recordingGesture)
        {
            recordingGesture = true;
            mivry.InputAction_RightTriggerPressed = true;
            isTrackingRight = true;
        }
        else if (righttriggerUp && recordingGesture
                && isTrackingRight)
        {
            recordingGesture = false;
            mivry.InputAction_RightTriggerPressed = false;
            isTrackingRight = false;
        }
        else if (lefttriggerDown && !recordingGesture)
        {
            recordingGesture = true;
            mivry.InputAction_LeftTriggerPressed = true;
            isTrackingRight = false; // Explicitly set to false for left hand
        }
        else if (lefttriggerUp && recordingGesture
                && !isTrackingRight)
        {
            recordingGesture = false;
            mivry.InputAction_LeftTriggerPressed = false;
        }
        if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.LTouch) && runeQueue.Count > 0) 
        {
            foreach (var rune in runeQueue)
            {
                Rune runeScript = runeQueue.Peek().GetComponent<Rune>();
                runeQueue.Dequeue();
                runeScript.HandleDispell();
            }
        }
    }

    public void AddRuneToQueue(GameObject rune) 
    {
        runeQueue.Enqueue(rune);
    }

    public void OnGestureCompleted(GestureCompletionData gestureCompletionData)
    {
        Debug.Log($"[GestureEventProcessor] Gesture completed. " +
              $"id={gestureCompletionData.gestureID}, name={gestureCompletionData.gestureName}, " +
              $"similarity={gestureCompletionData.similarity}");

        // if gesture completion throws error
        if (gestureCompletionData.gestureID < 0)
        {
            string errorMessage = GestureRecognition.getErrorMessage(gestureCompletionData.gestureID);
            Debug.LogError($"[GestureEventProcessor] Gesture error: {errorMessage}");
            return;
        }
        if (gestureCompletionData.gestureName == "Circle") // left vibrate
        {
            Debug.Log("[GestureEventProcessor] Circle gesture -> left vibration");
            if (runeQueue.Peek().GetComponent<Rune>().type == RuneType.Circle)
            {
                //dispell rune
                Debug.Log("[GestureEventProcessor] Dispelled Circle Rune!");
                DispellRune();
            }
            // vibrate test
            OVRInput.SetControllerVibration(1, 0.4f, OVRInput.Controller.LTouch);
            StartCoroutine(VibrationTime(0.6f, "left"));
        }
        else if (gestureCompletionData.gestureName == "Triangle")
        {
            Debug.Log("[GestureEventProcessor] Triangle gesture -> right vibration");
            if (runeQueue.Peek().GetComponent<Rune>().type == RuneType.Triangle)
            {
                //dispell rune
                Debug.Log("[GestureEventProcessor] Dispelled Triangle Rune!");
                DispellRune();
            }
            OVRInput.SetControllerVibration(1f, 0.4f, OVRInput.Controller.RTouch);
            StartCoroutine(VibrationTime(0.6f, "right"));
        }
        else if (gestureCompletionData.gestureName == "Lightning")
        {
            Debug.Log("[GestureEventProcessor] Lightning gesture -> both vibrations");
            if (runeQueue.Peek().GetComponent<Rune>().type == RuneType.ZigZag)
            {
                //dispell rune
                Debug.Log("[GestureEventProcessor] Dispelled Lightning Rune!");
                DispellRune();
            }
            OVRInput.SetControllerVibration(1f, 0.4f, OVRInput.Controller.LTouch);
            OVRInput.SetControllerVibration(1f, 0.4f, OVRInput.Controller.RTouch);
            StartCoroutine(VibrationTime(0.6f, "both"));
        }
    }
    public void DispellRune()
    {
        Rune runeScript = runeQueue.Peek().GetComponent<Rune>();
        StartCoroutine(PlayRuneEffect(1.0f, runeQueue.Peek().transform));
        runeQueue.Dequeue();
        runeScript.HandleDispell();
    }

    private IEnumerator VibrationTime(float seconds, string handType)
    {
        yield return new WaitForSeconds(seconds);
        if (handType == "left")
        {
            OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.LTouch);
        }
        if (handType == "right")
        {
            OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
        }
        if (handType == "both")
        {
            OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.LTouch);
            OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
        }
    }

    private IEnumerator PlayRuneEffect(float seconds, Transform position)
    {
        GameObject runeParticle = Instantiate(runeParticlePrefab, position.position, transform.rotation);
        yield return new WaitForSeconds(seconds);
        Destroy(runeParticle);
    }
}
