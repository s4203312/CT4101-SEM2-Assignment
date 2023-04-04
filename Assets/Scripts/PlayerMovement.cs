using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Android.Gradle;
using Unity.VisualScripting;
using UnityEditor.Presets;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Image;

public class PlayerMovement : MonoBehaviour {

    //Variables for the camera movements
    [SerializeField] private Camera cam;
    public Vector3 original;

    //Materials and UI variables
    [SerializeField] public Material routeMaterial;
    [SerializeField] public Material material;
    [SerializeField] public TMP_Text routeInfo;
    string information;

    //Pathfinding variables
    public Star playerStar;
    public Star targetStar;
    public int counter = 0;
    public Star preStar;
    public List<Star> path;

    public void Start() {
        //Setting camera position
        cam.transform.position = new Vector3((SetUp.sizeUniverse / 1.5f), (SetUp.sizeUniverse / 1.5f), - SetUp.sizeUniverse * 1.5f);
        original = cam.transform.position;
    }

    void Update() {
        //Position of camera
        //If left mouse button is pressed while looking at star change take camera to that star
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                if (hit.transform.CompareTag("Star")) {

                    cam.transform.position = hit.transform.GetChild(0).transform.position;
                    cam.transform.localRotation = hit.transform.GetChild(0).transform.localRotation;
                }
            }
        }
        //If right mouse button is pressed take camera back to original position
        if (Input.GetMouseButtonDown(1)) {
            cam.transform.position = original;
            cam.transform.localRotation = new Quaternion(0,0,0,0);
        }
        


        //Selecting routes
        //If Z is pressed while looking at a star then set that star as the starting star
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.CompareTag("Star"))
                {
                    playerStar = hit.transform.gameObject.GetComponent<Star>();
                    hit.transform.gameObject.GetComponent<AudioSource>().Play();        //Playing a sound so player knows that input has been recorded
                }
            }
        }
        // If X is pressed while looking at a star then set that star as the ending star ands perform the pathfinding
        if (Input.GetKeyDown(KeyCode.X))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.CompareTag("Star"))
                {
                    targetStar = hit.transform.gameObject.GetComponent<Star>();
                    hit.transform.gameObject.GetComponent<AudioSource>().Play();         //Playing a sound so player knows that input has been recorded

                    //Performing the pathfinding
                    path = Pathfinding_Daryl.FindPath(playerStar, targetStar);

                    //Reseting the previous route colours to white
                    GameObject[] preRoutes = GameObject.FindGameObjectsWithTag("Route");
                    foreach(GameObject route in preRoutes) {
                        route.gameObject.GetComponent<Renderer>().material = material;
                    }

                    //Setting title
                    information = "Selected Route \n \n";

                    //Changing the colour of the route
                    foreach (Star star in path) {

                        information += star.name + "\n"; 

                        if(counter != 0) {
                            foreach (GameObject route in star.routes) {
                                if (route.gameObject.name == "RouteTo " + preStar.gameObject.name || route.gameObject.name == "RouteTo " + star.gameObject.name) {
                                    route.gameObject.GetComponent<Renderer>().material = routeMaterial;
                                }
                            }
                            foreach (GameObject route in preStar.routes) {
                                if (route.gameObject.name == "RouteTo " + preStar.gameObject.name || route.gameObject.name == "RouteTo " + star.gameObject.name) {
                                    route.gameObject.GetComponent<Renderer>().material = routeMaterial;
                                }
                            }
                        }
                        preStar = star;
                        counter += 1;
                    }
                    //Updating UI for the route
                    routeInfo.gameObject.SetActive(true);
                    routeInfo.SetText(information);

                    //Setting the spaceship to move along the route
                    Spaceship spaceship = GameObject.Find("Ship").GetComponent<Spaceship>();
                    if (path != null) {
                        spaceship.SpawnShip(path);
                    }

                    counter = 0;
                }
            }
        }

        //Rotating Camera
        if (Input.GetKeyDown(KeyCode.W)) {
            cam.transform.Rotate(-5, 0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.A)) {
            cam.transform.Rotate(0, -5, 0);
        }
        else if(Input.GetKeyDown(KeyCode.S)) {
            cam.transform.Rotate(5, 0, 0);
        }
        else if(Input.GetKeyDown(KeyCode.D)) {
            cam.transform.Rotate(0, 5, 0);
        }
        //Zooming In and Out
        else if (Input.GetKeyDown(KeyCode.Q)) {
            cam.transform.position += new Vector3(0, 0, 15);
            original = cam.transform.position;
        }
        else if (Input.GetKeyDown(KeyCode.E)) {
            cam.transform.position += new Vector3(0, 0, -15);
            original = cam.transform.position;
        }
    }
}
