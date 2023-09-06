using UnityEngine;
using System;
using TMPro;

public class DiceManager : MonoBehaviour
{
    public static event Action<int> DiceResult;

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
        DiceResult?.Invoke(dices[0] + dices[1]);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
            DiceResult?.Invoke(2);
        if (Input.GetKeyDown(KeyCode.Alpha3))
            DiceResult?.Invoke(3);
        if (Input.GetKeyDown(KeyCode.Alpha4))
            DiceResult?.Invoke(4);
        if (Input.GetKeyDown(KeyCode.Alpha5))
            DiceResult?.Invoke(5);
        if (Input.GetKeyDown(KeyCode.Alpha6))
            DiceResult?.Invoke(6);
        if (Input.GetKeyDown(KeyCode.Alpha7))
            DiceResult?.Invoke(7);
        if (Input.GetKeyDown(KeyCode.Alpha8))
            DiceResult?.Invoke(8);
        if (Input.GetKeyDown(KeyCode.Alpha9))
            DiceResult?.Invoke(9);
        if (Input.GetKeyDown(KeyCode.Alpha0))
            DiceResult?.Invoke(10);
        if (Input.GetKeyDown(KeyCode.Alpha1))
            DiceResult?.Invoke(11);
        if (Input.GetKeyDown(KeyCode.Plus))
            DiceResult?.Invoke(12);
    }
}
