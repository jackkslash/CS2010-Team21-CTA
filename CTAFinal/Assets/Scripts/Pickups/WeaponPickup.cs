using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WeaponPickup : MonoBehaviour
{
    public int spriteIndex;
    public GameObject bulletPrefab;
    public int maxAmmo;
    public bool isGunAutomatic;
    public float shootDelay;
    public bool isGunBurst;
    public int burstAmount;


    void OnTriggerEnter2D(Collider2D collider){
        // check if player collides with pickup
        if(collider.tag.Equals("Player")){
            // destroy pickup
            collider.GetComponent<Player>().SetWeapon(spriteIndex, bulletPrefab.name, maxAmmo, shootDelay, burstAmount, isGunAutomatic, isGunBurst);
            gameObject.GetComponent<PhotonView>().RPC("DestroyPickup", RpcTarget.All);
        }
    }

    [PunRPC]
    private void DestroyPickup(){
        if(PhotonNetwork.IsMasterClient){
            PhotonNetwork.Destroy(this.gameObject);
        }
    }
}
