using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnemyMovement : MonoBehaviour
{
    public float speed; // speed of enemy movement
    public float stoppingDistance; //distance of which the enemy stops when following player
    public float retreatDistance; // distance of which the enemy starts moving away from the player
    public float followDistance; // distance of which the enemy starts following the player
    private Transform player; // the players transform component
    private List<Transform> players = new List<Transform>(); // the players transform component
    float lastChecked = 0;
    float timeSinceTargetSelected = 0;
    
    private float lastX; 
    private float lastY; 
    private float speedXY; // calculated by checking if x or y has changed since last frame
    private float hor; // calculated by checking if x position was less or more this frame
    private float ver;  // calculated by checking if y position was less or more this frame



    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        //timeBtwShots = startTimeBtwShots;
        //source = GetComponent<AudioSource>();
    }

    void Update(){
        if(players.Count != 2){
            GetPlayers();
        }else{
            player = SelectTarget();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ChasePlayer();
        if(lastX != gameObject.transform.position.x || lastY != gameObject.transform.position.y){
            speedXY = 1;
        }else{
            speedXY = 0;
        }
        if(lastX < gameObject.transform.position.x){
            // moving right
            hor = 1;
        }else if(lastX > gameObject.transform.position.x){
            // moving left
            hor = -1;
        }
        if(lastY < gameObject.transform.position.y){
            // moving up
            ver = 1;
        }else if(lastY > gameObject.transform.position.y){
            // moving down
            ver = -1;
        }
        lastX = gameObject.transform.position.x;
        lastY = gameObject.transform.position.y;

        gameObject.GetComponent<Animator>().SetFloat("Horizontal", hor);
        gameObject.GetComponent<Animator>().SetFloat("Vertical", ver);
        gameObject.GetComponent<Animator>().SetFloat("Speed", speedXY);
    }

    public void ChasePlayer(){
        if(player == null){
            try{
                player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
            }catch(UnityException e){
                Debug.Log(e);
            }
        }
        //check if enemy is within the follow distance (currently 5.5) before chasing charlie
        if(Vector2.Distance(transform.position, player.position) < followDistance)
        {
            if(Vector2.Distance(transform.position, player.position) > stoppingDistance)
            {
                transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

            } else if(Vector2.Distance(transform.position, player.position) > stoppingDistance && Vector2.Distance(transform.position, player.position) > retreatDistance)
            {
                transform.position = this.transform.position;
            }
            else if (Vector2.Distance(transform.position, player.position) < retreatDistance)
            {
                transform.position = Vector2.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);
            }
            if(player.gameObject.GetComponent<PhotonView>().IsMine){
                gameObject.GetComponent<EnemyAttack>().AttackPlayer(player.position);
            }else{
                Debug.Log(player.gameObject.name);
            }
        }


    }
    private void GetPlayers(){

        if(lastChecked <= 0){
            GameObject[] playerGameObjects = GameObject.FindGameObjectsWithTag("Player");
            players.Clear();
            foreach(GameObject p in playerGameObjects){
                players.Add(p.transform);
            }

            lastChecked = 5;
        }else{
            lastChecked -= Time.deltaTime;
        }

    }

    private Transform SelectTarget(){
        Transform closestPlayer = player;

        if(timeSinceTargetSelected <= 0){
            float closestPlayerDistance = Vector2.Distance(transform.position, player.position);
            foreach(Transform p in players){
                float playerDistance = Vector2.Distance(transform.position, p.position);
                if(playerDistance < closestPlayerDistance){
                    closestPlayer = p;
                }
            }
            timeSinceTargetSelected = 5;
        }else{
            timeSinceTargetSelected -= Time.deltaTime;
        }

        return closestPlayer;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){
        if(stream.IsWriting){
            stream.SendNext(player);
        } else{
            this.player = (Transform)stream.ReceiveNext();
        }
    }



}
