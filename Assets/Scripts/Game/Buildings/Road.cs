using UnityEngine;
using System.Collections.Generic;

public class Road : MonoBehaviour, Building
{
    Terrain[] terrains = new Terrain[3];
    int i = 0;

    public void SetTerrain(Terrain t)
    {
        terrains[i] = t;
        i++;
    }

    public Town[] GetNearestTowns()
    {
        //? Hay que obtener la mas cercana y la siguiente
        Town[] towns = terrains[0].FindNearestTownArray(transform.position, true);
        return towns;
    }

    public City[] GetNearestCities()
    {
        City[] cities = terrains[0].FindNearestCityArray(transform.position, true); ;
        return cities;
    }

    //TODO: implementar los terrenos invisibles a los lados;
    public Road[] GetNearestRoads()
    {
        Road[] tmp1 = terrains[0].FindNearestRoadArray(transform.position, false);
        Road[] tmp2 = new Road[0];
        if (i >= 2)
            tmp2 = terrains[1].FindNearestRoadArray(transform.position, false);
        List<Road> roads = new List<Road>();

        for (int i = 0; i < tmp1.Length + tmp2.Length; i++)
            if (i < tmp1.Length)
                addToList<Road>(ref roads, tmp1[i]);
            else if (i < tmp1.Length + tmp2.Length)
                addToList<Road>(ref roads, tmp2[i - tmp1.Length]);

        return roads.ToArray();
    }

    private void addToList<T>(ref List<T> list, T element)
    {
        bool repeated = false;
        foreach (T t in list)
            if (t.Equals(element))
            {
                repeated = true;
                break;
            }
        if (!repeated)
            list.Add(element);
    }
}
