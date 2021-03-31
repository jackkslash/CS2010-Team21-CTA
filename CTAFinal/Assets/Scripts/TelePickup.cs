using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TelePickup : MonoBehaviour
{

    public Transform target;
    private GameObject[] players;

    //public GameObject player;

    void OnTriggerEnter2D(Collider2D collider){
        // check if player collides with pickup
        if(collider.tag.Equals("Player")){

            if(collider.gameObject.GetComponent<PhotonView>().IsMine){
                gameObject.GetComponent<PhotonView>().RPC("TeleportPlayers", RpcTarget.AllBuffered);
            }
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
    private void TeleportPlayers(){
        foreach(GameObject p in GameObject.FindGameObjectsWithTag("Player")){
            if(p.GetComponent<PhotonView>().IsMine){
                Debug.Log("Leaving level: "+StaticVariables.level);
                StaticVariables.level++;
                Debug.Log("Joining level: "+StaticVariables.level);
                p.transform.position = target.transform.position;
            }
        }
    }
}
