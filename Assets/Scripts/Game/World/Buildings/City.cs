using UnityEngine;

public class City : MonoBehaviour
{
    public Player Owner;
    // clay, wood, ore, wool, wheat
    int[] price = { 0, 0, 3, 0, 2 };
    Terrain[] terrains = new Terrain[3];
    int i = 0;

    public void SetTerrain(Terrain t)
    {
        terrains[i] = t;
        i++;
    }

    public bool CanBePlaced(Town t, Player p)
    {
        if (t.gameObject.activeSelf && canPay(p))
        {
            p.PayResources(price);
            t.gameObject.SetActive(false);
            return true;
        }
        return false;
    }

    private bool canPay(Player p)
    {
        bool canPay = true;
        int i = 0;
        while (i < price.Length && canPay)
        {
            canPay = p.GetResources(i) >= price[i];
            i++;
        }
        return canPay;
    }
}
