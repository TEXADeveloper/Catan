using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class Terrain : MonoBehaviour
{
    [SerializeField] private Image circle;
    [SerializeField] private TMP_Text text;
    [SerializeField] private LayerMask townMask;
    [SerializeField] private LayerMask cityMask;
    [SerializeField] private LayerMask roadMask;
    [SerializeField] private bool isEmpty = false;
    private byte type;
    private byte number;

    Town[] towns;
    City[] cities;
    Road[] roads;

    public void SetType(byte value)
    {
        type = value;
    }

    public void SetNumber(byte value)
    {
        if (isEmpty)
            return;
        number = value;
        text.text = number.ToString();
        if (number == 0)
            circle.gameObject.SetActive(false);
    }

    void Start()
    {
        setArray<Town>(townMask, ref towns, 30);
        setArray<City>(cityMask, ref cities, 30);
        setArray<Road>(roadMask, ref roads, 0);

        if (!isEmpty)
            DiceManager.DiceResult += diceResult;
    }

    private void setArray<T>(int mask, ref T[] array, float initialAngle)
    {
        List<T> list = new List<T>();
        for (int i = 0; i < 6; i++)
        {
            Vector3 origin = transform.position;
            Vector3 direction = new Vector3(Mathf.Cos(Mathf.Deg2Rad * (initialAngle + 60 * i)), 0, Mathf.Sin(Mathf.Deg2Rad * (initialAngle + 60 * i)));
            RaycastHit hitInfo;
            bool hit = Physics.Raycast(origin, direction, out hitInfo, 10f, mask);
            if (hit)
                list.Add(hitInfo.collider.GetComponent<T>());
        }
        array = list.ToArray();
    }

    public void HideBuildings()
    {
        foreach (Town t in towns)
        {
            t.SetTerrain(this);
            t.gameObject.SetActive(false);
        }
        foreach (City c in cities)
        {
            c.SetTerrain(this);
            c.gameObject.SetActive(false);
        }
        foreach (Road r in roads)
        {
            r.SetTerrain(this);
            r.gameObject.SetActive(false);
        }
    }

    public bool Build(BuildingType type, Vector3 pos, int turn, Player player)
    {
        switch (type)
        {
            case BuildingType.town:
                return buildTowns(pos, turn, player);
            case BuildingType.city:
                return buildCity(pos, player);
            case BuildingType.road:
                return buildRoad(pos, player);
        }
        return false;
    }

    public bool buildTowns(Vector3 pos, int turn, Player player)
    {
        Town nearestTown = FindNearestTown(pos, false);
        if (nearestTown.gameObject.activeSelf || !nearestTown.CanBePlaced(turn, player))
            return false;
        nearestTown.gameObject.SetActive(true);
        nearestTown.Owner = player;
        nearestTown.TurnObtained = turn;
        player.AddVictoryPoints(1);
        return true;
    }

    public bool buildCity(Vector3 pos, Player player)
    {
        City nearestCity = FindNearestCity(pos, false);
        if (nearestCity.gameObject.activeSelf || !nearestCity.CanBePlaced(FindNearestTown(pos, false), player))
            return false;
        nearestCity.gameObject.SetActive(true);
        nearestCity.Owner = player;
        player.AddVictoryPoints(1);
        return true;
    }

    public bool buildRoad(Vector3 pos, Player player)
    {
        Road nearestRoad = FindNearestRoad(pos, false);
        if (nearestRoad.gameObject.activeSelf || !nearestRoad.CanBePlaced(player))
            return false;
        nearestRoad.gameObject.SetActive(true);
        nearestRoad.Owner = player;
        return true;
    }

    public City FindNearestCity(Vector3 pos, bool exclude)
    {
        City nearestCity = null;
        float shortestDistance = Mathf.Infinity;
        foreach (City city in cities)
        {
            float distance = Vector3.Distance(pos, city.transform.position);
            if (distance < shortestDistance && (!exclude || city.transform.position != pos))
            {
                shortestDistance = distance;
                nearestCity = city;
            }
        }
        return nearestCity;
    }

    public City[] FindNearestCityArray(Vector3 pos, bool isRoad)
    {
        int arrayPos = int.MinValue;
        float shortestDistance = Mathf.Infinity;
        for (int i = 0; i < cities.Length; i++)
        {
            float distance = Vector3.Distance(pos, cities[i].transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                arrayPos = i;
            }
        }
        City[] nearestCities = new City[2];
        if (isRoad && arrayPos != 0)
        {
            nearestCities[0] = cities[arrayPos + 1];
            nearestCities[1] = cities[arrayPos];
            return nearestCities;
        }

        if (isRoad)
        {
            if (towns[arrayPos].transform.position.y > pos.y)
                nearestCities[0] = cities[cities.Length - 1];
            else
                nearestCities[0] = cities[arrayPos + 1];
            nearestCities[1] = cities[arrayPos];
            return nearestCities;
        }

        nearestCities[0] = cities[(arrayPos + 1 >= cities.Length) ? 0 : arrayPos + 1];
        nearestCities[1] = cities[(arrayPos - 1 < 0) ? cities.Length - 1 : arrayPos - 1];
        return nearestCities;
    }

    public Town FindNearestTown(Vector3 pos, bool exclude)
    {
        Town nearestTown = null;
        float shortestDistance = Mathf.Infinity;
        foreach (Town town in towns)
        {
            float distance = Vector3.Distance(pos, town.transform.position);
            if (distance < shortestDistance && (!exclude || town.transform.position != pos))
            {
                shortestDistance = distance;
                nearestTown = town;
            }
        }
        return nearestTown;
    }

    public Town[] FindNearestTownArray(Vector3 pos, bool isRoad)
    {
        int arrayPos = int.MinValue;
        float shortestDistance = Mathf.Infinity;
        for (int i = 0; i < towns.Length; i++)
        {
            float distance = Vector3.Distance(pos, towns[i].transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                arrayPos = i;
            }
        }

        Town[] nearestTowns = new Town[2];
        if (isRoad && arrayPos != 0)
        {
            nearestTowns[0] = towns[(arrayPos + 1 < towns.Length) ? arrayPos + 1 : 0];
            nearestTowns[1] = towns[arrayPos];
            return nearestTowns;
        }

        if (isRoad)
        {
            if (towns[arrayPos].transform.position.y > pos.y)
                nearestTowns[0] = towns[towns.Length - 1];
            else
                nearestTowns[0] = towns[arrayPos + 1];
            nearestTowns[1] = towns[arrayPos];
            return nearestTowns;
        }

        nearestTowns[0] = towns[(arrayPos + 1 >= towns.Length) ? 0 : arrayPos + 1];
        nearestTowns[1] = towns[(arrayPos - 1 < 0) ? towns.Length - 1 : arrayPos - 1];
        return nearestTowns;
    }

    public Road FindNearestRoad(Vector3 pos, bool exclude)
    {
        Road nearestRoad = null;
        float shortestDistance = Mathf.Infinity;
        foreach (Road road in roads)
        {
            float distance = Vector3.Distance(pos, road.transform.position);
            if (distance < shortestDistance && (!exclude || road.transform.position != pos))
            {
                shortestDistance = distance;
                nearestRoad = road;
            }
        }
        return nearestRoad;
    }

    public Road[] FindNearestRoadArray(Vector3 pos, bool isTown)
    {
        int arrayPos = int.MinValue;
        float shortestDistance = Mathf.Infinity;
        for (int i = 0; i < roads.Length; i++)
        {
            float distance = Vector3.Distance(pos, roads[i].transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                arrayPos = i;
            }
        }
        Road[] nearestRoads = new Road[2];
        if (isTown && arrayPos != 0)
        {
            nearestRoads[0] = roads[(arrayPos + 1 < roads.Length) ? arrayPos + 1 : 0];
            nearestRoads[1] = roads[arrayPos];
            return nearestRoads;
        }

        if (isTown)
        {
            if (towns[arrayPos].transform.position.y > pos.y)
                nearestRoads[0] = roads[roads.Length - 1];
            else
                nearestRoads[0] = roads[arrayPos + 1];
            nearestRoads[1] = roads[arrayPos];
            return nearestRoads;
        }

        nearestRoads[0] = roads[(arrayPos + 1 >= roads.Length) ? 0 : arrayPos + 1];
        nearestRoads[1] = roads[(arrayPos - 1 < 0) ? roads.Length - 1 : arrayPos - 1];
        return nearestRoads;
    }

    private void diceResult(int value)
    {
        if (number == value)
        {
            foreach (Town t in towns)
                if (t.gameObject.activeSelf)
                    t.Owner.AddResources(type - 1, 1);
            foreach (City c in cities)
                if (c.gameObject.activeSelf)
                    c.Owner.AddResources(type - 1, 2);
        }
    }

    public void GiveInitialMaterials()
    {
        foreach (Town t in towns)
            if (t.gameObject.activeSelf && t.TurnObtained == 1)//TODO: Colocar valor adecuado
                t.Owner.AddResources(type - 1, 1);
    }

    void OnDisable()
    {
        if (!isEmpty)
            DiceManager.DiceResult -= diceResult;
    }
}