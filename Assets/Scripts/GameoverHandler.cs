using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameoverHandler : MonoBehaviour
{
    [SerializeField] GameObject newHighText;
    // Start is called before the first frame update
    void Awake()
    {
        if(PlayerPrefs.GetInt("NewHigh") == 1)
        {
            PlayerPrefs.SetInt("NewHigh", 0);
            newHighText.SetActive(true);
            PlayerPrefs.Save();
        }
    }
}
