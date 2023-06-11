using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameDataConfig : MonoBehaviour
{
    [Header("General")]
    #region general
    [SerializeField] private GameData gameData;
    private int[] numbers;

    public void StartGame()
    {
        copyArray(amountOfMaterials, ref gameData.AmountOfMaterials);
        copyArray(amountOfProgress, ref gameData.AmountOfProgress);
        copyArray(terrainData, ref gameData.TerrainOrder);
        copyArray(numberElements, ref gameData.TerrainNumbers);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void copyArray(int[] origin, ref int[] destiny)
    {
        destiny = new int[origin.Length];
        for (int i = 0; i < origin.Length; i++)
            destiny[i] = origin[i];
    }

    private void copyArray(byte[] origin, ref byte[] destiny)
    {
        destiny = new byte[origin.Length];
        for (int i = 0; i < origin.Length; i++)
            destiny[i] = origin[i];
    }

    private void copyArray(NumberConfigElement[] origin, ref byte[] destiny)
    {
        destiny = new byte[origin.Length];
        for (int i = 0; i < origin.Length; i++)
            destiny[i] = origin[i].number;
    }

    #endregion

    [Header("Cards")]
    #region cards
    private int[] amountOfMaterials = { 19, 19, 19, 19, 19 };
    private int[] amountOfProgress = { 14, 2, 2, 2, 5 }; //25 en total (14 caballeros, 6 progreso(2 de cada una), 5 puntos de victoria)

    public int getMaterialCard(int id) { return amountOfMaterials[id - 1]; }

    public void SaveMaterialCards(int id, int num) => amountOfMaterials[id - 1] = num;

    public int getProgressCard(int id) { return amountOfProgress[id - 1]; }

    public void SaveProgressCards(int id, int num) => amountOfProgress[id - 1] = num;
    #endregion

    [Header("Terrain")]
    #region terrain
    [SerializeField] private byte[] defaultTerrainData = new byte[TERRAIN_AMOUNT];
    [SerializeField] private int[] defaultTerrainAmount = { 1, 3, 4, 3, 4, 4 };
    [SerializeField] private TerrainUI[] terrainTypes;
    [SerializeField] private Transform terrainOrderParent;
    [SerializeField] private TerrainLoader[] terrainLoaders;

    private const int TERRAIN_AMOUNT = 19;
    private byte[] terrainData = new byte[TERRAIN_AMOUNT];

    private Image[] terrainOrderMenu = new Image[TERRAIN_AMOUNT];
    private int[] terrainAmount = new int[6];
    private int tPos1 = -1, tPos2 = -1;

    void Awake()
    {
        fillTerrains();
        DefaultTerrainData();
        DefaultNumberData();
    }

    private void fillTerrains()
    {
        for (int i = 0; i < terrainOrderParent.childCount; i++)
        {
            terrainOrderMenu[i] = terrainOrderParent.GetChild(i).GetChild(0).GetComponent<Image>();
            numberElements[i] = terrainNumberParent.GetChild(i).GetComponent<NumberConfigElement>();
        }
    }

    private void displayTerrainData()
    {
        for (int i = 0; i < terrainOrderMenu.Length; i++)
        {
            terrainOrderMenu[i].sprite = terrainTypes[terrainData[i]].UIImage;
            numberElements[i].transform.GetChild(0).GetComponent<Image>().sprite = terrainTypes[terrainData[i]].UIImage;
        }
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
        int total = 0;
        for (int i = 0; i < terrainAmount.Length; i++)
            total += terrainAmount[i];
        if (total != TERRAIN_AMOUNT)
            return;

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
        displayNumbers(start);
    }

    public void SelectTerrain(int pos)
    {
        if (tPos1 == -1)
            tPos1 = pos;
        else if (tPos2 == -1)
            tPos2 = pos;
        if (tPos1 != tPos2 && (tPos1 != -1) && (tPos2 != -1))
        {
            changeTerrainOrder(tPos1, tPos2);
            displayTerrainData();
            displayNumbers(start);
            cleanTerrainPositions();
        }
        else if (tPos1 == tPos2)
            cleanTerrainPositions();
    }

    private void changeTerrainOrder(int i, int j)
    {
        byte tmp = terrainData[i];
        terrainData[i] = terrainData[j];
        terrainData[j] = tmp;
    }

    private void cleanTerrainPositions()
    {
        tPos1 = -1;
        tPos2 = -1;
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
    #endregion

    [Header("Numbers")]
    #region numbers
    [SerializeField] private byte[] defaultNumberOrder;
    private byte[] numberOrder;
    [SerializeField] private Transform terrainNumberParent;
    private NumberConfigElement[] numberElements = new NumberConfigElement[TERRAIN_AMOUNT];
    private int numPos1 = -1, numPos2 = -1;
    private int start;
    private bool cleaned = false;

    public void SetCleaned(bool value) => cleaned = value;

    public void DefaultNumberData()
    {
        numberOrder = new byte[defaultNumberOrder.Length];
        for (int i = 0; i < numberOrder.Length; i++)
            numberOrder[i] = defaultNumberOrder[i];
        start = 0;
        displayNumbers(start);
    }

    public void UpdateNumbers()
    {
        displayNumbers(start);
    }

    public void displayNumbers(int start)
    {
        int position = 0;
        int index = start;
        numberElements[index].SetText((hideNumber(index)) ? (byte)0 : numberOrder[position++], position - 1);
        while (numberElements[index].nextElement != start)
        {
            index = numberElements[index].nextElement;
            numberElements[index].SetText((hideNumber(index)) ? (byte)0 : numberOrder[position++], position - 1);
        }

        index = numberElements[index].internalElement;
        start = index;
        numberElements[index].SetText((hideNumber(index)) ? (byte)0 : numberOrder[position++], position - 1);
        while (numberElements[index].nextElement != start)
        {
            index = numberElements[index].nextElement;
            numberElements[index].SetText((hideNumber(index)) ? (byte)0 : numberOrder[position++], position - 1);
        }
        numberElements[9].SetText((hideNumber(9)) ? (byte)0 : numberOrder[position++], position - 1);
    }

    bool hideNumber(int i)
    {
        return cleaned || terrainData[i] == 0;
    }

    public void SelectNumber(NumberConfigElement element)
    {
        if (cleaned)
            return;
        if (numPos1 == -1 && element.position != -1)
            numPos1 = element.position;
        else if (numPos2 == -1 && element.position != -1)
            numPos2 = element.position;
        if (numPos1 != numPos2 && (numPos1 != -1) && (numPos2 != -1))
        {
            changeNumberOrder(numPos1, numPos2);
            displayNumbers(start);
            cleanNumPositions();
        }
        else if (numPos1 == numPos2)
            cleanNumPositions();
    }

    private void changeNumberOrder(int i, int j)
    {
        byte tmp = numberOrder[i];
        numberOrder[i] = numberOrder[j];
        numberOrder[j] = tmp;
    }

    private void cleanNumPositions()
    {
        numPos1 = -1;
        numPos2 = -1;
    }

    public void CleanNumbers()
    {
        SetCleaned(true);
        start = 0;
        DefaultNumberData();
    }

    public void SetNumberStart(int number)
    {
        if (number == -1 || !cleaned)
            return;
        SetCleaned(false);
        start = number;
        displayNumbers(start);
    }
    #endregion
}