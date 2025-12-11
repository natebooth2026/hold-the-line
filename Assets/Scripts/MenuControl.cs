using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Accessibility;

public class MenuControl : MonoBehaviour
{
    [SerializeField] private string GAME_SCENE;
    [SerializeField] private string MENU_SCENE;
    [SerializeField] TextMeshProUGUI highScoreText;
    private bool colorblind = false;

    void Awake()
    {
        if (!PlayerPrefs.HasKey("HighScore") && highScoreText != null)
        {
            highScoreText.text = "High Score: 0";
        } else if (highScoreText != null)
        {
            highScoreText.text = "High Score: " + PlayerPrefs.GetInt("HighScore").ToString();
        }
    }
    public void StartButton()
    {
        SceneManager.LoadScene(GAME_SCENE);
    }

    public void ReturnButton()
    {
        SceneManager.LoadScene(MENU_SCENE);
    }

    public void ExitButton()
    {
        Application.Quit();
    }
}
