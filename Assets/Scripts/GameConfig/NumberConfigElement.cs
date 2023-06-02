using UnityEngine;
using TMPro;

public class NumberConfigElement : MonoBehaviour
{
    public int nextElement;
    public int internalElement;
    [SerializeField] private TMP_Text text;

    public void SetText(int number, bool show)
    {
        if (show)
            text.text = number.ToString();
        else
            text.text = "";
    }
}
