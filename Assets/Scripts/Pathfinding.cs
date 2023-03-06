using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Pathfinding{

    public float gCost;
    public float hCost;
    public float fCost;

    public GameObject currentStar;

    public List<GameObject> pathStars;

    public Collider[] neighbours;
    public List<float> neighboursFCosts;
    public GameObject correctNeighbour;

    public void FindPath(GameObject playerStar, GameObject targetStar) {
        
        Debug.Log("Starting");
        
        //At start
        gCost = 0;
        hCost = Distance(playerStar, targetStar);
        fCost = CalcFCost(gCost,hCost);
        currentStar = playerStar;
        
        Debug.Log(fCost);

        //Looping unitl path found

        neighboursFCosts = new List<float>();
        pathStars = new List<GameObject>();

        while (currentStar != targetStar) {
            neighbours = Physics.OverlapSphere(currentStar.transform.position, 300);
            if (neighbours != null) {
                //Calculating neighbour h,g,f costs
                foreach (Collider neighbour in neighbours) {
                    hCost = Distance(currentStar, neighbour.gameObject);
                    gCost = /*currentStar.gCost + */ hCost;
                    fCost = CalcFCost(gCost, hCost);
                    Debug.Log(fCost);
                    neighboursFCosts.Add(fCost);
                }
                //Finding best neighbour
                float minValue = neighboursFCosts.Min();
                int positionList = neighboursFCosts.IndexOf(minValue);
                correctNeighbour = neighbours[positionList].gameObject;
                //Clearing neighbours
                neighboursFCosts.Clear();
                //Adding to the correct path
                pathStars.Add(correctNeighbour);
                currentStar = correctNeighbour;
            }
            else {
                Debug.Log("No neighbours");
                break;
            }
        }

        //Drawing path
        foreach (GameObject star in pathStars) {
            Debug.Log(star);
            //Gizmos.DrawLine(star,//the next star??)        
        }
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

//function reconstruct_path(cameFrom, current)
//    total_path:= { current}
//while current in cameFrom.Keys:
//        current:= cameFrom[current]
//        total_path.prepend(current)
//    return total_path

//// A* finds a path from start to goal.
//// h is the heuristic function. h(n) estimates the cost to reach goal from node n.
//function A_Star(start, goal, h)
//    // The set of discovered nodes that may need to be (re-)expanded.
//    // Initially, only the start node is known.
//    // This is usually implemented as a min-heap or priority queue rather than a hash-set.
//    openSet:= { start}

//// For node n, cameFrom[n] is the node immediately preceding it on the cheapest path from the start
//// to n currently known.
//cameFrom:= an empty map

//    // For node n, gScore[n] is the cost of the cheapest path from start to n currently known.
//    gScore := map with default value of Infinity
//    gScore[start] := 0

//    // For node n, fScore[n] := gScore[n] + h(n). fScore[n] represents our current best guess as to
//    // how cheap a path could be from start to finish if it goes through n.
//    fScore:= map with default value of Infinity
//    fScore[start] := h(start)

//    while openSet is not empty
//        // This operation can occur in O(Log(N)) time if openSet is a min-heap or a priority queue
//        current := the node in openSet having the lowest fScore[] value
//        if current = goal
//            return reconstruct_path(cameFrom, current)

//        openSet.Remove(current)
//        for each neighbor of current
//            // d(current,neighbor) is the weight of the edge from current to neighbor
//            // tentative_gScore is the distance from start to the neighbor through current
//            tentative_gScore := gScore[current] + d(current, neighbor)
//            if tentative_gScore < gScore[neighbor]
//                // This path to neighbor is better than any previous one. Record it!
//                cameFrom[neighbor] := current
//                gScore[neighbor] := tentative_gScore
//                fScore[neighbor] := tentative_gScore + h(neighbor)
//                if neighbor not in openSet
//                    openSet.add(neighbor)

//    // Open set is empty but goal was never reached
//    return failure
