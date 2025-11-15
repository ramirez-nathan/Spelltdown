using UnityEngine;
using Oculus.Interaction;
using Unity.VisualScripting;
using System.Collections;

public class GestureEventProcessor : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
            // vibrate test
            OVRInput.SetControllerVibration(1, 0.4f, OVRInput.Controller.LTouch);
            StartCoroutine(VibrationTime(0.6f, "left"));
        }
        else if (gestureCompletionData.gestureName == "Triangle")
        {
            Debug.Log("[GestureEventProcessor] Triangle gesture -> right vibration");
            OVRInput.SetControllerVibration(1f, 0.4f, OVRInput.Controller.RTouch);
            StartCoroutine(VibrationTime(0.6f, "right"));
        }
        else if (gestureCompletionData.gestureName == "Lightning")
        {
            Debug.Log("[GestureEventProcessor] Lightning gesture -> both vibrations");
            OVRInput.SetControllerVibration(1f, 0.4f, OVRInput.Controller.LTouch);
            OVRInput.SetControllerVibration(1f, 0.4f, OVRInput.Controller.RTouch);
            StartCoroutine(VibrationTime(0.6f, "both"));
        }
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
