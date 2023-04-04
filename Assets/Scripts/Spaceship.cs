using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class Spaceship : MonoBehaviour {
    //Ship variables
    public GameObject ship;
    List<Star> path;
    Star[] arrayPath;
    int i = 0;

    bool moving = false;

    public void SpawnShip(List<Star> shipPath) {
        path = shipPath;
        arrayPath = new Star[shipPath.Count];


        foreach (Star star in path) {
            arrayPath[i] = star;
            i++;
        }

        ship.transform.position = arrayPath[0].transform.position;
        i = 1;
        moving = true;
    }

    public void Update() {
        if (moving) {
            MoveShip(path);
        }
    }


    public void MoveShip(List<Star> shipPath) {
        if (i != arrayPath.Length) {
            ship.transform.position = Vector3.MoveTowards(transform.position, arrayPath[i].transform.position, 0.1f);
            if (ship.transform.position == arrayPath[i].transform.position) {
                i++;
            }
        }
        else {
            moving = false;
            ResetingShip();
        }
    }

    public void ResetingShip() {

        ship.transform.position = arrayPath[0].transform.position;
        i = 1;
        moving = true;
    }
}
