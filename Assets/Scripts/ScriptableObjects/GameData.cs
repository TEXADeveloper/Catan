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

    public Player[] Players;

    List<int> extIndex;
    List<int> intIndex;
}
