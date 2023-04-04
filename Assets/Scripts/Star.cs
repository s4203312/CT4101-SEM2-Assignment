using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Star : MonoBehaviour {

    //Star class used to store useful information about the individual stars

    public string starName;
    public Dictionary<Star, float> starRoutes = new Dictionary<Star, float>();      //All other stars it is connected with and the distance away used for pathfinding
    public List<GameObject> routes;                                                 //All routes that this star has
}
