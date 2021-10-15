using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyerShip : BaseShip
{
    [Tooltip("The base damage the ship does to earth")]
    [SerializeField] float baseDamage;

    /// <summary>
    /// Do damage to the earth's health based on the given base damage and multiplier
    /// </summary>
    public override void DoTask()
    {
        float totalDamage = baseDamage * multiplier;
        ui.UpdateHealth(totalDamage);
    }
}
