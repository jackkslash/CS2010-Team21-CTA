using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class MenuController : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update

    [SerializeField] private GameObject UsernameMenu;
    [SerializeField] private GameObject ConnectPanel;

    [SerializeField] private InputField UsernameInput;
    [SerializeField] private InputField CreateGameInput;
    [SerializeField] private InputField JoinGameInput;

    [SerializeField] private GameObject StartButton;


    private void Awake(){
        PhotonNetwork.ConnectUsingSettings();
    }

    private void Start(){
        UsernameMenu.SetActive(true);
    }

    override
    public void OnConnectedToMaster(){
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    public void ChangeUsernameInput(){
        if(UsernameInput.text.Length >= 3){
            StartButton.SetActive(true);
        }else{
            StartButton.SetActive(false);
        }
    }

    public void SetUsername(){
        UsernameMenu.SetActive(false);
        PhotonNetwork.NickName = UsernameInput.text;
    }

    public void CreateGame(){
        PhotonNetwork.CreateRoom(CreateGameInput.text, new RoomOptions() {MaxPlayers = 2}, null);
    }

    public void JoinGame(){
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        PhotonNetwork.JoinOrCreateRoom(JoinGameInput.text, roomOptions, TypedLobby.Default);
    }

    override
    public void OnJoinedRoom(){
        PhotonNetwork.LoadLevel("MainGame");
    }

    public void DisconnectPlayer(){
        StartCoroutine(DisconnectAndLoad());
    }
    IEnumerator DisconnectAndLoad(){
        PhotonNetwork.Disconnect();
        while(PhotonNetwork.IsConnected)
            yield return null;
        SceneManager.LoadScene("StartMenu");
    }

    public void LoadLeaderboardScene(){
        SceneManager.LoadScene("Leaderboard");
    }





}
