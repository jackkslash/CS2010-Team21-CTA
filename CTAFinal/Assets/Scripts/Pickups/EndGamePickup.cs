using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class EndGamePickup : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider){
        // check if player collides with pickup
        if(collider.tag.Equals("Player")){
            StaticVariables.score = GameObject.Find("GameManager").GetComponent<GameManager>().score;
            // submit score
            GameObject.Find("GameManager").GetComponent<GameManager>().PostScore(gameObject.GetComponent<PhotonView>().Owner.NickName);
            // load game over scene
            gameObject.GetComponent<PhotonView>().RPC("EndGame", RpcTarget.All);
            // destroy pickup
            gameObject.GetComponent<PhotonView>().RPC("DestroyPickup", RpcTarget.All);
        }
    }

    [PunRPC]
    private void DestroyPickup(){
        if(PhotonNetwork.IsMasterClient){
            PhotonNetwork.Destroy(this.gameObject);
        }
    }

    [PunRPC]
    private void EndGame(){
        StaticVariables.score = GameObject.Find("GameManager").GetComponent<GameManager>().score;
        SceneManager.LoadScene("Win");
    }
}
