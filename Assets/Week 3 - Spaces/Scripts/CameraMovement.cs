using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    //Variables for the camera movements
    private Vector3 cam_pos;
    [SerializeField] private float cam_speed;
    [SerializeField] private Camera cam;

    //void Start() {
    //    cam_pos = gameObject.transform.position;        //Setting the position of the camera
    //}


    private void Start() {
        cam = Camera.main;      //Setting the cam variable to be the main camera
        if (Cursor.lockState != CursorLockMode.Locked)      //Locks the cursor into the screen
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }


    void Update() {

        float mouseX = (Input.mousePosition.x / Screen.width) - 0.5f;
        float mouseY = (Input.mousePosition.y / Screen.height) - 0.5f;
        transform.localRotation = Quaternion.Euler(new Vector4(-1f * (mouseY * 180f), mouseX * 360f, transform.localRotation.z));

        

        //if (Input.GetKey(KeyCode.LeftShift)) {          //Creating a way to move camera faster
        //    cam_speed = 50;
        //}
        //else {
        //    cam_speed = 30;
        //}
        ////Code for camera movement
        //if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) {
        //    cam_pos = cam_pos + (Vector3.left * cam_speed * Time.deltaTime);
        //    gameObject.transform.position = cam_pos;
        //}
        //else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) {
        //    cam_pos = cam_pos + (Vector3.right * cam_speed * Time.deltaTime);
        //    gameObject.transform.position = cam_pos;
        //}
        //else if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) {
        //    cam_pos = cam_pos + (Vector3.forward * cam_speed * Time.deltaTime);
        //    gameObject.transform.position = cam_pos;
        //}
        //else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) {
        //    cam_pos = cam_pos + (Vector3.back * cam_speed * Time.deltaTime);
        //    gameObject.transform.position = cam_pos;
        //}
        //transform.position = cam_pos;
    }
}
