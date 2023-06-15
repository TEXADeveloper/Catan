using UnityEngine;
using UnityEngine.UI;

public class CardDislplay : MonoBehaviour
{
    [SerializeField] private Card cardType;

    void Start()
    {
        Image img = GetComponent<Image>();
        if (img != null)
            img.sprite = cardType.CardImage;
    }
}
