using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding{
    
    public float gCost;
    public float hCost;
    public float fCost;

    public List<GameObject> openStars;
    public List<GameObject> closedStars;

    public void FindPath(GameObject playerStar, GameObject targetStar) {
        //At start
        gCost = 0;
        hCost = Distance(playerStar, targetStar);
        fCost = CalcFCost(gCost,hCost);
    }
    
    
    private float Distance(GameObject currentStar, GameObject adjacentStar) {
        float distance = Vector3.Distance(currentStar.transform.position, adjacentStar.transform.position);
        return distance;
    }

    private float CalcFCost(float gCost, float hCost) {
        fCost = gCost + hCost;
        return fCost;
    }
}
