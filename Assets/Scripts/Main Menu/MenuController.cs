using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class MenuController : MonoBehaviour
{
    public static event Action<int, string, RoomActions> WrongName;
    Player p;
    private string roomName;

    void Start()
    {
        p = new Player();
        ConnectionManager.EnterRoom += playGame;
    }

    public void UpdatePlayerName(string value)
    {
        p.Name = value;
    }

    public void playGame()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void UpdateRoomName(string value)
    {
        roomName = value;
    }

    public void CreateRoom()
    {
        if (roomName == null || roomName == "")
        {
            WrongName?.Invoke(0, "Room name can't be blank", RoomActions.Create);
            return;
        }
        if (p.Name == null || p.Name == "")
        {
            WrongName?.Invoke(1, "You must choose a name", RoomActions.Create);
            return;
        }
        ConnectionManager.CM.CreateRoom(roomName, p);
    }

    public void JoinRoom()
    {
        if (roomName == null || roomName == "")
        {
            WrongName?.Invoke(0, "Room name can't be blank", RoomActions.Join);
            return;
        }
        if (p.Name == null || p.Name == "")
        {
            WrongName?.Invoke(1, "You must choose a name", RoomActions.Join);
            return;
        }
        ConnectionManager.CM.JoinRoom(roomName, p);
    }

    public void QuitGame()
    {
        Application.Quit(0);
    }

    void OnDestroy()
    {
        ConnectionManager.EnterRoom -= playGame;
    }
}
