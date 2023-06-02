using UnityEngine;

public class DiceFace : MonoBehaviour
{
    [SerializeField] private Dice dice;
    [SerializeField] private int number;

    void OnTriggerEnter(Collider col)
    {
        dice.SetNum(number);
    }
}
