using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class CardLoader : MonoBehaviour
{
    [SerializeField] private GameDataConfig gameDataConfig;
    [SerializeField] private Card cardType;
    [SerializeField] private TMP_Text nameDisplay;
    [SerializeField] private Image image;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField, Range(0, 19)] private int minValue;
    [SerializeField, Range(0, 19)] private int maxValue;
    private int num = 0;

    void Start()
    {
        nameDisplay.text = cardType.name;
        image.sprite = cardType.Image;
        inputField.text = gameDataConfig.getMaterialCard(cardType.ID).ToString();
    }

    public void ChangeValue(string stringValue)
    {
        int value = 0;
        int.TryParse(stringValue, System.Globalization.NumberStyles.Integer, null, out value);
        if ((maxValue == 0 && value >= minValue) || (value <= maxValue && value >= minValue))
            num = value;
        else
        {
            num = Mathf.Clamp(value, minValue, maxValue);
            inputField.text = num.ToString();
        }
        if (cardType.IsResource)
            gameDataConfig.SaveMaterialCards(cardType.ID, num);
        else
            gameDataConfig.SaveProgressCards(cardType.ID, num);
    }
    public void PressButton(int amount)
    {
        inputField.text = (num + amount).ToString();
    }
}
