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
    [SerializeField] private Slider sizeUniverseInput;
    [SerializeField] private Slider numberOfStarsInput;
    [SerializeField] private Slider planetsPerStarInput;

    public static float sizeUniverse;
    public static int numberOfStars;
    public static int planetsPerStar;

    //Variables for the teleportation image
    [SerializeField] private GameObject teleport;
    int timer = 0;
    bool pressed = false;

    public void StartGame() {
        //Setting the player inputs as static variable for the map
        //Size of universe switch statement
        switch(sizeUniverseInput.value) {
            case 1:
                sizeUniverse = 500;
                //Switch case for the amount of stars depending on the size of universe
                switch (numberOfStarsInput.value) {
                    case 1:
                        numberOfStars = 50;
                        break;
                    case 2:
                        numberOfStars = 100;
                        break;
                    case 3:
                        numberOfStars = 150;
                        break;
                    default:
                        break;
                }
                
                break;
            case 2:
                sizeUniverse = 1000;
                //Switch case for the amount of stars depending on the size of universe
                switch (numberOfStarsInput.value) {
                    case 1:
                        numberOfStars = 100;
                        break;
                    case 2:
                        numberOfStars = 150;
                        break;
                    case 3:
                        numberOfStars = 200;
                        break;
                    default:
                        break;
                }

                break;
            case 3:
                sizeUniverse = 2000;
                //Switch case for the amount of stars depending on the size of universe
                switch (numberOfStarsInput.value) {
                    case 1:
                        numberOfStars = 150;
                        break;
                    case 2:
                        numberOfStars = 200;
                        break;
                    case 3:
                        numberOfStars = 250;
                        break;
                    default:
                        break;
                }

                break;
            default:
                break;
        }

        //Amount of planets switch statement
        switch (planetsPerStarInput.value)
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
