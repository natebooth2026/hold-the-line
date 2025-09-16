using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class MenuControl : MonoBehaviour
{
    public string GAME_SCENE;

    public void StartButton()
    {
        SceneManager.LoadScene(GAME_SCENE);
    }

    public void ExitButton()
    {
        Application.Quit();
    }
}
