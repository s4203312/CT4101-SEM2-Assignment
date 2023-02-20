using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatingObjects : MonoBehaviour
{
    //Stars
    [SerializeField] public GameObject star;
    [SerializeField] public GameObject[] starArray;
    [SerializeField] public string[] starNames;

    //Planets
    [SerializeField] public GameObject planet;
    [SerializeField] public GameObject[] planetArray;
    [SerializeField] public string[] planetNames;
    private int planetCounter = 0;

    //Orbiting
    [SerializeField] public float orbitSpeed;
    private GameObject planetSetting;
    Vector3[] rotations = new Vector3[] { Vector3.up, Vector3.left, Vector3.right, Vector3.down };
    [SerializeField] public List<Vector3> rotationDirection;
    private int rotationCounter = 0;

    //Rendering
    [SerializeField] public Material[] mat;

    void Start()
    {
        Creation();
        Rendering();
    }

    private void Update() 
    {
        Orbiting();
    }

    private void Creation()
    {
        starArray = new GameObject[10];
        planetArray = new GameObject[30];
        rotationDirection = new List<Vector3>();

        for (var i = 0; i < 10; i++)
        {
            var positionStar = new Vector3(Random.Range(-1000.0f, 1000.0f), Random.Range(-1000.0f, 1000.0f), Random.Range(-1000.0f, 1000.0f));
            starArray[i] = Instantiate(star, positionStar, Quaternion.identity);
            starArray[i].transform.tag = "Star";
            starArray[i].transform.name = starNames[Random.Range(0, starNames.Length)] + " " + i;

            for (var j = 0; j < 3; j++)
            {
                var positionPlanet = new Vector3(Random.Range(-100.0f, 100.0f), Random.Range(-100.0f, 100.0f), Random.Range(-100.0f, 100.0f));
                planetArray[planetCounter] = Instantiate(planet, positionPlanet + positionStar, Quaternion.identity, starArray[i].transform);
                planetArray[planetCounter].transform.tag = "Planet";
                planetArray[planetCounter].transform.name = planetNames[Random.Range(0, planetNames.Length)];
                planetCounter += 1;
                rotationDirection.Add(rotations[Random.Range(0, rotations.Length)]);
            }
        }
    }

    private void Rendering() {
        foreach (var star in starArray) {
            star.GetComponent<Renderer>().material = mat[Random.Range(0, mat.Length)];
        }
        foreach (var planet in planetArray) {
            planet.GetComponent<Renderer>().material = mat[Random.Range(0, mat.Length)];
        }
    }

    private void Orbiting() 
    {
        foreach (var star in starArray) {
            for(var i = 0; i < 3; i++) {
                planetSetting = star.gameObject.transform.GetChild(i).gameObject;
                Vector3 pos = star.transform.localPosition;
                planetSetting.transform.RotateAround(pos, rotationDirection[rotationCounter], Random.Range(5.0f,30.0f) * Time.deltaTime);
                rotationCounter += 1;
            }
        }
        rotationCounter = 0;
    }
}
