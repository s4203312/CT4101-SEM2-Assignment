using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SetUp : MonoBehaviour
{
    //Variables for the player inputs
    [SerializeField] private TMP_InputField sizeUniverseInput;
    [SerializeField] private TMP_InputField numberOfStarsInput;
    [SerializeField] private TMP_InputField planetsPerStarInput;
    
    [SerializeField] private Slider sizeUniverseInput1;
    [SerializeField] private Slider numberOfStarsInput1;
    [SerializeField] private Slider planetsPerStarInput1;

    public static float sizeUniverse;
    public static int numberOfStars;
    public static int planetsPerStar;

    //Variables for the teleportation image
    [SerializeField] private GameObject teleport;
    int timer = 0;
    bool pressed = false;

    public void StartGame() {
        //Setting the player inputs as static variable for the map
        //sizeUniverse = float.Parse(sizeUniverseInput.text);
        numberOfStars = int.Parse(numberOfStarsInput.text);
        //planetsPerStar = int.Parse(planetsPerStarInput.text);

        //Size of universe switch statement
        switch(sizeUniverseInput1.value) {
            case 1:
                sizeUniverse = 500;
                    break;
            case 2:
                sizeUniverse = 1000;
                    break;
            case 3:
                sizeUniverse = 2000;
                    break;
            default:
                break;
        }

        //Amount of planets switch statement
        switch (planetsPerStarInput1.value)
        {
            case 1:
                planetsPerStar = 1;
                break;
            case 2:
                planetsPerStar = 3;
                break;
            case 3:
                planetsPerStar = 5;
                break;
            default:
                break;
        }


        //Starting the teleport animation
        teleport.SetActive(true);
        teleport.GetComponent<Animator>().SetTrigger("teleport");
        GetComponent<AudioSource>().Play();                         //Playing a teleporting sound
        pressed = true;
        
    }
    public void Update() {
        if (pressed) {
            timer++;
        }
        //Loading the level scene after update has been called 1000 times
        if(timer == 1000) {
            pressed = false;
            SceneManager.LoadScene("StarMap");
        }
    }
}
