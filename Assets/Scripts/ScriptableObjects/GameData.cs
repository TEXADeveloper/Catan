using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData")]
public class GameData : ScriptableObject
{
    public int[] AmountOfMaterials;
    public int[] AmountOfProgress;
    public byte[] TerrainOrder;

    public Player[] Players;
}
