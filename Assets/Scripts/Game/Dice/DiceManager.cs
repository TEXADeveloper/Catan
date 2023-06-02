using UnityEngine;
using TMPro;

public class DiceManager : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    int[] dices = new int[2];
    int callCount = 0;

    public void SetDiceValue(int value, byte id)
    {
        dices[id] = value;
        callCount++;
        if (callCount == 2)
            showResult();
    }

    private void showResult()
    {
        callCount = 0;
        text.text = "Salió: " + (dices[0] + dices[1]);
    }
}
