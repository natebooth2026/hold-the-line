using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarManager : MonoBehaviour
{
    public static HealthBarManager instance; // Singleton instance

    public Text healthText; // Reference to the UI Text component for displaying health

    private int currentHealth = 100; // Default health value

    // Singleton pattern to ensure only one instance of HealthBarManager exists
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Method to update the health display
    public void UpdateHealth(int health)
    {
        currentHealth = health; // Update current health
        healthText.text = "Health: " + currentHealth / 100 * 100 + "%"; // Display health as a percentage for now
    }

    // Start is called before the first frame update to initialize the health display
    void Start()
    {
        UpdateHealth(currentHealth);
    }
}
