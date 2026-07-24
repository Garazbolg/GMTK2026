using UnityEngine;

public class LootSpawner : MonoBehaviour
{
    public float probability;
    
    public GameObject lootPrefab;
    
    public void SpawnLoot()
    {
        if (UnityEngine.Random.value < probability)
        {
            Instantiate(lootPrefab, transform.position, Quaternion.identity);
        }
    }
}