using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class bootSceneController : MonoBehaviour
{
    //will not be used
    public static int level = 1;
    private void Awake()
    {
        SceneManager.LoadScene(1);
        SceneManager.sceneLoaded += OnLoadNewLevel;
        DontDestroyOnLoad(gameObject);
        SetFPS();
    }

    private void SetFPS()
    {
        Application.targetFrameRate = 60;
    }

    void OnLoadNewLevel(Scene scene, LoadSceneMode mode)
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        if(currentScene == 0)
        {
            return;
        }
        else if(currentScene < SceneManager.sceneCountInBuildSettings - 1)
        {
            SceneManager.LoadScene(currentScene + 1);
        }
        else
        {
            SceneManager.LoadScene(1);
        }
        
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnLoadNewLevel;
    }
}
