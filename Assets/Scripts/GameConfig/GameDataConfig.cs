using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameDataConfig : MonoBehaviour
{
    [Header("Cards")]
    private int[] amountOfMaterials = { 1, 2, 3, 4, 5 };
    private int[] amountOfProgress = { 1, 2, 3 };

    public int getMaterialCard(int id) { return amountOfMaterials[id - 1]; }

    public void SaveMaterialCards(int id, int num) => amountOfMaterials[id - 1] = num;

    public int getProgressCard(int id) { return amountOfProgress[id - 1]; }

    public void SaveProgressCards(int id, int num) => amountOfProgress[id - 1] = num;


    [Header("Terrain")]
    const int TERRAIN_AMOUNT = 19;
    private byte[] terrainData = new byte[TERRAIN_AMOUNT];
    [SerializeField] private byte[] defaultTerrainData = new byte[TERRAIN_AMOUNT];
    [SerializeField] private int[] defaultTerrainAmount = { 1, 3, 4, 3, 4, 4 };
    [SerializeField] private TerrainUI[] terrainTypes;
    [SerializeField] private Transform terrainOrderParent;
    [SerializeField] private TerrainLoader[] terrainLoaders;
    private Image[] terrainOrderMenu = new Image[TERRAIN_AMOUNT];
    private int[] terrainAmount = new int[6];
    private int pos1 = -1, pos2 = -1;

    void Awake()
    {
        fillTerrains();
        DefaultTerrainData();
    }

    private void fillTerrains()
    {
        for (int i = 0; i < terrainOrderParent.childCount; i++)
            terrainOrderMenu[i] = terrainOrderParent.GetChild(i).GetChild(0).GetComponent<Image>();
    }

    private void displayTerrainData()
    {
        for (int i = 0; i < terrainOrderMenu.Length; i++)
            terrainOrderMenu[i].sprite = terrainTypes[terrainData[i]].UIImage;
    }

    public void DefaultTerrainData()
    {
        for (int i = 0; i < terrainData.Length; i++)
            terrainData[i] = defaultTerrainData[i];
        for (int i = 0; i < terrainAmount.Length; i++)
        {
            terrainAmount[i] = defaultTerrainAmount[i];
            terrainLoaders[i].DisplayTerrainAmount(terrainAmount[i]);
        }
        displayTerrainData();
    }

    public void RandomizeTerrainData()
    {
        int index = 0;
        for (int i = 0; i < terrainAmount.Length; i++)
            for (int j = 0; j < terrainAmount[i]; j++)
            {
                terrainData[index] = Convert.ToByte(i);
                index++;
            }
        System.Random random = new System.Random();
        terrainData = terrainData.OrderBy(x => random.Next()).ToArray();
        displayTerrainData();
    }

    public void SelectTerrain(int pos)
    {
        if (pos1 == -1)
            pos1 = pos;
        else if (pos2 == -1)
            pos2 = pos;
        if (pos1 != pos2 && (pos1 != -1) && (pos2 != -1))
        {
            changeTerrainOrder(pos1, pos2);
            displayTerrainData();
            cleanPositions();
        }
        else if (pos1 == pos2)
            cleanPositions();
    }

    private void changeTerrainOrder(int i, int j)
    {
        byte tmp = terrainData[i];
        terrainData[i] = terrainData[j];
        terrainData[j] = tmp;
    }

    private void cleanPositions()
    {
        pos1 = -1;
        pos2 = -1;
    }

    public int GetTerrainAmount(int pos)
    {
        return terrainAmount[pos];
    }

    public int[] GetTerrainAmountArray()
    {
        return terrainAmount;
    }

    public void ChangeTerrainAmount(int pos, int value)
    {
        int total = 0;
        for (int i = 0; i < terrainAmount.Length; i++)
            if (i != pos)
                total += terrainAmount[i];
        if (total < 19 - value)
            terrainAmount[pos] = value;
        else
            terrainAmount[pos] = 19 - total;
    }
}
