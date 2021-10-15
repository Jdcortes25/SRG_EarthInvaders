using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    [Tooltip("Damage amount the cannon ball will do to the earth")]
    [SerializeField] float damage;

    /// <summary>
    /// Reference to the UI_Manager script in the scene
    /// </summary>
    UI_Manager ui;

    // Start is called before the first frame update
    void Awake()
    {
        ui = FindObjectOfType<UI_Manager>();
    }

    /// <summary>
    /// Check what the cannonball hit based on tags, perform task and then destroy the cannonball
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Earth":
                //Do damange to the earth
                ui.UpdateHealth(damage);
                break;
            case "AlienShip":
                //Destroy the hit ship
                collision.gameObject.GetComponent<BaseShip>().DestroyShip();
                break;
            case "HealthPack":
                //Add health and destroy health pack
                collision.gameObject.GetComponent<HealthPack>().AddHealth();
                break;
            default:
                break;
        }
        Destroy(gameObject);
    }
}
