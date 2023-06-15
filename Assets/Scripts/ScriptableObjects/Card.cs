using UnityEngine;

[CreateAssetMenu(fileName = "Card")]
public class Card : ScriptableObject
{
    public byte ID;
    public Sprite CardImage;
    public Sprite CardIcon;
    public string Name;
    public bool IsResource = true;
}
