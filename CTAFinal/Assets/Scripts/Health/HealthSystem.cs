using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;

public class HealthSystem : MonoBehaviour, IPunObservable
{
    [SerializeField] protected int maxHealth;
    protected int currentHealth;
    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (currentHealth <= 0)
        {
            Death();
        }
    }

    public void TakeDamage(int damage)
    {

        /*if(currentHealth > 0){
            currentHealth -= damage;
            Debug.Log("Current health: "+currentHealth);
        }*/
        gameObject.GetComponent<PhotonView>().RPC("DecrementHealth", RpcTarget.AllBuffered, damage);
        if(currentHealth > 0 && gameObject.tag.Equals("enemy")){
            gameObject.GetComponent<EnemyScoreController>().HitIncreaseScore();
        }
        /*
        if(gameObject.tag == "Enemy" || gameObject.tag == "EnemyBoss"){
            gameObject.GetComponent<GiveScore>().EnemyGivePlayerScore(false);
        }*/
    }

    [PunRPC]
    private void DecrementHealth(int damage)
    {
        currentHealth -= damage;
        //Debug.Log("Current health: " + currentHealth);
    }


    public void IncrementHealth(int health){
        AddHealth(health);
    }

    public void AddHealth(int health)
    {
        if (currentHealth + health <= maxHealth){
            currentHealth += health;
        }else {
            currentHealth = maxHealth;
        }
    }

    protected void Death()
    {
        if(gameObject.tag.Equals("enemy")){
            gameObject.GetComponent<EnemyScoreController>().DeathIncreaseScore();
            gameObject.GetComponent<EnemyDrop>().DropPickup(); 
        }else if(gameObject.tag.Equals("Player")){
            GameObject.Find("GameManager").GetComponent<GameManager>().PostScore(gameObject.GetComponent<PhotonView>().Owner.NickName);
            gameObject.GetComponent<PhotonView>().RPC("LoadGameOver", RpcTarget.All);
        }
        gameObject.GetComponent<PhotonView>().RPC("DestroyGO", RpcTarget.All);
        
        
    }

    [PunRPC]
    private void DestroyGO(){
        if(PhotonNetwork.IsMasterClient){
            PhotonNetwork.Destroy(gameObject);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){
        if(stream.IsWriting){
            stream.SendNext(currentHealth);
        } else{
            this.currentHealth = (int)stream.ReceiveNext();
        }
    }

    [PunRPC]
    private void LoadGameOver(){
        StaticVariables.score = GameObject.Find("GameManager").GetComponent<GameManager>().score;
        SceneManager.LoadScene("GameOver");
    }

    // Getter / setters
    public int CurrentHealth
    {
        get { return currentHealth; }
        set { currentHealth = value; }
    }

    public int MaxHealth
    {
        get { return maxHealth; }
        set { maxHealth = value; }

    }
}
