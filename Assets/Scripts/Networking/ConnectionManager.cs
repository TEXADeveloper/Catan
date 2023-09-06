using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using PhotonPlayer = Photon.Realtime.Player;
using System.Collections.Generic;


public class ConnectionManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameData gameData;
    public static ConnectionManager CM;
    public static event Action<string, Color> ConnectionState;
    public static event Action<int, string, RoomActions> RoomError;
    public static event Action EnterRoom;

    Player player;

    public Player GetPlayer()
    {
        return player;
    }

    void Awake()
    {
        if (CM == null)
            CM = this;
        else
            Destroy(this);
        DontDestroyOnLoad(CM);
    }

    void Start()
    {
        PhotonNetwork.GameVersion = Application.version;
        PhotonNetwork.AutomaticallySyncScene = true;
        startConnection();
    }

    private void startConnection()
    {
        PhotonNetwork.ConnectUsingSettings();
        ConnectionState?.Invoke("Connecting", Color.yellow);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected");
        ConnectionState?.Invoke("Connected", Color.green);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogError("Desconectado debido a " + cause);
        ConnectionState?.Invoke("Disconnected", Color.red);
    }

    public void CreateRoom(string roomName, Player p)
    {
        player = p;

        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 4;
        options.PlayerTtl = 60000;
        options.EmptyRoomTtl = 180000;
        PhotonNetwork.CreateRoom(roomName, options, TypedLobby.Default);
    }

    public void JoinRoom(string roomName, Player p)
    {
        player = p;

        PhotonNetwork.JoinRoom(roomName);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        RoomError?.Invoke(returnCode, message, RoomActions.Create);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        RoomError?.Invoke(returnCode, message, RoomActions.Join);
    }

    public override void OnJoinedRoom()
    {
        EnterRoom?.Invoke();
        Debug.Log("se unió a la sala");

        gameData.AddPlayer(player);

        PhotonView PV = GetComponent<PhotonView>();
        PV.RPC("addPlayer", RpcTarget.Others, player);
    }

    public override void OnPlayerLeftRoom(PhotonPlayer photonPlayer)
    {
        //Eliminarlo de la lista
    }

    [PunRPC]
    private void addPlayer(Player p)
    {
        gameData.AddPlayer(p);

        if (!PhotonNetwork.LocalPlayer.IsMasterClient)
            return;

        PhotonView PV = GetComponent<PhotonView>();
        PV.RPC("setList", RpcTarget.Others, gameData.Players);
    }

    [PunRPC]
    private void setList(List<Player> playerList)
    {
        gameData.Players = playerList;
    }
}

public enum RoomActions
{
    Create,
    Join
}
