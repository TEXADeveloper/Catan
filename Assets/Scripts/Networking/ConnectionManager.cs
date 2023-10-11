using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using PhotonPlayer = Photon.Realtime.Player;


public class ConnectionManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameData gameData;
    public static ConnectionManager CM;
    public static event Action<string, Color> ConnectionState;
    public static event Action<int, string, RoomActions> RoomError;
    public static event Action<PhotonPlayer[]> UpdateList;

    [HideInInspector] public bool IsConnected = false;
    Player player;

    public Player GetPlayer()
    {
        return player;
    }

    //! //FIXME: Error with singleton (both objects destroyed when going back to scene 0)
    void Awake()
    {
        if (CM == null)
            CM = this;
        else
            Destroy(this.gameObject);
        DontDestroyOnLoad(CM);
    }

    void Start()
    {
        PhotonNetwork.GameVersion = Application.version;
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void StartConnection(Player p)
    {
        player = p;
        PhotonNetwork.AuthValues = new AuthenticationValues();
        PhotonNetwork.AuthValues.UserId = player.Name;
        PhotonNetwork.ConnectUsingSettings();
        SendConnectionState("Connecting", Color.yellow);
    }

    public override void OnConnectedToMaster()
    {
        IsConnected = true;
        SendConnectionState("Connected", Color.green);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        IsConnected = true;
        SendConnectionState("Disconnected", Color.red);
    }

    public void SendConnectionState(string state, Color color)
    {
        ConnectionState?.Invoke(state, color);
    }

    public void CreateRoom(string roomName)
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 4;
        options.PlayerTtl = 60000;
        options.EmptyRoomTtl = 180000;
        options.PublishUserId = true;
        PhotonNetwork.CreateRoom(roomName, options, TypedLobby.Default);
    }

    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
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
        foreach (PhotonPlayer p in PhotonNetwork.PlayerList)
            gameData.AddPlayer(new Player(p.UserId));

        StartCoroutine(showList());
    }

    public override void OnPlayerEnteredRoom(PhotonPlayer photonPlayer)
    {
        gameData.AddPlayer(new Player(photonPlayer.UserId));
        StartCoroutine(showList());
    }

    public override void OnPlayerLeftRoom(PhotonPlayer photonPlayer)
    {
        gameData.RemovePlayer(photonPlayer.UserId);
        StartCoroutine(showList());
    }

    private IEnumerator showList()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
            yield return new WaitUntil(() => operation.isDone);
        }
        UpdateList?.Invoke(PhotonNetwork.PlayerList);
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
    }

}

public enum RoomActions
{
    Connect,
    Create,
    Join
}
