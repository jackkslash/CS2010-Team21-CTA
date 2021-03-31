using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Robobullet : MonoBehaviour
{ 

    public float speed;
    private Rigidbody2D rb;
    public int robotdamage = 3;


    void DestroyBullet()
    {
        this.GetComponent<PhotonView>().RPC("destroy", RpcTarget.All);
    }


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        rb.velocity = transform.up * -speed;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag.Equals("wall") || collider.tag.Equals("Player"))
        {
            PhotonNetwork.Destroy(this.gameObject);
        }

        if (collider.tag.Equals("Player"))
        {
            collider.gameObject.GetComponent<HealthSystem>().TakeDamage(robotdamage);
        }
    }

    [PunRPC]
    public void destroy()
    {
        PhotonNetwork.Destroy(this.gameObject);
    }
}

