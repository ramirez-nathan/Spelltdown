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
    public bool isTrackingRight;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mivry = GetComponent<Mivry>();
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch) && !recordingGesture)
        {
            recordingGesture = true;
            mivry.InputAction_RightTriggerPressed = true;
            isTrackingRight = true;
        }
        else if (OVRInput.GetUp(OVRInput.Button.One, OVRInput.Controller.RTouch) && recordingGesture
                && isTrackingRight)
        {
            recordingGesture = false;
            mivry.InputAction_RightTriggerPressed = false;
            isTrackingRight = false;
        }
        else if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.LTouch) && !recordingGesture)
        {
            recordingGesture = true;
            mivry.InputAction_LeftTriggerPressed = true;
            isTrackingRight = false; // Explicitly set to false for left hand
        }
        else if (OVRInput.GetUp(OVRInput.Button.One, OVRInput.Controller.LTouch) && recordingGesture
                && !isTrackingRight)
        {
            recordingGesture = false;
            mivry.InputAction_LeftTriggerPressed = false;
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
}
