using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Projectile : MonoBehaviour
{
    Vector3 playerDirection;
    Vector3 playerDirectionNormalised;
    [SerializeField] int speed = 2;
    [SerializeField] int projectileDamage = 2;
    [SerializeField] private float timeUntilDelete = 5f;
    [SerializeField] private GameObject explosion;
    // Start is called before the first frame update
    void Start()
    {
        playerDirectionNormalised = (playerDirection - transform.position).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        if(timeUntilDelete <= 0){
            PhotonNetwork.Destroy(this.gameObject);
        }else{
            timeUntilDelete -= Time.deltaTime;
        }
        transform.position += playerDirectionNormalised * speed * Time.deltaTime;
    } 

        // bullet detection
    void OnTriggerEnter2D(Collider2D collider){
        if (collider.tag.Equals("wall") || collider.tag.Equals("Player"))
        {
            if(explosion != null){
                gameObject.GetComponent<PhotonView>().RPC("SpawnExplosion", RpcTarget.All, explosion.name, gameObject.transform.position);
            }
            PhotonNetwork.Destroy(this.gameObject);
        }

        if(collider.tag.Equals("Player") && collider.GetComponent<PhotonView>().IsMine){
            collider.gameObject.GetComponent<HealthSystem>().TakeDamage(projectileDamage);
        }
    }

    [PunRPC]
    public void SetDirection(Vector2 direction){
        this.playerDirection = direction; 
    }
    [PunRPC]
    public void SpawnExplosion(string prefabName, Vector3 spawnPosition){
        if(PhotonNetwork.IsMasterClient){
            PhotonNetwork.Instantiate(prefabName, spawnPosition, Quaternion.identity);
        }
    }
}
