using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnRate;

    public float spawnRange;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(SpawnOneEnemy), 0.0f, spawnRate);
    }

    void SpawnOneEnemy()
    {
        Vector3 pos = transform.position;
        pos.x += Random.Range(0.0f, spawnRange);
        pos.z += Random.Range(0.0f, spawnRange);

        Instantiate(enemyPrefab, pos, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
