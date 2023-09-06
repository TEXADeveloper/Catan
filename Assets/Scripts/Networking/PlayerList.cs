using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using PhotonPlayer = Photon.Realtime.Player;

public class PlayerList : MonoBehaviourPunCallbacks
{
    private void updateList()
    {
        foreach (PhotonPlayer player in PhotonNetwork.PlayerList)
        {

        }
    }

    public override void OnPlayerEnteredRoom(PhotonPlayer photonPlayer)
    {
        updateList();
    }

    public override void OnPlayerLeftRoom(PhotonPlayer photonPlayer)
    {
        updateList();
    }

}
