using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameoverToggle : MonoBehaviour
{
    [SerializeField] HealthBarManager healthTracker;
    [SerializeField] string gameoverSceneName;
    public KillsTextManager killsScript;

    void Awake()
    {
        killsScript = GetComponent<KillsTextManager>();
        if(!PlayerPrefs.HasKey("HighScore") && !PlayerPrefs.HasKey("NewHigh"))
        {
            PlayerPrefs.SetInt("HighScore", 0);
            PlayerPrefs.SetInt("NewHigh", 0);
            PlayerPrefs.Save();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(healthTracker != null && healthTracker.currentHealth <= 0)
        {
            if (PlayerPrefs.HasKey("HighScore") && PlayerPrefs.HasKey("NewHigh"))
            {
                if(PlayerPrefs.GetInt("HighScore") < killsScript.kills)
                {
                    PlayerPrefs.SetInt("HighScore", killsScript.kills);
                    PlayerPrefs.SetInt("NewHigh", 1);
                    PlayerPrefs.Save();
                }
            }
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SceneManager.LoadScene(gameoverSceneName);
        }
    }
}
