using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [Tooltip("Prefab reference to the cannon ball")]
    [SerializeField] GameObject cannonBallRef;

    [Tooltip("The amount of force to add to the cannon ball when is shot")]
    [SerializeField] float cannonForce;

    /// <summary>
    /// Create a cannon ball and add force to it towards earth
    /// </summary>
    public void FireCannon()
    {
        GameObject cannonBall = Instantiate(cannonBallRef, transform);
        cannonBall.GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, cannonForce), ForceMode.Force);
    }
}
