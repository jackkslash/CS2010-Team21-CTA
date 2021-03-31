using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviourPun
{


    public float speed = 10f;
    public float destroyTime = 2f;
    public Vector2 Direction;
    public int bulletDamage = 1;

    void Awake()
    {

    }

    // allows bullet to be destroyed
    void DestroyBullet()
    {
        this.GetComponent<PhotonView>().RPC("destroy", RpcTarget.All);
    }

    void Update()
    {
        // tell the bullet to move in the direction of the mouse pointer
        if (Direction != new Vector2(0, 0))
        {
            transform.Translate(Direction * Time.deltaTime * speed);
        }
    }

    // bullet detection
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag.Equals("wall") || collider.tag.Equals("enemy"))
        {
            PhotonNetwork.Destroy(this.gameObject);
        }

        if (collider.tag.Equals("enemy") && gameObject.transform.parent.GetComponent<PhotonView>().IsMine)
        {
            collider.gameObject.GetComponent<HealthSystem>().TakeDamage(bulletDamage);
        }
    }

    // set the direction of the mouse pointer
    [PunRPC]
    public void SetDirection(Vector2 direction)
    {
        this.Direction = direction;
    }

    // destroy the bullet
    [PunRPC]
    public void destroy()
    {
        PhotonNetwork.Destroy(this.gameObject);
    }
}