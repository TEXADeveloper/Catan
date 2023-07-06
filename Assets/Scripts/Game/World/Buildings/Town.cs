using UnityEngine;
using System.Collections.Generic;

public class Town : MonoBehaviour
{
    public Player Owner;
    Terrain[] terrains = new Terrain[3];
    int i = 0;

    public void SetTerrain(Terrain t)
    {
        terrains[i] = t;
        i++;
    }

    public bool CanBePlaced(int turn)
    {
        Town[] nearestTowns = getNearestTowns();
        for (int i = 0; i < nearestTowns.Length; i++)
            if (nearestTowns[i].gameObject.activeSelf)
                return false;

        if (turn <= 2)
            return true;

        City[] nearestCities = getNearestCities();
        for (int i = 0; i < nearestCities.Length; i++)
            if (nearestCities[i].gameObject.activeSelf)
                return false;

        Road[] nearestRoads = getNearestRoads();
        for (int i = 0; i < nearestRoads.Length; i++)
            if (nearestRoads[i].gameObject.activeSelf)
                return true;

        return false;
    }

    private Town[] getNearestTowns()
    {
        Town[] tmp1 = terrains[0].FindNearestTownArray(transform.position, false);
        Town[] tmp2 = new Town[0];
        Town[] tmp3 = new Town[0];
        if (i >= 2)
            tmp2 = terrains[1].FindNearestTownArray(transform.position, false);
        if (i > 2)
            tmp3 = terrains[2].FindNearestTownArray(transform.position, false);
        List<Town> towns = new List<Town>();

        for (int i = 0; i < tmp1.Length + tmp2.Length + tmp3.Length; i++)
            if (i < tmp1.Length)
                addToList<Town>(ref towns, tmp1[i]);
            else if (i < tmp1.Length + tmp2.Length)
                addToList<Town>(ref towns, tmp2[i - tmp1.Length]);
            else if (i < tmp1.Length + tmp2.Length + tmp3.Length)
                addToList<Town>(ref towns, tmp3[i - tmp1.Length - tmp2.Length]);

        return towns.ToArray();
    }

    private City[] getNearestCities()
    {
        City[] tmp1 = terrains[0].FindNearestCityArray(transform.position, false);
        City[] tmp2 = new City[0];
        City[] tmp3 = new City[0];
        if (i >= 2)
            tmp2 = terrains[1].FindNearestCityArray(transform.position, false);
        if (i > 2)
            tmp3 = terrains[2].FindNearestCityArray(transform.position, false);
        List<City> cities = new List<City>();

        for (int i = 0; i < tmp1.Length + tmp2.Length + tmp3.Length; i++)
            if (i < tmp1.Length)
                addToList<City>(ref cities, tmp1[i]);
            else if (i < tmp1.Length + tmp2.Length)
                addToList<City>(ref cities, tmp2[i - tmp1.Length]);
            else if (i < tmp1.Length + tmp2.Length + tmp3.Length)
                addToList<City>(ref cities, tmp3[i - tmp1.Length - tmp2.Length]);

        return cities.ToArray();
    }

    private Road[] getNearestRoads()
    {
        Road[] tmp1 = terrains[0].FindNearestRoadArray(transform.position, false);
        Road[] tmp2 = new Road[0];
        Road[] tmp3 = new Road[0];
        if (i >= 2)
            tmp2 = terrains[1].FindNearestRoadArray(transform.position, false);
        if (i > 2)
            tmp3 = terrains[2].FindNearestRoadArray(transform.position, false);
        List<Road> roads = new List<Road>();

        for (int i = 0; i < tmp1.Length + tmp2.Length + tmp3.Length; i++)
            if (i < tmp1.Length)
                addToList<Road>(ref roads, tmp1[i]);
            else if (i < tmp1.Length + tmp2.Length)
                addToList<Road>(ref roads, tmp2[i - tmp1.Length]);
            else if (i < tmp1.Length + tmp2.Length + tmp3.Length)
                addToList<Road>(ref roads, tmp3[i - tmp1.Length - tmp2.Length]);

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