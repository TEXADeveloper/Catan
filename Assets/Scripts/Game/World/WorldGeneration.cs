using UnityEngine;
using System;
using System.Linq;

enum TerrainTypes
{
    Desert = 0,
    Clay = 1,
    Forest = 2,
    Mountains = 3,
    Plain = 4,
    Wheat = 5
}

public class WorldGeneration : MonoBehaviour
{
    const int TERRAIN_AMOUNT = 19;
    private readonly Vector2 distance = new Vector2(17.3f, 15f);
    private byte[] terrainData = new byte[TERRAIN_AMOUNT];
    [SerializeField] private Material terrainMaterial;
    [SerializeField] private GameObject terrainBase;
    [SerializeField] private GameObject[] terrains;
    [SerializeField] private int[] amount;

    void Start()
    {
        createTerrain();
    }

    private void createTerrain()
    {
        fillTerrainData(amount);
        int index = 0;
        for (int y = 2; y >= -2; y--)
            if (y % 2 == 0)
            {
                int n = (Mathf.Abs(y) == 2) ? 1 : 2;
                for (int x = -n; x <= n; x++)
                {
                    Vector2 position = new Vector2(x * distance.x, y * distance.y);
                    instantiateTerrain(position, terrainData[index]);
                    index++;
                }
            }
            else
                for (int x = -2; x <= 2; x++)
                    if (x != 0)
                    {
                        int n = (Mathf.Abs(x) > 1) ? sign(x) : 0;
                        Vector2 position = new Vector2(sign(x) * distance.x / 2 + n * distance.x, y * distance.y);
                        instantiateTerrain(position, terrainData[index]);
                        index++;
                    }
    }

    private void fillTerrainData(int[] amount)
    {
        int index = 0;
        for (int i = 0; i < amount.Length; i++)
            for (int j = 0; j < amount[i]; j++)
            {
                terrainData[index] = Convert.ToByte(i);
                index++;
            }
        System.Random random = new System.Random();
        terrainData = terrainData.OrderBy(x => random.Next()).ToArray();
    }



    private int sign(int i)
    {
        if (i > 0)
            return 1;
        else if (i < 0)
            return -1;
        return 0;
    }

    private void instantiateTerrain(Vector2 position, byte type)
    {
        GameObject spawnedBase = GameObject.Instantiate(terrainBase, new Vector3(position.x, 0, position.y), Quaternion.Euler(-90, 0, 0));
        GameObject spawnedTerrain = GameObject.Instantiate(terrains[type], new Vector3(position.x, 0, position.y), Quaternion.Euler(-90, 0, 0));
        spawnedBase.GetComponent<MeshRenderer>().material = terrainMaterial;
        spawnedTerrain.GetComponent<MeshRenderer>().material = terrainMaterial;
        spawnedTerrain.transform.parent = spawnedBase.transform;
        spawnedBase.transform.parent = this.transform;
    }
}
