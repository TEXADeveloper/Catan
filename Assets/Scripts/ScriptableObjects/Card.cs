using UnityEngine;

[CreateAssetMenu(fileName = "Card")]
public class Card : ScriptableObject
{
    public byte ID;
    public Sprite Image;
    public string Name;
    public bool IsResource = true;
}
