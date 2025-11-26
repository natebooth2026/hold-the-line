using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class UpgradeToggle : MonoBehaviour
{
    public bool activeUpgradeMenu = false;
    [SerializeField] GameObject firstPersonCam;
    [SerializeField] GameObject birdseyeCam;
    [SerializeField] GameObject upgradeMenu;
    [SerializeField] ManualTurret lGun;
    [SerializeField] ManualTurret rGun;
    [SerializeField] EnemySpawner trackerScript;
    private bool canToggleBack = true;

    void ToggleMouse(bool l)
    {
        if (l)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        } else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void Awake()
    {
        ToggleMouse(true);
    }

    private IEnumerator toggleCooldown()
    {
        canToggleBack = false;
        yield return new WaitForSeconds(1f);
        canToggleBack = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U) && !lGun.isShooting && !rGun.isShooting 
        && (!trackerScript.betweenWave || (trackerScript.betweenWave && canToggleBack)))
        {
            StartCoroutine(toggleCooldown());
            activeUpgradeMenu = !activeUpgradeMenu;
            if (!activeUpgradeMenu)
            {
                ToggleMouse(true);
                birdseyeCam.SetActive(false);
                upgradeMenu.SetActive(false);
                firstPersonCam.SetActive(true);
            } else
            {
                firstPersonCam.SetActive(false);
                birdseyeCam.SetActive(true);
                upgradeMenu.SetActive(true);
                ToggleMouse(false);
            }
        }  
    }
}
