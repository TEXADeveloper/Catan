using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class TerrainLoader : MonoBehaviour
{
    [SerializeField] private GameDataConfig gameDataConfig;
    [SerializeField] private TerrainUI terrainType;
    [SerializeField] private TMP_Text nameDisplay;
    [SerializeField] private Image image;
    [SerializeField] private TMP_InputField inputField;
    private int minValue = 0;

    void Start()
    {
        nameDisplay.text = terrainType.name;
        image.sprite = terrainType.UIImage;
        inputField.text = gameDataConfig.GetTerrainAmount(terrainType.ID).ToString();
    }

    public void ChangeValue(string stringValue)
    {
        int value = 0;
        int.TryParse(stringValue, System.Globalization.NumberStyles.Integer, null, out value);
        int amount = gameDataConfig.GetTerrainAmount(terrainType.ID);
        if (stringValue != amount.ToString() && value >= minValue)
            gameDataConfig.ChangeTerrainAmount(terrainType.ID, value);
        DisplayTerrainAmount(gameDataConfig.GetTerrainAmount(terrainType.ID));
    }

    public void PressButton(int amount)
    {
        DisplayTerrainAmount(gameDataConfig.GetTerrainAmount(terrainType.ID) + amount);
        RandomizeTerrain();
    }

    public void DisplayTerrainAmount(int amount)
    {
        inputField.text = amount.ToString();
    }

    public void RandomizeTerrain()
    {
        int total = 0;
        for (int i = 0; i < gameDataConfig.GetTerrainAmountArray().Length; i++)
            total += gameDataConfig.GetTerrainAmount(i);
        if (total == 19)
            gameDataConfig.RandomizeTerrainData();
    }
}
