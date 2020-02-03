using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{

    public Button StartButton;
    public Button ExitButton;

    public void Start()
    {
        StartButton.onClick.AddListener(() => { StartGame(); });
        ExitButton.onClick.AddListener(() => { ExitGame(); });
    }

    private void StartGame ()
    {
        SceneManager.LoadScene("lvl_01");
    }


    private void ExitGame()
    {
        if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXEditor)
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }
        else
        {
            Application.Quit();
        }
    }
}
