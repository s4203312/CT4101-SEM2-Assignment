using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SetUp : MonoBehaviour
{
    [SerializeField] private TMP_InputField sizeUniverseInput;
    [SerializeField] private TMP_InputField numberOfStarsInput;
    [SerializeField] private TMP_InputField planetsPerStarInput;

    [SerializeField] private GameObject teleport;

    public static float sizeUniverse;
    public static int numberOfStars;
    public static int planetsPerStar;

    int timer = 0;
    bool pressed = false;

    public void StartGame() {
        sizeUniverse = float.Parse(sizeUniverseInput.text);
        numberOfStars = int.Parse(numberOfStarsInput.text);
        planetsPerStar = int.Parse(planetsPerStarInput.text);

        teleport.SetActive(true);
        teleport.GetComponent<Animator>().SetTrigger("teleport");
        GetComponent<AudioSource>().Play();
        pressed = true;
        
    }
    public void Update() {
        if (pressed) {
            timer++;
        }
        if(timer == 1000) {
            pressed = false;
            SceneManager.LoadScene("StarMap");
        }
    }
}
