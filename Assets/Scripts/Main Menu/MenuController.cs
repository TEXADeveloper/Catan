using UnityEngine;
using System;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject listPanel;
    public static event Action<int, string, RoomActions> WrongName;
    Player p;
    private string roomName;

    void Start()
    {
        ConnectionManager.ConnectionState += connected;

        if (ConnectionManager.CM.IsConnected)
        {
            ConnectionManager.CM.SendConnectionState("Connected", Color.green);
            p = ConnectionManager.CM.GetPlayer();
        }
        else
            p = new Player();
    }

    public void UpdatePlayerName(string value)
    {
        p.Name = value;
    }

    public void UpdateRoomName(string value)
    {
        roomName = value;
    }

    public void ConnectToServer()
    {
        if (p.Name == null || p.Name == "")
        {
            WrongName?.Invoke(1, "You must choose a name", RoomActions.Connect);
            return;
        }
        ConnectionManager.CM.StartConnection(p);
    }

    private void connected(string value, Color c)
    {
        if (value == "Connected")
        {
            mainPanel.SetActive(true);
            listPanel.SetActive(false);
        }
    }

    public void CreateRoom()
    {
        if (roomName == null || roomName == "")
        {
            WrongName?.Invoke(0, "Room name can't be blank", RoomActions.Create);
            return;
        }

        ConnectionManager.CM.CreateRoom(roomName);
    }

    public void JoinRoom()
    {
        if (roomName == null || roomName == "")
        {
            WrongName?.Invoke(0, "Room name can't be blank", RoomActions.Join);
            return;
        }

        ConnectionManager.CM.JoinRoom(roomName);
    }

    public void QuitGame()
    {
        Application.Quit(0);
    }

    void OnDisable()
    {
        ConnectionManager.ConnectionState -= connected;
    }
}
