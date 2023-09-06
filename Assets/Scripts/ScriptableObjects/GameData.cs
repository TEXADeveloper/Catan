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
}
