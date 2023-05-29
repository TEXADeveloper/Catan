using UnityEngine;

public class Terrain : MonoBehaviour
{
    const float RADIUS = 11f;
    Town[] towns;
    City[] cities;
    Road[] roads;

    void Start()
    {
        setArray<Town>(7, ref towns);
        setArray<City>(8, ref cities);
        setArray<Road>(9, ref roads);
    }

    private void setArray<T>(int mask, ref T[] array)
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, RADIUS, 1 << mask);
        array = new T[cols.Length];
        for (int i = 0; i < cols.Length; i++)
            array[i] = cols[i].GetComponent<T>();
    }

    public void HideBuildings()
    {
        foreach (Town t in towns)
            t.gameObject.SetActive(false);
        foreach (City c in cities)
            c.gameObject.SetActive(false);
        foreach (Road r in roads)
            r.gameObject.SetActive(false);
    }

    public bool Build(BuildingType type, Vector3 pos)
    {
        switch (type)
        {
            case BuildingType.town:
                return buildTowns(pos);
            case BuildingType.city:
                return buildCity(pos);
            case BuildingType.road:
                return buildRoad(pos);
        }
        return false;
    }

    public bool buildTowns(Vector3 pos)
    {
        Town nearestTown = null;
        float shortestDistance = Mathf.Infinity;
        foreach (Town town in towns)
        {
            float distance = Vector3.Distance(pos, town.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestTown = town;
            }
        }
        if (nearestTown.gameObject.activeSelf || !canPlaceTown(nearestTown.transform.position))
            return false;
        nearestTown.gameObject.SetActive(true);
        return true;
    }

    public bool buildCity(Vector3 pos)
    {
        City nearestCity = null;
        float shortestDistance = Mathf.Infinity;
        foreach (City city in cities)
        {
            float distance = Vector3.Distance(pos, city.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestCity = city;
            }
        }
        if (nearestCity.gameObject.activeSelf || !canPlaceCity(nearestCity.transform.position))
            return false;
        nearestCity.gameObject.SetActive(true);
        return true;
    }

    public bool buildRoad(Vector3 pos)
    {
        Road nearestRoad = null;
        float shortestDistance = Mathf.Infinity;
        foreach (Road road in roads)
        {
            float distance = Vector3.Distance(pos, road.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestRoad = road;
            }
        }
        if (nearestRoad.gameObject.activeSelf || !canPlaceRoad(nearestRoad.transform.position))
            return false;
        nearestRoad.gameObject.SetActive(true);
        return true;
    }

    private bool canPlaceTown(Vector3 pos)
    {
        foreach (City c in cities)
            if (c.transform.position == pos && !c.gameObject.activeSelf)
                return true;
        return false;
    }

    private bool canPlaceCity(Vector3 pos)
    {
        foreach (Town t in towns)
            if (t.transform.position == pos && t.gameObject.activeSelf)
            {
                t.gameObject.SetActive(false);
                return true;
            }
        return false;
    }

    private bool canPlaceRoad(Vector3 pos)
    {
        for (int i = 0; i < roads.Length - 1; i++)
            for (int j = i; j < roads.Length; j++)
                if (roads[i].transform.position == pos || (Vector3.Distance(pos, roads[i].transform.position) < Vector3.Distance(pos, roads[j].transform.position)))
                {
                    Road tmp = roads[i];
                    roads[i] = roads[j];
                    roads[j] = tmp;
                }
        //TODO: Falta verificar las ciudades y poblados
        return (roads[0].gameObject.activeSelf || roads[1].gameObject.activeSelf);
    }
}