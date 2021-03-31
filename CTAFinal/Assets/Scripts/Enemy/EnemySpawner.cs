using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;

    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private float spawnInterval = 1.0f;
    [SerializeField] private int maxSpawnedEnemies;
    [SerializeField] private int enemiesUntilBossSpawnsInput;
    private int enemiesUntilBossSpawns;
    private bool bossSpawned;
 
    private void Awake(){
        Debug.Log("Enemy spawner started");
        //SpawnEnemy();
        //enemiesUntilBossSpawns = enemiesUntilBossSpawnsInput;
        //InvokeRepeating("SpawnHandler", this.spawnInterval, this.spawnInterval);

    }

void SpawnHandler(){
        if(GameObject.FindGameObjectsWithTag("enemy").Length < maxSpawnedEnemies && GameObject.FindGameObjectsWithTag("Player").Length > 0 && enemiesUntilBossSpawns>0){
            SpawnEnemy();
            enemiesUntilBossSpawns--;
        }
        // spawns boss if all enemies are defeated and the boss hasnt already spawned
        else if(enemiesUntilBossSpawns == 0 && GameObject.FindGameObjectsWithTag("Enemy").Length == 0 && !bossSpawned){
            // spawn boss
            SpawnBoss();
            bossSpawned = !bossSpawned;
        }
    }

    void SpawnEnemy(){
        float randomValue = Random.Range(-1f, 1f);
        PhotonNetwork.Instantiate(enemyPrefab.name, new Vector2(this.transform.position.x * randomValue, this.transform.position.y), Quaternion.identity, 0);
    }

     void SpawnBoss(){
       Debug.Log("Boss Spawned");
    }
}
