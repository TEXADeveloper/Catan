using UnityEngine;

public class City : MonoBehaviour
{
    public Player Owner;
    Terrain[] terrains = new Terrain[3];
    int i = 0;

    public void SetTerrain(Terrain t)
    {
        terrains[i] = t;
        i++;
    }

    public bool CanBePlaced(Town t)
    {
        if (t.gameObject.activeSelf)
        {
            t.gameObject.SetActive(false);
            return true;
        }
        return false;
    }
}
