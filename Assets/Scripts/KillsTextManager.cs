using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KillsTextManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI killsText;
    public int kills = 0;

    // Update is called once per frame
    void Update()
    {
        killsText.text = "Kills: " + kills.ToString();
    }
}
