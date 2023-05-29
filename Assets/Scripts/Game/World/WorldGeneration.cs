using UnityEngine;
using System.Collections;

public class WorldGeneration : MonoBehaviour
{
    private readonly Vector2 distance = new Vector2(17.3f, 15f);
    [SerializeField] private Material terrainMaterial;
    [SerializeField] private GameObject terrainBase;
    [SerializeField] private GameObject[] terrainTypes;
    private Terrain[] terrains;

    public void CreateTerrain(byte[] terrainOrder)
    {
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
}
