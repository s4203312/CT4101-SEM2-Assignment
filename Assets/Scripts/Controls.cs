using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    //Script used for opening and closing the controls panel on main menu and star map
    public void OpenControls() {
        gameObject.SetActive(true);
    }
    public void CloseControls() {
        gameObject.SetActive(false);
    }
}
