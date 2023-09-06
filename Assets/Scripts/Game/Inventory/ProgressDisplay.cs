using UnityEngine;
using TMPro;
using System.Diagnostics;

public class ProgressDisplay : MonoBehaviour
{
    [SerializeField] private CardUI cardType;
    [SerializeField] private TMP_Text text;
    private Player player;

    private ICard card;
    int amount = 0;

    void Start()
    {
        if (cardType == null)
            card = null;
        else if (cardType.ID == 1)
            card = new KnightICard();
        else if (cardType.ID == 2)
            card = new RoadICard();
        else if (cardType.ID == 3)
            card = new InventICard();
        else if (cardType.ID == 4)
            card = new MonopolyICard();
        else if (cardType.ID == 5)
            card = new PointICard();
    }

    public void SetPlayer(Player newPlayer)
    {
        if (player != null)
            player.CardsUpdated -= updateAmount;
        newPlayer.CardsUpdated += updateAmount;

        player = newPlayer;
    }

    void updateAmount()
    {
        if (player != null)
        {
            amount = player.GetCards(cardType.ID - 1);
            text.text = amount.ToString();
        }
    }

    void OnDisable()
    {
        if (player != null)
            player.CardsUpdated -= updateAmount;
    }

    public void UseCard()
    {
        if (amount > 0)
        {
            player.AddCards(cardType.ID - 1, -1);
            card?.Action();
        }
    }
}