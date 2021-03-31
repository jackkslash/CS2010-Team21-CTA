using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;

public class HealthBar : MonoBehaviour, IPunObservable
{
    // Start is called before the first frame update
    public Slider slider;
    int health;
    int currentHealth;
    float normalisedHealth;
    Transform bar;
    Color enemyColor;
    Color playerColor;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        enemyColor = new Color(0.88f, 0.13f, 0.13f);
        playerColor = new Color(0.07f, 0.68f, 0.19f);
        
        Debug.Log(gameObject.transform.parent.name);
        health = gameObject.transform.parent.gameObject.transform.parent.GetComponent<HealthSystem>().MaxHealth;
        currentHealth = gameObject.transform.parent.gameObject.transform.parent.GetComponent<HealthSystem>().CurrentHealth;
        slider.maxValue = health;
        SetBarColour();
        SetHealthBar();
    }

    // Update is called once per frame
    void Update()
    {
        SetHealthBar();
    }
    void UpdateHealthValues(){
        health = gameObject.transform.parent.gameObject.transform.parent.GetComponent<HealthSystem>().MaxHealth;
        currentHealth = gameObject.transform.parent.gameObject.transform.parent.GetComponent<HealthSystem>().CurrentHealth;
    }
    void SetBarColour(){
        if(player.tag.Equals("Player")){
            gameObject.transform.GetChild(1).GetComponent<Image>().color = playerColor;
        }
    }
    void SetHealthBar(){
        UpdateHealthValues();
        UpdateHealthBar();
    }

    void UpdateHealthBar(){
        slider.value = currentHealth;
    }

    
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){
        if(stream.IsWriting){
            stream.SendNext(slider.value);
        } else{
            slider.value = (float)stream.ReceiveNext();
        }
    }


}
