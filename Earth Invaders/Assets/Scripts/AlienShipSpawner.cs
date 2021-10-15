using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienShipSpawner : MonoBehaviour
{
    [Tooltip("Reference to the destroyer ship prefab")]
    [SerializeField] GameObject destroyerShipRef;

    [Tooltip("Reference to the miner ship prefab")]
    [SerializeField] GameObject minerShipRef;

    [Tooltip("Reference to the health pack prefab")]
    [SerializeField] GameObject healtPackRef;

    [Tooltip("Reference to the earth gameObject")]
    [SerializeField] GameObject earth;

    [Tooltip("The rate of ship spawning per second")]
    [SerializeField] float spawnRate;

    [Tooltip("The amount of ships allowed per stack")]
    [SerializeField] float maxShipStack;

    /// <summary>
    /// List of all base or parent ships to track each stack
    /// </summary>
    List<GameObject> baseShips = new List<GameObject>();

    /// <summary>
    /// List of random numbers used to randomly go through the index of baseShips when selecting which base ship will add a new ship to the stack
    /// </summary>
    List<int> randomInts = new List<int>();

    /// <summary>
    /// int to add every time a ship is created. Used to add a number to the name of the ship to better track ships for debuggin purposes.
    /// </summary>
    int shipNum = 0;

    /// <summary>
    /// This is called at start of the game to spawn the initial 6-10 base ships as per how the game is desinged
    /// Afterwards we set up a coroutine that will handle spawning additional ships on a set spawn rate
    /// </summary>
    public void SpawnInitialShips()
    {
        int numOfShips = Random.Range(6, 11);
        for(int i = 0; i < numOfShips; i++)
        {
            if(Random.Range(0,2) == 0)
            {
                SpawnBaseShip(destroyerShipRef);
            }
            else
            {
                SpawnBaseShip(minerShipRef);
            }
        }
        StartCoroutine(WaitToSpawn());
    }

    /// <summary>
    /// A alient ship will be created as a base ship and be added to the list of base ships
    /// </summary>
    /// <param name="shipRef"></param>
    void SpawnBaseShip(GameObject shipRef)
    {
        //Random.onUnitSphere will handle placing a ship randomly around the earth and transform look at will ensure the ship will be angled in a correct manner
        GameObject ship = Instantiate(shipRef, Random.onUnitSphere, Quaternion.identity, earth.transform);
        ship.transform.LookAt(earth.transform);
        ship.name = ship.name + " " + shipNum;
        shipNum++;
        baseShips.Add(ship);
    }

    /// <summary>
    /// A random base ship will be picked to create and add a new ship to the stack if there is space
    /// </summary>
    void SpawnChildShip()
    {
        bool childSpawned = false;
        GenerateNumbers();

        //A randome number will be picked from the generated random numbers and be used as the index for which ship to pick from the list of base ships
        //If that base ship has capacity then a new ship will be created in that stack
        //If that base ship has no capacity, that index will be removed from the generated numbers and a new random number will be picked to repeat the process.
        //This process will be attempted until gone through all base ships. If no base ship was selected then a new base ship will be created.
        //Coroutine will be called again to spawn another ship in the future.
        for(int i = 0; i < baseShips.Count; i++)
        {
            int randomBaseShip = Random.Range(0, randomInts.Count);
            GameObject ship;
            if (baseShips[randomBaseShip].GetComponent<BaseShip>().GetShipCount() + 1 < maxShipStack)
            {
                ship = Instantiate(baseShips[randomBaseShip], baseShips[randomBaseShip].transform.position, Quaternion.identity, earth.transform);
                ship.transform.LookAt(earth.transform);
                ship.name = ship.name + " " + shipNum;
                shipNum++;
                ship.GetComponent<BaseShip>().SetAsChildShip(baseShips[randomBaseShip].GetComponent<BaseShip>());
                childSpawned = true;
                break;
            }
            randomInts.Remove(randomBaseShip);
        }
        if(!childSpawned)
        {
            if (Random.Range(0, 10) % 2 == 0)
            {
                SpawnBaseShip(destroyerShipRef);
            }
            else
            {
                SpawnBaseShip(minerShipRef);
            }
        }
        StartCoroutine(WaitToSpawn());
    }

    /// <summary>
    /// Generate a list of random numbers that will be used to pick base ships when trying to add a new ship to an existing stack
    /// </summary>
    void GenerateNumbers()
    {
        randomInts.Clear();
        for(int i = 0; i < baseShips.Count; i++)
        {
            randomInts.Add(i);
        }
    }

    /// <summary>
    /// Couroutine used to have ship spawning done at a set rate
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitToSpawn()
    {
        yield return new WaitForSeconds(spawnRate);
        SpawnChildShip();
    }
    
    /// <summary>
    /// When the game is over, destroy all the ships and clear the list of base ships
    /// </summary>
    public void Stop()
    {
        StopAllCoroutines();
        foreach (BaseShip ship in FindObjectsOfType<BaseShip>())
        {
            Destroy(ship.gameObject);
        }
        baseShips.Clear();
    }

    /// <summary>
    /// When a base ship has been destroyed, update it with a given child from the stack or remove the it from the list
    /// </summary>
    /// <param name="oldParent"></param>
    /// <param name="newParent"></param>
    public void UpdateBaseShipList(BaseShip oldParent, BaseShip newParent)
    {
        for(int i = 0; i< baseShips.Count; i++)
        {
            if (baseShips[i].gameObject == oldParent.gameObject)
            {
                if(newParent)
                {
                    baseShips[i] = newParent.gameObject;

                }
                else
                {
                    baseShips.Remove(baseShips[i]);
                }
                break;
            }
            
        }
    }

    /// <summary>
    /// Spawn a health pack item placed randomly on the earth
    /// Called when a base ship with no children has been destroyed
    /// </summary>
    public void SpawnHealthPack()
    {
        Instantiate(healtPackRef, Random.onUnitSphere, Quaternion.identity, earth.transform);
    }
}
