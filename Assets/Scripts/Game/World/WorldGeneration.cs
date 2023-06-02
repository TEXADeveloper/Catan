using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldGeneration : MonoBehaviour
{
    private readonly Vector2 distance = new Vector2(17.3f, 15f);
    [SerializeField] private Material terrainMaterial;
    [SerializeField] private GameObject terrainBase;
    [SerializeField] private GameObject[] terrainTypes;
    private Terrain[] terrains;

    [SerializeField] private LayerMask terrainLayer;
    List<Terrain> externalTerrains = new List<Terrain>();
    List<Terrain> internalTerrains = new List<Terrain>();
    Terrain centerTerrain;

    public void CreateTerrain(byte[] terrainOrder)
    {
        if (terrainOrder.Length == 0)
            return;
        terrains = new Terrain[terrainOrder.Length];
        int index = 0;
        for (int y = 2; y >= -2; y--)
            if (y % 2 == 0)
            {
                int n = (Mathf.Abs(y) == 2) ? 1 : 2;
                for (int x = -n; x <= n; x++)
                {
                    Vector2 position = new Vector2(x * distance.x, y * distance.y);
                    instantiateTerrain(position, terrainOrder[index], index);
                    index++;
                }
            }
            else
                for (int x = -2; x <= 2; x++)
                    if (x != 0)
                    {
                        int n = (Mathf.Abs(x) > 1) ? sign(x) : 0;
                        Vector2 position = new Vector2(sign(x) * distance.x / 2 + n * distance.x, y * distance.y);
                        instantiateTerrain(position, terrainOrder[index], index);
                        index++;
                    }
        StartCoroutine(hideStructures());
        initializeCircularLists();
    }

    private IEnumerator hideStructures()
    {
        yield return null;
        foreach (Terrain t in terrains)
            t.HideBuildings();
    }

    private int sign(int i)
    {
        if (i > 0)
            return 1;
        else if (i < 0)
            return -1;
        return 0;
    }

    private void instantiateTerrain(Vector2 position, byte type, int index)
    {
        GameObject spawnedBase = GameObject.Instantiate(terrainBase, new Vector3(position.x, 0, position.y), Quaternion.Euler(-90, 0, 0));
        GameObject spawnedTerrain = GameObject.Instantiate(terrainTypes[type], new Vector3(position.x, 0, position.y), Quaternion.Euler(-90, 0, 0));

        spawnedBase.GetComponent<MeshRenderer>().material = terrainMaterial;
        spawnedTerrain.GetComponent<MeshRenderer>().material = terrainMaterial;

        spawnedTerrain.transform.parent = spawnedBase.transform;
        spawnedBase.transform.parent = this.transform;
        spawnedBase.name = "Terrain " + type + " " + position;

        terrains[index] = spawnedBase.AddComponent<Terrain>();
    }

    private void initializeCircularLists()
    {
        centerTerrain = terrains[9];
        Collider[] intCols = Physics.OverlapSphere(centerTerrain.transform.position, distance.x, terrainLayer);
        foreach (Collider col in intCols)
            internalTerrains.Add(col.GetComponent<Terrain>());

        Collider[] extCols = Physics.OverlapSphere(centerTerrain.transform.position, distance.x * 2, terrainLayer);
        foreach (Collider col in extCols)
            externalTerrains.Add(col.GetComponent<Terrain>());

        foreach (Terrain t in internalTerrains)
            externalTerrains.Remove(t);
        internalTerrains.Remove(centerTerrain);
        externalTerrains.Remove(centerTerrain);
        StartCoroutine(a());
    }

    private IEnumerator a()
    {
        yield return new WaitForSeconds(1f);
        foreach (Terrain t in externalTerrains)
        {
            Destroy(t.gameObject);
            yield return new WaitForSeconds(1f);
        }
        foreach (Terrain t in internalTerrains)
        {
            Destroy(t.gameObject);
            yield return new WaitForSeconds(1f);
        }
        Destroy(centerTerrain.gameObject);
    }
}
