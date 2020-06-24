using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candy : MonoBehaviour
{
    [SerializeField]
    private string candyName;
    [SerializeField]
    private int candyPoint;
    [SerializeField]
    private string candyColour;

    public string GetCandyName()
    {
        return this.candyName;
    }
    public int GetCandyPoint()
    {
        return this.candyPoint;
    }
    public string GetCandyColour()
    {
        return this.candyColour;
    }

}
