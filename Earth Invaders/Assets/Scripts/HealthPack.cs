using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
    [Tooltip("The amount of health to restore to the earth")]
    [SerializeField] float healthAmount;

    /// <summary>
    /// Reference to the UI_Manager script in the scene
    /// </summary>
    private UI_Manager ui;

    // Start is called before the first frame update
    void Start()
    {
        ui = FindObjectOfType<UI_Manager>();
    }

    /// <summary>
    /// Call the UpdateHealth function and add the negative amount of healthAmount since we are adding health instead of substracting
    /// Afterwars, destory the health pack
    /// </summary>
    public void AddHealth()
    {
        ui.UpdateHealth(healthAmount * -1);
        Destroy(gameObject);
    }
}
