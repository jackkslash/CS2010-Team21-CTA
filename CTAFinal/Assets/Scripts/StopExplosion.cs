using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class StopExplosion : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<PhotonView>().RPC("DestroyExplosion", RpcTarget.All);
    }

    [PunRPC]
    private void DestroyExplosion(){
        Destroy(gameObject, 3f);
    }

}
