using UnityEngine;
using TMPro;

public class ResourcesDisplay : MonoBehaviour
{
    [SerializeField] private CardUI cardType;
    [SerializeField] private TMP_Text text;
    private Player player;

    public void SetPlayer(Player newPlayer)
    {
        if (player != null)
            player.ResourcesUpdated -= updateAmount;
        newPlayer.ResourcesUpdated += updateAmount;

        player = newPlayer;
    }

    void updateAmount()
    {
        if (player != null)
            text.text = player.GetResources(cardType.ID - 1).ToString();
    }

    void OnDisable()
    {
        if (player != null)
            player.ResourcesUpdated -= updateAmount;
    }
}
