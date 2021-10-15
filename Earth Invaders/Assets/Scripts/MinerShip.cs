using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinerShip : BaseShip
{
    [Tooltip("The base amount of resource the ship gathers")]
    [SerializeField] float baseResourceOutput;

    /// <summary>
    /// Mine resources from the earth based on the given base output and multiplier
    /// </summary>
    public override void DoTask()
    {
        float totalResourceOutput = baseResourceOutput * multiplier;
        ui.UpdateResources(baseResourceOutput);
    }
}
