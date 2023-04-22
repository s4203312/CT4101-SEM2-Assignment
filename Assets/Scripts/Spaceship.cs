using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor.Experimental;
using UnityEngine;

public class Spaceship : MonoBehaviour {
    
    //Ship variables
    public GameObject ship;
    List<Star> path;
    Star[] arrayPath;
    public int i = 0;
    public bool moving = false;

    public bool lightSwitch;
    public int lightCounter;

    //Function for setting the route array and to start the spaceship at ther first star
    public void SpawnShip(List<Star> shipPath) {
        path = shipPath;
        arrayPath = new Star[shipPath.Count];

        //Setting up the route
        foreach (Star star in path) {
            arrayPath[i] = star;
            i++;
        }

        ship.transform.position = arrayPath[0].transform.position;
        i = 1;
        moving = true;
    }

    //Moving ship every frame if moving is true
    public void Update() {
        if (moving) {
            MoveShip(path);
            LightFlashing();
        }
    }

    //Moving ship torwards the star unitl it arrives then setting it to move torwards the next star
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

    //Reseting its position back to the start when it arrvies at its destination
    public void ResetingShip() {

        ship.transform.position = arrayPath[0].transform.position;
        i = 1;
        moving = true;
    }

    //For flashing the light
    public void LightFlashing() {
        if(lightCounter == 300) {
            if (lightSwitch) {
                lightSwitch = false;
            }
            else {
                lightSwitch = true;
            }
            ship.transform.GetChild(7).gameObject.SetActive(lightSwitch);
            lightCounter = 0;
        }
        lightCounter++;
    }
}
