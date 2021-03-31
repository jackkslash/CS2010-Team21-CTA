using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnemyAttack : MonoBehaviour
{

    private float timeBtwShots; // keep track of how long it is until the enemy attacks
    private float startTimeBtwShots = 2; // stores the amount of time an enemy has to wait before attacking
    public GameObject projectile; // stores prefab of enemy projectile

    // Start is called before the first frame update
    void Start()
    {
        timeBtwShots = startTimeBtwShots;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void AttackPlayer(Vector2 playerDirection){
        if(timeBtwShots <= 0){
            GameObject newProjectile = PhotonNetwork.Instantiate(projectile.name, transform.position, Quaternion.identity);
            newProjectile.GetComponent<PhotonView>().RPC("SetDirection", RpcTarget.All, playerDirection);
            timeBtwShots = startTimeBtwShots;
        }else{
            timeBtwShots -= Time.deltaTime;
        }
    }
}
