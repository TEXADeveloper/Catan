using System.Collections.Generic;
using UnityEngine;

public class RoadICard : ICard
{
    private Queue<int> turnObtained = new Queue<int>();
    private int amount;
    public int Amount
    {
        get => amount;
        set => amount = value;
    }


    public void Action()
    {
        Debug.Log("Road");
    }
}
