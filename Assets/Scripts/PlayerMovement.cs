using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.Image;

public class PlayerMovement : MonoBehaviour {

    //Variables for the camera movements
    [SerializeField] private Camera cam;
    public Vector3 original;

    [SerializeField] public Material routeMaterial;

    public Star playerStar;
    public Star targetStar;

    public void Start() {
        //Setting camera position
        cam.transform.position = new Vector3((SetUp.sizeUniverse / 1.5f), (SetUp.sizeUniverse / 1.5f), - SetUp.sizeUniverse);
        original = cam.transform.position;
    }

    void Update() {
        //Position of camera
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
        if (Input.GetMouseButtonDown(1)) {
            cam.transform.position = original;
            cam.transform.localRotation = new Quaternion(0,0,0,0);
        }
        


        //Selecting routes
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.CompareTag("Star"))
                {
                    playerStar = hit.transform.gameObject.GetComponent<Star>();
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.CompareTag("Star"))
                {
                    targetStar = hit.transform.gameObject.GetComponent<Star>();
                    List<Star> path = Pathfinding_Daryl.FindPath(playerStar, targetStar);
                    
                    foreach(Star star in path) {
                        Debug.Log(star);
                        star.GetComponent<Renderer>().material = routeMaterial;
                    }
                }
            }
        }

        //Moving Camera
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
