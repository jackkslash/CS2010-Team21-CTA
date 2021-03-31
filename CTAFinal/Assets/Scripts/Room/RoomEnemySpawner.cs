using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RoomEnemySpawner : MonoBehaviour, IPunObservable
{
    [SerializeField] GameObject[] enemyPrefabs;
    [SerializeField] GameObject spawnPoint;
    private int spawnedEnemies = 0; // SYNC // used to keep track of how many enemies have spawned in this room
    private int roomPlayerCount = 0; // SYNC // used to check if there are players in the room, if so then enemies can spawn
    private int maxEnemies = 3; // used to create a limit to how many enemies will spawn in the room
    private int enemyIndex; // used to decide which enemy will spawn
    private float timeUntilSpawn = 1; // SYNC
    private float spawnXIncrementor = 0.001f;
    private float spawnX; // SYNC // Used to make sure enemies do not spawn ontop of each other
    GameObject spawningPlayer = null; // SYNC // used to check which player entered the room first, used to make sure only one player is spawning enemies
    GameObject[][] enemies;

    void Start(){
        spawnX = spawnPoint.transform.position.x;
        EnemyTypes enemyTypes = GameObject.Find("GameManager").GetComponent<EnemyTypes>();
        enemies = new GameObject[][]
        {
            enemyTypes.soldiers,
            enemyTypes.farmers,
            enemyTypes.french
        };
    }

    void Update(){
        // spawn enemies if players are in the room
        if(spawningPlayer == null){
            return;
        }
        if(timeUntilSpawn <= 0 &&  spawningPlayer.GetComponent<PhotonView>().IsMine){
            SpawnEnemies();
            timeUntilSpawn = 2;
        }else if(spawningPlayer.GetComponent<PhotonView>().IsMine){
            timeUntilSpawn -= Time.deltaTime;
        }
    }

    void OnTriggerEnter2D(Collider2D collider){
        if (collider.tag.Equals("Player"))
        {
            if(roomPlayerCount <= 0){
                spawningPlayer = collider.gameObject;
            }
            roomPlayerCount++;
            Debug.Log("Entered room");
        }
    }

    void OnTriggerExit2D(Collider2D collider) {
        if (collider.tag.Equals("Player"))
        {
            roomPlayerCount--;
            Debug.Log("Left room");
        }
    }

    private void SpawnEnemies(){
        if(spawnedEnemies < maxEnemies && roomPlayerCount > 0){
            System.Random rand = new System.Random();
            enemyIndex = rand.Next(enemies[StaticVariables.level].Length);

            spawnedEnemies = spawnedEnemies + 1;
            spawnX+=spawnXIncrementor;
            gameObject.GetComponent<PhotonView>().RPC("SpawnEnemy", RpcTarget.All, enemies[StaticVariables.level][enemyIndex].name, new Vector3(spawnX, spawnPoint.transform.position.y, 0));
            Debug.Log("Spawned enemy");
        }
    }

    [PunRPC]
    public void SpawnEnemy(string prefabName, Vector3 pos){
        if(PhotonNetwork.IsMasterClient){
            PhotonNetwork.Instantiate(prefabName, pos, Quaternion.identity); 
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){
        if(stream.IsWriting){
            stream.SendNext(spawnedEnemies);
            stream.SendNext(roomPlayerCount);
            stream.SendNext(timeUntilSpawn);
            stream.SendNext(spawnX);
            stream.SendNext(enemyIndex);
        } else{
            this.spawnedEnemies = (int)stream.ReceiveNext();
            this.roomPlayerCount = (int)stream.ReceiveNext();
            this.timeUntilSpawn = (float)stream.ReceiveNext();
            this.spawnX = (float)stream.ReceiveNext();
            this.enemyIndex = (int)stream.ReceiveNext();
        }
    }
}