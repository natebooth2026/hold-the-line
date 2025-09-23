using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControl : MonoBehaviour
{
    [SerializeField] private string GAME_SCENE;
    public void StartButton()
    {
        SceneManager.LoadScene(GAME_SCENE);
    }

    public void ExitButton()
    {
        Application.Quit();
    }
}
