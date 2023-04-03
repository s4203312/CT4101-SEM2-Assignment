using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class Spaceship : MonoBehaviour
{
    //Ship variables
    public GameObject ship;
    int starCount = 0;
    int currentCount = 0;
    List<Star> path;
    Star currentStar;

    bool moving = false;

    public void SpawnShip(List<Star> shipPath) {
        path = shipPath;

        foreach (Star star in path) {
            if (currentCount == 0) {
                ship.transform.position = star.transform.position;
                currentStar = star;
                currentCount++;
                starCount++;
            }
        }
        Debug.Log("Calling MoveShip");
        moving = true;
    }

    public void Update() {
        if (moving) {
            MoveShip(path);
        }
    }


    public void MoveShip(List<Star> shipPath) {

        path = shipPath;
        Debug.Log("Looping stars");
        if (starCount == currentCount) {
            foreach (Star star in path) {
                if (path.IndexOf(star) != 1) {
                    currentCount++;
                    currentStar = star;
                }
            }
            ship.transform.position = Vector3.MoveTowards(transform.position, currentStar.transform.position, 60);
            if (ship.transform.position == currentStar.transform.position) {
                Debug.Log("Setting arrived");
                starCount++;
            }
        }
        if(path.Count == starCount) {
            moving = false;
            DestroyShip();
        }
    }
    public void DestroyShip() {
        Debug.Log("Destroying");
        Destroy(ship);
    }
}
