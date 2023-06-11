using UnityEngine;
using TMPro;

public class NumberConfigElement : MonoBehaviour
{
    [HideInInspector] public int position = -1;
    [HideInInspector] public byte number = 0;
    public int nextElement;
    public int internalElement;
    [SerializeField] private TMP_Text text;

    public void SetText(byte number, int index)
    {
        if (number != 0)
        {
            position = index;
            this.number = number;
            text.text = number.ToString();
        }
        else
        {
            position = -1;
            this.number = number;
            text.text = "";
        }
    }
}
