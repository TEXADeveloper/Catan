using UnityEngine;

public class Player
{
    public int ID;
    public Sprite Image;
    public string Name;
    public int[] resources;
    public int[] cards;
    public bool[] achievements;
    public byte VictoryPoints;

    public delegate void EventHandler();
    public event EventHandler ResourcesUpdated;

    public void AddResources(int id, int amount)
    {
        resources[id] += amount;
        ResourcesUpdated?.Invoke();
    }

    public Player()
    {
        resources = new int[5];
        cards = new int[5];
        achievements = new bool[2];
    }
}
