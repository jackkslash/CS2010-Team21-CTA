using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RoomPlans : MonoBehaviour
{
    //store the room prefabs in these relevant arrays
    public GameObject[] northRooms;
    public GameObject[] eastRooms;
    public GameObject[] southRooms;
    public GameObject[] westRooms;
    public GameObject closedRoom;
    //public GameObject floor;
    //store all the rooms generated in the order they spawn here
    public List<GameObject> rooms;

    //store level loadout information;
    public GameObject[] floors;
    public GameObject[] bosses;

    public int levelLoadOut;
    //level loadouts for floors and bosses
    //North America - 0
    //South America - 1
    //Africa        - 2
    //Antartica     - 3
    //Austrailia    - 4
    //Europe        - 5
    //Asia          - 6
    //Bonus         - 7

    //boss room variables
    //public GameObject boss;
    private bool hasSpawnedBoss = false;
    //time for boss to spawn in level (currently have input 10)
    //Contient      - waitTime (s)
    //North America - ?
    //South America - ?
    //Africa        - ?
    //Antartica     - ?
    //Austrailia    - ?
    //Europe        - ?
    //Asia          - ?
    //Bonus         - ?
    public float waitTime;
    

    //enemy types for the level store here 
    public GameObject[] enemies;
    //probabilities for enemies to spawn in level => ? in 4
    //Contient      - Chance           => int
    //North America - 1 in 4 (0.25)    => 1
    //South America - 1 in 4 (0.25)    => 1
    //Africa        - 2 in 4 (0.5)     => 2
    //Antartica     - 2 in 4 (0.5)     => 2
    //Austrailia    - 3 in 4 (0.75)    => 3
    //Europe        - 3 in 4 (0.75)    => 3
    //Asia          - 3 in 4 (0.75)    => 3
    //Bonus         - 4 in 4 (1)       => 4
    public int enemyProb;
    private int enemyProbMax = 4;


    //powerup types for the level store here
    public GameObject[] powerUps;
    //probabilities for powerup to spawn in level => ? in 8
    //Contient      - Chance            => int
    //North America - 1 in 8 (0.125)    => 1
    //South America - 1 in 8 (0.125)    => 1
    //Africa        - 2 in 8 (0.25)     => 2
    //Antartica     - 2 in 8 (0.25)     => 2
    //Austrailia    - 3 in 8 (0.375)    => 3
    //Europe        - 3 in 8 (0.375)    => 3
    //Asia          - 4 in 8 (0.5)      => 4
    //Bonus         - 8 in 8 (1)        => 8
    public int powerUpProb;
    private int powerUpProbMax = 8;

    void Start()
    {
        levelLoadOut = Random.Range(0, 8);
    }

    //spawn boss room things in this update function
    //current condition for boss to spawn is after some wait time
    void Update(){
        if(PhotonNetwork.IsMasterClient)
        {
            if(waitTime <= 0 && hasSpawnedBoss == false)
            {
                //GameObject bossInTheLastRoom = Instantiate(boss, rooms[rooms.Count-1].transform.position, Quaternion.identity);
                PhotonNetwork.Instantiate(bosses[levelLoadOut].name, rooms[rooms.Count-1].transform.position, Quaternion.identity, 0);
                hasSpawnedBoss = true;            
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
        
    }
}
