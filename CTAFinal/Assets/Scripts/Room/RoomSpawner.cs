using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

/*
Room Spawner is placed on every spawn point (which are associated to a door opening)
This script randomly selects a room to be spawned
*/
public class RoomSpawner : MonoBehaviour
{
    //opening direction of doors needed to determine the room to be selected
    //1 needs SOUTH door
    //2 needs NORTH door
    //3 needs WEST door
    //4 needs EAST door
    //
    public int openDir;

    //this gameobject stores other gameobjects such as rooms and enemy prefabs
    public RoomPlans plans;

    //to store a random value
    private int rnd;

    //a room has not spawned yet, so false
    public bool hasSpawned = false;

    //the time to wait before destroying the spawnpoint (to reduce wasted memory space)
    public float waitTime = 4f;
    private int level;
    GameObject roomPlans;


    //call this method first and only once
    void Start()
    {
        roomPlans = gameObject.transform.parent.gameObject.GetComponent<AddRoom>().roomPlans;
        level = gameObject.transform.parent.gameObject.GetComponent<AddRoom>().levelType;
        //call only on first player, aka master client
        //so rooms drawn on master client aka player 1, and communicated to other clients aka player 2 with PhotonNetwork.Instantiate
        //make take a few seconds for updated map to be communicated to player 2
        if(PhotonNetwork.IsMasterClient)
        {
            //Destroy the spawn points (with colliders and rigid bodies) after about 4 seconds
            //as they no longer needed, so reducing wasted memory space and lag, and optimising code
            Destroy(gameObject, waitTime);
            //find the RoomPlans GameObject in the current scene
            plans = roomPlans.GetComponent<RoomPlans>();
            //calls the spawn method after 0.1s delay, to give time to get the plans GameObject
            Invoke("Spawn", 0.1f);
        }
    }

    //spawn a room on this spawn point
    //in the room testing scene, comment out the PhotonNetwork.Instantiate and use the next line to check generation working as expected.
    void Spawn(){
        if(hasSpawned == false)
        {
            //spawn a floor
            //GameObject floor = Instantiate(plans.floor, transform.position, Quaternion.identity);
            GameObject newFloor = PhotonNetwork.Instantiate(plans.floors[level].name, transform.position, Quaternion.identity, 0);
            newFloor.transform.SetParent(this.gameObject.transform.parent.transform);

            //then spawn a room with a 
            if(openDir == 1){
                //SOUTH door opening
                rnd = Random.Range(0, plans.southRooms.Length);
                GameObject newRoom = PhotonNetwork.Instantiate(plans.southRooms[rnd].name, transform.position, Quaternion.identity, 0);
                newRoom.transform.SetParent(this.gameObject.transform.parent.transform);
                newRoom.GetComponent<AddRoom>().levelType = level;
                newRoom.GetComponent<AddRoom>().roomPlans = roomPlans;
                //GameObject room = Instantiate(plans.southRooms[rnd], transform.position, Quaternion.identity);
            }else if(openDir == 2){
                //NORTH door opening
                rnd = Random.Range(0, plans.northRooms.Length);
                GameObject newRoom = PhotonNetwork.Instantiate(plans.northRooms[rnd].name, transform.position, Quaternion.identity, 0);
                newRoom.transform.SetParent(this.gameObject.transform.parent.transform);
                newRoom.GetComponent<AddRoom>().levelType = level;
                newRoom.GetComponent<AddRoom>().roomPlans = roomPlans;
                //GameObject room = Instantiate(plans.northRooms[rnd], transform.position, Quaternion.identity);
            }else if(openDir == 3){
                //WEST door opening
                rnd = Random.Range(0, plans.westRooms.Length);
                GameObject newRoom = PhotonNetwork.Instantiate(plans.westRooms[rnd].name, transform.position, Quaternion.identity, 0);
                newRoom.transform.SetParent(this.gameObject.transform.parent.transform);
                newRoom.GetComponent<AddRoom>().levelType = level;
                newRoom.GetComponent<AddRoom>().roomPlans = roomPlans;
                //GameObject room = Instantiate(plans.westRooms[rnd], transform.position, Quaternion.identity);
            }else if(openDir == 4){
                //EAST door opening 
                rnd = Random.Range(0, plans.eastRooms.Length);
                GameObject newRoom = PhotonNetwork.Instantiate(plans.eastRooms[rnd].name, transform.position, Quaternion.identity, 0);
                newRoom.transform.SetParent(this.gameObject.transform.parent.transform);
                newRoom.GetComponent<AddRoom>().levelType = level;
                newRoom.GetComponent<AddRoom>().roomPlans = roomPlans;
                //GameObject room = Instantiate(plans.eastRooms[rnd], transform.position, Quaternion.identity);
            }
            hasSpawned = true;
            //SpawnEnemy();
            //SpawnPowerUp();
        }
    }

    //when a spawn point collides with another Collider2D gameobject this method is triggered
    //if the other gameobject is also a spawnpoint,
    //and that other spawnpoint has not spawned a room, and this spawnpoint has not spawned a room
    //then spawn a closed room and destroy the spawn point.
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("RoomSpawnPoint"))
        {
            RoomSpawner rs = other.gameObject.GetComponent<RoomSpawner>();
            //Debug.Log(rs);
            if(rs != null)
            {
                //other.gameObject.GetComponent<RoomSpawner>().hasSpawned == false
                if(rs.hasSpawned == false && this.hasSpawned == false)
                {
                    //spawn a filled room blocking off any openings
                    PhotonNetwork.Instantiate(plans.closedRoom.name, transform.position, Quaternion.identity, 0);
                    //GameObject room = Instantiate(plans.closedRoom, transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
                hasSpawned = true;
            }
        }
    }

    //TO DO:
    //  consider positions that enemies and powerups are spawned in
    //  implement probabilities
    
    /* not yet
    //probabilities for enemies to spawn in level => ? in 4
    void SpawnEnemy()
    {
        //spawn an enemy depending on probability
        rnd = Random.Range(0, plans.enemyProbMax);
        if(rnd < plans.enemyProb)
        {
            rnd = Random.Range(0, plans.enemies.Length);
            PhotonNetwork.Instantiate(plans.enemies[rnd].name, transform.position, Quaternion.identity, 0);
            //GameObject enemy = Instantiate(plans.enemies[rnd], transform.position, Quaternion.identity);
        }
    }
    */
    
    /* not yet
    //probabilities for powerup to spawn in level => ? in 8
    void SpawnPowerUp()
    {
        //spawn a power up depending on probability
        rnd = Random.Range(0, plans.powerUpProbMax);
        if(rnd < plans.powerUpProb)
        {
            rnd = Random.Range(0, plans.powerUps.Length);
            PhotonNetwork.Instantiate(plans.powerUps[rnd].name, transform.position, Quaternion.identity, 0);
            //GameObject powerUp = Instantiate(plans.powerUps[rnd], transform.position, Quaternion.identity);
        }
    }
    */

}