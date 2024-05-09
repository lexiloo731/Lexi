/*
 * Author: Alexia Nguyen
 */

using UnityEngine;
using System.Collections;

public class ItemSpawner : MonoBehaviour
{
    public GameObject itemPrefab1;
    public GameObject itemPrefab2;
    public float spawnRadius = 5f;
    public int maxItems = 10;
    public float spawnDelay = 1f;
    float spawnYPosition = 0.4f;

    private GameObject player;

    void Start()
    {
        player = GameObject.FindWithTag("Player");

        if (player == null)
        {
            Debug.LogError("Player object not found!");
        }
        else
        {
            StartCoroutine(DelayedStart(3f));
        }
    }

    IEnumerator DelayedStart(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(SpawnItemRoutine());
    }

    void Update()
    {
        // Check if the player is no longer active
        if (player != null && !player.activeSelf)
        {
            DestroyAllItems();
        }
    }

    IEnumerator SpawnItemRoutine()
    {
        while (transform.childCount < maxItems)
        {
            SpawnItem();
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    void SpawnItem()
    {
        Vector3 randomSpawnPoint = transform.position + Random.insideUnitSphere * spawnRadius;
        randomSpawnPoint.y = spawnYPosition; // Set the y position explicitly

        GameObject itemPrefab = Random.Range(0, 2) == 0 ? itemPrefab1 : itemPrefab2;
        GameObject newItem = Instantiate(itemPrefab, randomSpawnPoint, Quaternion.identity);

        newItem.transform.parent = transform; // Set the spawner as the parent of the spawned item
    }

    void DestroyAllItems()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
