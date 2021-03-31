using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Destroyer : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other){
        //destory all gameobjects colliding with the start room spawn point which havea wall tag
        //should now not destroy other gameobjects that collide with that spawn point, ie the players and enemies
        if(other.CompareTag("wall")){
            Destroy(other.gameObject);
        }
    }
}
