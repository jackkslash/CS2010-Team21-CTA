using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPun, IPunObservable
{
    public int score; // keep track of score (same for each player)
    public GameObject scoreTextBox; // gameobject keeping track of score
    public GameObject playerOnePrefab;
    public GameObject playerTwoPrefab;
    public GameObject gameCanvas;
    public GameObject sceneCamera;
    public GameObject enemySpawner;
    public GameObject hudCanvas;
    public GameObject roomStart; //the start room which starts the room generation
    public GameObject roomPlans; //stores all the room data needed for the room generation
   
    public GameObject roomStart1; //the start room which starts the room generation
    public GameObject roomPlans1;
    public GameObject roomStart2; //the start room which starts the room generation
    public GameObject roomPlans2;
    public GameObject waitMessage;
    public GameObject startButton;
    public GameObject startIndicatorMessage;
    private int playerNum;
    private bool player1Spawned = false;

    private void Awake(){
        gameCanvas.SetActive(true);
        // give the player a number
        playerNum = PhotonNetwork.CurrentRoom.PlayerCount;
    }

    private void Start(){
        if(playerNum == 2){
            gameObject.GetComponent<PhotonView>().RPC("SetStartIndicatorMessage", RpcTarget.AllBuffered);
        }
    }

    public void SpawnPlayer(){
        float randomValue = Random.Range(-1f, 1f);
        
        // if the player was the first in the lobby, then spawn them as charlie
        if(playerNum == 1){
            PhotonNetwork.Instantiate(playerOnePrefab.name, new Vector2(this.transform.position.x * randomValue, this.transform.position.y), Quaternion.identity, 0);
            player1Spawned = true;
            // set start button to green
            gameObject.GetComponent<PhotonView>().RPC("SetButtonColour", RpcTarget.AllBuffered);
        }else{
            if(!player1Spawned){
                waitMessage.SetActive(true);
                return;
            }
            PhotonNetwork.Instantiate(playerTwoPrefab.name, new Vector2(this.transform.position.x * randomValue, this.transform.position.y), Quaternion.identity, 0);
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }
        gameCanvas.SetActive(false);
        sceneCamera.SetActive(false);
        startIndicatorMessage.SetActive(false);
        enemySpawner.SetActive(true);
        hudCanvas.SetActive(true);
        roomPlans.SetActive(true); //roomPlans must be active before roomStart
        roomStart.SetActive(true);
        roomPlans1.SetActive(true); //roomPlans must be active before roomStart
        roomStart1.SetActive(true);
        roomPlans2.SetActive(true); //roomPlans must be active before roomStart
        roomStart2.SetActive(true);
        waitMessage.SetActive(false);
    }

    void Update(){
        scoreTextBox.GetComponent<Text>().text = "Score: "+score;
    }

    [PunRPC]
    void SetButtonColour(){
        startButton.GetComponent<Image>().color = new Color(0.41f, 0.9f, 0.34f);
    }
    [PunRPC]
    void SetStartIndicatorMessage(){
        startIndicatorMessage.SetActive(true);
    }

    [PunRPC]
    public void UpdateScore(int addScore){
        score += addScore;
    }

    public void PostScore(string name){
        StartCoroutine(gameObject.GetComponent<HSController>().PostScores(score, name));
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){
        if(stream.IsWriting){
            stream.SendNext(score);
            stream.SendNext(player1Spawned);
        } else{
            this.score = (int)stream.ReceiveNext();
            this.player1Spawned = (bool)stream.ReceiveNext();
        }
    }
}
