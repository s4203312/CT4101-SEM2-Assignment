using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controls : MonoBehaviour
{
    //Script used for opening and closing the controls panel on main menu and star map
    public void OpenControls() {
        gameObject.SetActive(true);
    }
    public void CloseControls() {
        gameObject.SetActive(false);
    }
    public void EscapeGame() {
        Application.Quit();
    }
    public void MainMenu() {
        SceneManager.LoadScene("Menu");
    }
}
