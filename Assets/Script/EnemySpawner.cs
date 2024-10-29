using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnRate;

    public float spawnMinRange;
    public float spawnMaxRange;
    public Transform baseTransform;

    public int maxSpawnCount;
    public TextMeshProUGUI enemyInfoText;



    int spawnCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(SpawnOneEnemy), 0.0f, spawnRate);
    }

    void SpawnOneEnemy()
    {
        Vector3 pos = transform.position;
        float distance = UnityEngine.Random.Range(spawnMinRange, spawnMaxRange);
        float angle = UnityEngine.Random.Range(0.0f, 2.0f * (float)Math.PI);
        pos += new Vector3((float)Math.Cos(angle), 0.0f, (float)Math.Sin(angle)) * distance;

        var obj = Instantiate(enemyPrefab, pos, Quaternion.identity);
        // obj.GetComponent<EnemyFSM>().baseTransform = baseTransform;
        obj.GetComponent<EnemyFSM>().baseTransform = transform;


        print(pos);

        if (++spawnCount >= maxSpawnCount)
        {
            CancelInvoke(nameof(SpawnOneEnemy));
        }
    }

    // Update is called once per frame
    void Update()
    {
        var enemies = FindObjectsByType<EnemyFSM>(FindObjectsSortMode.None);
        // enemies.Length;
        enemyInfoText.text = $"{(maxSpawnCount - (spawnCount - enemies.Length))} Enemies Remaining!";

        if (spawnCount == maxSpawnCount && enemies.Length == 0)
        {
            SceneManager.LoadScene("WinScene");
        }
    }
}
