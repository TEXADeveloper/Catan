using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "GameData")]
public class GameData : ScriptableObject
{
    public int[] AmountOfMaterials;
    public int[] AmountOfProgress;
    public byte[] TerrainOrder;
    public byte[] TerrainNumbers;

    public List<Player> Players = new List<Player>();

    public void AddPlayer(Player p)
    {
        if (!Players.Contains(p))
        {
            p.ID = Players.Count;
            Players.Add(p);
        }
    }

    public void RemovePlayer(Player p)
    {
        if (Players.Contains(p))
            Players.Remove(p);
    }

    public void RemovePlayer(string n)
    {
        foreach (Player p in Players)
            if (p.Name == n)
            {
                Players.Remove(p);
                return;
            }
    }

    public Player GetPlayer(int i)
    {
        if (Players.Count > i)
            return Players[i];
        return null;
    }
}
