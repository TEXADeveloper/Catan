using UnityEngine;

public class City : MonoBehaviour, Building
{
    Terrain[] terrains = new Terrain[3];
    int i = 0;

    public void SetTerrain(Terrain t)
    {
        terrains[i] = t;
        i++;
    }
}
