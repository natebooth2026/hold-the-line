using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameoverToggle : MonoBehaviour
{
    [SerializeField] HealthBarManager healthTracker;
    [SerializeField] string gameoverSceneName;

    // Update is called once per frame
    void Update()
    {
        if(healthTracker != null && healthTracker.currentHealth <= 0)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SceneManager.LoadScene(gameoverSceneName);
        }
    }
}
