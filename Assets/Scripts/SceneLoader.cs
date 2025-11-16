using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Management;
using System.Collections;
using NUnit.Framework.Constraints;

public class SceneLoader : MonoBehaviour
{
    public void LoadSceneByName(string sceneName)
    {
        Debug.Log("Start loading scene: " + sceneName);
        StartCoroutine(LoadSceneWithXR(sceneName));
    }

    IEnumerator LoadSceneWithXR(string sceneName)
    {
        yield return XRGeneralSettings.Instance.Manager.InitializeLoader();

        if (XRGeneralSettings.Instance.Manager.activeLoader == null)
        {
            Debug.LogError("XR loader failed to initialize!");
            yield break;
        }

        XRGeneralSettings.Instance.Manager.StartSubsystems();
        Debug.Log("XR initialized. Loading scene: " + sceneName);
        SceneManager.LoadScene(sceneName);
    }

    public void PlayGame()
    {
        GameManager.instance.GoToPlay();
    }

    public void QuitGame()
    {
        Debug.Log("Quit button pressed!");
        Application.Quit();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
