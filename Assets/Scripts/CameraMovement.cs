using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.Image;

public class CameraMovement : MonoBehaviour {

    //Variables for the camera movements
    [SerializeField] private Camera cam;
    [SerializeField] private Transform original;


    void Update() {
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
            cam.transform.position = original.transform.position;
            cam.transform.localRotation = original.transform.localRotation;
        }
    }
}
