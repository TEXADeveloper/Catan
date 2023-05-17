using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData")]
public class GameData : ScriptableObject
{
    const int TERRAIN_AMOUNT = 19;

    public int[] TerrainAmount;
    public int[] CardsAmount;
    private byte[] terrainOrder = new byte[TERRAIN_AMOUNT];

    private void orderTerrain(int[] amount)
    {
        int index = 0;
        for (int i = 0; i < amount.Length; i++)
            for (int j = 0; j < amount[i]; j++)
            {
                terrainOrder[index] = Convert.ToByte(i);
                index++;
            }
        System.Random random = new System.Random();
        terrainOrder = terrainOrder.OrderBy(x => random.Next()).ToArray();
    }

    public Player[] Players;


}
