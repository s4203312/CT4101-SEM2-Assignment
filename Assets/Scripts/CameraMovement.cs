using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.Image;

public class CameraMovement : MonoBehaviour {

    //Variables for the camera movements
    [SerializeField] private Camera cam;
    public Vector3 original;

    public Pathfinding pathfinding;

    public GameObject playerStar;
    public GameObject targetStar;

    public void Start() {
        //Setting camera position
        cam.transform.position = new Vector3((SetUp.sizeUniverse / 1.5f), (SetUp.sizeUniverse / 1.5f), - SetUp.sizeUniverse);
        original = cam.transform.position;

        //Defining pathfinding
        pathfinding = new Pathfinding();
    }

    void Update() {
        //Position of camera
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                if (hit.transform.CompareTag("Star")) {
                    
                    playerStar = hit.transform.gameObject;

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
                    playerStar = hit.transform.gameObject;
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
                    targetStar = hit.transform.gameObject;
                    pathfinding.FindPath(playerStar, targetStar);
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
    }
}
