using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HealthPickup : MonoBehaviour
{
    public int healthToAdd = 2;
    
    void OnTriggerEnter2D(Collider2D collider){
        HealthSystem playerHS = collider.GetComponent<HealthSystem>();
        // check if player collides with pickup
        if(collider.tag.Equals("Player") && playerHS.CurrentHealth < playerHS.MaxHealth){

            if(collider.GetComponent<PhotonView>().IsMine){


                // increase health
                playerHS.IncrementHealth(healthToAdd);

                // remove the gameobject
                //PhotonNetwork.Destroy(this.gameObject);
                gameObject.GetComponent<PhotonView>().RPC("DestroyPickup", RpcTarget.All);
            
            }
        }
    }

    [PunRPC]
    private void DestroyPickup(){
        if(PhotonNetwork.IsMasterClient){
            PhotonNetwork.Destroy(this.gameObject);
        }
    }
}
 