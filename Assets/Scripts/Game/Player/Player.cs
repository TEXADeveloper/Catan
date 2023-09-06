using UnityEngine;

public class Player
{
    public int ID;
    public Sprite Image;
    public string Name;
    private int[] resources;
    private int[] cards;
    private bool[] achievements;
    public byte VictoryPoints;

    public delegate void EventHandler();
    public event EventHandler ResourcesUpdated;
    public event EventHandler CardsUpdated;

    public void AddResources(int id, int amount)
    {
        resources[id] += amount;
        ResourcesUpdated?.Invoke();
    }

    public void AddCards(int id, int amount)
    {
        cards[id] += amount;
        CardsUpdated?.Invoke();
    }

    public int GetResources(int id)
    {
        return resources[id];
    }

    public int GetCards(int id)
    {
        return cards[id];
    }

    public void PayResources(int[] payment)
    {
        for (int i = 0; i < payment.Length; i++)
            resources[i] -= payment[i];
        ResourcesUpdated?.Invoke();
    }

    public Player()
    {
        resources = new int[5];
        cards = new int[5];
        achievements = new bool[2];
    }
}
