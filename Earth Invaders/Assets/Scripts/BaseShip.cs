using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is the parent class of the different types of alien ships and will contain all the shared functionality between the diffent ships
public abstract class BaseShip : MonoBehaviour
{
    /// <summary>
    /// Reference to the UI_Manager script in the scene
    /// </summary>
    protected UI_Manager ui;

    /// <summary>
    /// Reference to the parent/base ship of the stack
    /// </summary>
    protected BaseShip parentShip;

    /// <summary>
    /// Reference to the AlienShipSpawner script in the scene
    /// </summary>
    protected AlienShipSpawner spawner;

    /// <summary>
    /// List of ships that belong in the stack
    /// </summary>
    protected List<BaseShip> childShips;

    /// <summary>
    /// How often does the ship perform its task
    /// </summary>
    protected float taskCooldown;

    /// <summary>
    /// Multiplier to add to the amount of value that the parent ship is performing on the task based on stack size
    /// </summary>
    protected float multiplier;

    /// <summary>
    /// Set initial values
    /// Ship will initially set itself as the parent but can be changed if the SetAsChildShip function is called
    /// CoolDown coroutine will start which will handle calling the DoTask function based on the taskCooldown
    /// </summary>
    void Awake()
    {
        multiplier = 1;
        taskCooldown = 1.0f;
        spawner = FindObjectOfType<AlienShipSpawner>();
        ui = FindObjectOfType<UI_Manager>();
        childShips = new List<BaseShip>();
        parentShip = this;

        StartCoroutine(CoolDown());
    }

    /// <summary>
    /// Call DoTask which its functionality will be different based on the ship type and then setup a cooldown again
    /// </summary>
    /// <returns></returns>
    public IEnumerator CoolDown()
    {
        yield return new WaitForSeconds(taskCooldown);
        DoTask();
        StartCoroutine(CoolDown());
    }

    /// <summary>
    /// Get the amount of ships in the stack
    /// </summary>
    /// <returns></returns>
    public int GetShipCount()
    {
        return childShips.Count;
    }

    /// <summary>
    /// Set the this ship as the child of the passed parent, add the ship to the paren't stack, add the parent's multiplier, and place the ship nearby the parent but not on top.
    /// Positions will be either an offset on the x or y depending on the list size
    /// </summary>
    /// <param name="parent"></param>
    public void SetAsChildShip(BaseShip parent)
    {
        float positionOffset = 0.05f;
        parentShip = parent;
        parentShip.childShips.Add(this);
        parentShip.multiplier += 0.5f;
        if (childShips.Count % 2 == 0)
        { 
            transform.Translate(Vector3.right * positionOffset);
        }
        else
        {
            transform.Translate(Vector3.up * positionOffset);
        }
    }

    /// <summary>
    /// Before destroying the ship, check if the ship is a parent or not and perform some setups to avoid errors
    /// </summary>
    public void DestroyShip()
    {
        //If the ships is a parent then check if it has children in the stack in order to assign a child ship as the new parent
        //If it has no children then spawn a health pack and destroy the 
        //If the ship is not a parent then remove itself from the parent's stack and decrease its multiplier
        if(parentShip == this)
        {
            if(childShips.Count > 0)
            {
                //Grab the first child ship on the list and assign it and the new parent
                //Pass in values to the new parent such as the children ships of the old parent and its multiplier
                //Let all the child ships know of the new parent, and update the baseShip list in the spawner script
                BaseShip newParent = childShips[0];
                multiplier -= 0.5f;
                newParent.parentShip = newParent;
                newParent.childShips = childShips;
                newParent.childShips.Remove(newParent);
                newParent.multiplier = multiplier;
                for (int i = 0; i < childShips.Count; i++)
                {
                    childShips[i].parentShip = newParent;
                }
                spawner.UpdateBaseShipList(this, newParent);
            }
            else
            {
                //Update the list of baseShips from the spawner script and spawn a health pack
                spawner.UpdateBaseShipList(this, null);
                spawner.SpawnHealthPack();
            }

        }
        else
        {
            parentShip.multiplier -= 0.5f;
            parentShip.childShips.Remove(this);
        }

        ui.AddShipDestroyed();
        Destroy(gameObject);
    }

    /// <summary>
    /// DoTask will be overriden and will perform a task based on the child's class design
    /// </summary>
    public abstract void DoTask();

}
