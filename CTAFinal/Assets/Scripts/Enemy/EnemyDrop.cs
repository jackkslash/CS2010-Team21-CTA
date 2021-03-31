using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;


public class EnemyDrop : MonoBehaviour
{
    [SerializeField] private List<GameObject> pickupPrefabs;

    [SerializeField] private int probability = 5; // needs to be between 1 and 10

    public void DropPickup(){
        System.Random rand = new System.Random(); // create random number generator
        int num = rand.Next(pickupPrefabs.Count); // generate random number
        GameObject pickup = pickupPrefabs[num]; // use random number to select a pickup

        // if statement to introduce a probability of dropping a pickup
        num = rand.Next(10); // generate number from 0 - 10
        // if number is less than probability number then drop a pickup
        if(num < probability){
            gameObject.GetComponent<PhotonView>().RPC("SpawnPickup", RpcTarget.All, pickup.name, transform.position);
            //GameObject newPickup = PhotonNetwork.Instantiate(pickup.name, transform.position, Quaternion.identity);
        }

    }
    
    [PunRPC]
    private void SpawnPickup(string prefabName, Vector3 pos){
        if(PhotonNetwork.IsMasterClient){
            PhotonNetwork.Instantiate(prefabName, pos, Quaternion.identity); 
        }
    }
}
