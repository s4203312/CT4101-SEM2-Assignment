using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarNode {

    public float gCost;
    public float hCost;
    public float fCost;

    public StarNode previousStar;

    public GameObject currentStar;
}
