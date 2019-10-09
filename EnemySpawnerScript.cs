using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerScript : MonoBehaviour
{
    private const float Max = 8.4f;
    public GameObject  enemy;
    float randX;
    Vector2 whereTospawn;
    public float spawnRate = 2f;
    float nextSpawn = 5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextSpawn)
        {
            nextSpawn = Time.time + spawnRate;
            randX = Random.Range (-8.4f, Max);
            whereTospawn = new Vector2 (randX, transform.position.y);
            Instantiate (enemy, whereTospawn, Quaternion.identity);
        }
    }
}
