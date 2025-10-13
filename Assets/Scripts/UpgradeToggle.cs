using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeToggle : MonoBehaviour
{
    public bool activeUpgradeMenu = false;
    [SerializeField] GameObject firstPersonCam;
    [SerializeField] GameObject birdseyeCam;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            activeUpgradeMenu = !activeUpgradeMenu;
            if (!activeUpgradeMenu)
            {
                birdseyeCam.SetActive(false);
                firstPersonCam.SetActive(true);
            } else
            {
                firstPersonCam.SetActive(false);
                birdseyeCam.SetActive(true);
            }
        }  
    }
}
