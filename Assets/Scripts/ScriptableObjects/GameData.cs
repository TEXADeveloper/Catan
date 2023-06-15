using System;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "GameData")]
public class GameData : ScriptableObject
{
    public int[] AmountOfMaterials;
    public int[] AmountOfProgress;
    public byte[] TerrainOrder;
    public byte[] TerrainNumbers;

    public Player[] Players;

    public void InitializePlayer(int amount)
    {
        Players = new Player[amount];
        for (int i = 0; i < amount; i++)
            Players[i] = new Player();
    }
}
