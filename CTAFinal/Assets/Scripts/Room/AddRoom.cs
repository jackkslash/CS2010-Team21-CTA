using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
AddRoom is on every room prefab and will append this room gameobject to a list storing all the rooms in the randomly generated map
*/
public class AddRoom : MonoBehaviour
{
    public GameObject roomPlans;
    public int levelType;

    void Start(){
        //plans = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomPlans>();
        roomPlans.GetComponent<RoomPlans>().rooms.Add(this.gameObject);
    }
}
