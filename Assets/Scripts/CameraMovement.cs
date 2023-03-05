using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.Image;

public class CameraMovement : MonoBehaviour {

    //Variables for the camera movements
    [SerializeField] private Camera cam;
    [SerializeField] private Transform original;

    public Pathfinding pathfinding;

    public GameObject playerStar;
    public GameObject targetStar;


    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                if (hit.transform.CompareTag("Star")) {
                    
                    playerStar = hit.transform.gameObject;

                    //cam.transform.position = hit.transform.GetChild(0).transform.position;
                    //cam.transform.localRotation = hit.transform.GetChild(0).transform.localRotation;
                }
            }
        }
        if (Input.GetMouseButtonDown(1)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                if (hit.transform.CompareTag("Star")) {

                    targetStar = hit.transform.gameObject;
                    pathfinding.FindPath(playerStar, targetStar);
                }
            }




                    //cam.transform.position = original.transform.position;
                    //cam.transform.localRotation = original.transform.localRotation;
        }
    }
}
