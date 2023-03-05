using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarNode{

    private float x;
    private float y;

    public float gCost;
    public float hCost;
    public float fCost;

    public StarNode previousStar;

    public StarNode(float x, float y) {
        this.x = x;
        this.y = y;
    }
}
