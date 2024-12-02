using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PickupSpawningSystem : MonoBehaviour
{
    [SerializeField] List<GameObject> spawnablePickups;
    // Pickups that're more likely to spawn for any stage in particular.
    List<GameObject> pickedStagePickups;
    [SerializeField] List<GameObject> dockPickups;
    [SerializeField] List<GameObject> rooftopPickups;
    [SerializeField] float timeUntilStartSpawning;
    [SerializeField] float timeBetweenSpawns;
    List<GameObject> spawnLocationObjects;

    public static PickupSpawningSystem instance;
    private void Start()
    {
        if (instance == null) instance = this;
        else Destroy(this);
        if (PlayerDataStorage.pickedStage == PlayerDataStorage.PickedStage.Rooftop)
            pickedStagePickups = rooftopPickups;
        else if (PlayerDataStorage.pickedStage == PlayerDataStorage.PickedStage.Docks)
            pickedStagePickups = dockPickups;
        else pickedStagePickups.Clear();
    }
    public IEnumerator StartSpawnRoutine()
    {
        yield return new WaitForSeconds(timeUntilStartSpawning);
        while (true)
        {
            SpawnPickup();
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }

    public void SpawnPickup(int amount = 1)
    {
        for(int i = 0; i < amount; i++)
        {
            GameObject pickedSpawnLocation = spawnLocationObjects[Random.Range(0, spawnLocationObjects.Count)];
            Instantiate(Random.Range(1, 6) != 5 ? spawnablePickups[Random.Range(0, spawnablePickups.Count)] : pickedStagePickups[Random.Range(0, pickedStagePickups.Count)], pickedSpawnLocation.transform.position, Quaternion.identity);
            pickedSpawnLocation.GetComponentInChildren<ParticleSystem>().Play();
        }
    }
}
