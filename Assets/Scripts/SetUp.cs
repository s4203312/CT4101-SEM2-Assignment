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

    public static float sizeUniverse;
    public static int numberOfStars;
    public static int planetsPerStar;

    public void StartGame() {
        sizeUniverse = float.Parse(sizeUniverseInput.text);
        numberOfStars = int.Parse(numberOfStarsInput.text);
        planetsPerStar = int.Parse(planetsPerStarInput.text);

        SceneManager.LoadScene("StarMap");
    }
}
