using UnityEngine;

public enum CardType
{
    knight = 1,
    road = 2,
    invent = 3,
    monopoly = 4,
    point = 5
};

public class PlayerCards : MonoBehaviour
{
    [SerializeField] private GameData gameData;
    Player p;

    public void BuyCard()
    {
        p = gameData.Players[0];
        p.AddCards(Random.Range(0, 5), 1);
    }
}
