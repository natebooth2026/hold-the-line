using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
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
    [SerializeField] GameObject[] HUD = new GameObject[3];
    private bool canToggleBack = true;
    private GameObject sfx;
    private AudioSource[] sfxCollection;
    private const int CLICK_SOUND = 2;

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
        sfx = GameObject.Find("SFX_SOURCE");
        sfxCollection = sfx.GetComponents<AudioSource>();
    }

    void HandleUpgradeMenu()
    {
        lGun.isShooting = false;
        rGun.isShooting = false;
        sfxCollection[CLICK_SOUND].Play();
        if (!activeUpgradeMenu)
            {
                ToggleMouse(true);
                birdseyeCam.SetActive(false);
                upgradeMenu.SetActive(false);
                firstPersonCam.SetActive(true);
                for(int i = 0; i < 3; ++i) HUD[i].SetActive(true);
            } else
            {
                for(int i = 0; i < 3; ++i) HUD[i].SetActive(false);
                firstPersonCam.SetActive(false);
                birdseyeCam.SetActive(true);
                upgradeMenu.SetActive(true);
                ToggleMouse(false);
            }
    }

    private IEnumerator toggleCooldown()
    {
        if(canToggleBack) {
            canToggleBack = false;
            activeUpgradeMenu = !activeUpgradeMenu;
            HandleUpgradeMenu();
            yield return new WaitForSeconds(1f);
            canToggleBack = true;
            if(!activeUpgradeMenu) lGun.shoot = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(trackerScript == null)
        {
            Debug.LogWarning("UpgradeToggle: One or more required references are missing!");
            return;
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            StartCoroutine(toggleCooldown());
        }  
    }
}
