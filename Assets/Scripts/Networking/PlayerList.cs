using TMPro;
using UnityEngine;
using System.Collections.Generic;
using Photon.Realtime;
using PhotonPlayer = Photon.Realtime.Player;
using Photon.Pun;

public class PlayerList : MonoBehaviour
{
    [SerializeField] private GameObject playerDisplay;
    [SerializeField] private Vector2 achorPosition;

    private List<RectTransform> displays = new List<RectTransform>();

    void Start()
    {
        ConnectionManager.UpdateList += updateList;
    }

    private void updateList(PhotonPlayer[] players)
    {
        destroyDisplays();
        foreach (PhotonPlayer p in players)
        {
            GameObject go = Instantiate(playerDisplay, this.transform);
            RectTransform rt = go.GetComponent<RectTransform>();
            rt.anchorMax = achorPosition;
            rt.anchorMin = achorPosition;
            rt.anchoredPosition = new Vector2(0, -180 * displays.Count - 80);
            TMP_Text txt = go.GetComponentInChildren<TMP_Text>();
            if (txt != null)
                txt.text = p.UserId;
            displays.Add(rt);
        }

    }

    private void destroyDisplays()
    {
        foreach (RectTransform rt in displays)
            Destroy(rt.gameObject);
        displays.Clear();
    }

    void OnDisable()
    {
        ConnectionManager.UpdateList -= updateList;
    }

    public void LeaveRoom()
    {
        ConnectionManager.CM.LeaveRoom();
    }
}
