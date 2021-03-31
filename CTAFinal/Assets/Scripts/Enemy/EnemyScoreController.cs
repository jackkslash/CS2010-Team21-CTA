using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnemyScoreController : MonoBehaviour
{
    [SerializeField] private int hitScore = 2;
    [SerializeField] private int dieScore = 4;

    public void HitIncreaseScore(){
        GameObject.Find("GameManager").GetComponent<PhotonView>().RPC("UpdateScore", RpcTarget.AllBuffered, hitScore);
    }
    public void DeathIncreaseScore(){
        GameObject.Find("GameManager").GetComponent<PhotonView>().RPC("UpdateScore", RpcTarget.AllBuffered, dieScore);
    }
}
