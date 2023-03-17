using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.Image;

public class CameraMovement : MonoBehaviour {

    //Variables for the camera movements
    [SerializeField] private Camera cam;
    private Transform original;

    public Pathfinding pathfinding;

    public GameObject playerStar;
    public GameObject targetStar;

    public void Start() {
        //Setting camera position
        cam.transform.position = new Vector3((SetUp.sizeUniverse / 2), (SetUp.sizeUniverse / 2), - SetUp.sizeUniverse);
        original = cam.transform;

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
            cam.transform.position = original.transform.position;
            cam.transform.localRotation = original.transform.localRotation;
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
    }
}
