using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CreatingObjects : MonoBehaviour
{
    //Universe
    public float sizeUniverse;
    public int numberOfStars;
    public int planetsPerStar;

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
    Vector3[] rotations = new Vector3[]
    {
        new Vector3(1,0,0), new Vector3(0, 1, 0), new Vector3(0, 0, 1),
        new Vector3(1,1,0), new Vector3(0, 1, 1), new Vector3(1, 0, 1),new Vector3(1, 1, 1),
        new Vector3(-1,0,0), new Vector3(0, -1, 0), new Vector3(0, 0, -1),
        new Vector3(-1,-1,0), new Vector3(0, -1, -1), new Vector3(-1, 0, -1),new Vector3(-1, -1, -1),
    };
    [SerializeField] public List<Vector3> rotationDirection;
    private int rotationCounter = 0;

    //Rendering
    [SerializeField] public Material[] mat;

    void Start()
    {
        //Getting Set Up information
        sizeUniverse = SetUp.sizeUniverse;
        numberOfStars = SetUp.numberOfStars;
        planetsPerStar = SetUp.planetsPerStar;

        Creation();
        Rendering();
    }

    private void Update()
    {
        Orbiting();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(new Vector3(0,0,0) , sizeUniverse);
    }

    private void Creation()
    {
        starArray = new GameObject[numberOfStars];
        planetArray = new GameObject[planetsPerStar * numberOfStars];
        rotationDirection = new List<Vector3>();

        for (var i = 0; i < numberOfStars; i++)
        {
            var positionStar = new Vector3(Random.Range(-sizeUniverse, sizeUniverse), Random.Range(-sizeUniverse, sizeUniverse), Random.Range(-sizeUniverse, sizeUniverse));
            starArray[i] = Instantiate(star, positionStar, Quaternion.identity);
            starArray[i].transform.tag = "Star";
            starArray[i].transform.name = starNames[Random.Range(0, starNames.Length)] + " " + i;

            for (var j = 0; j < planetsPerStar; j++)
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

    private void Rendering()
    {
        foreach (var star in starArray)
        {
            star.GetComponent<Renderer>().material = mat[Random.Range(0, mat.Length)];
        }
        foreach (var planet in planetArray)
        {
            planet.GetComponent<Renderer>().material = mat[Random.Range(0, mat.Length)];
        }
    }

    private void Orbiting()
    {
        foreach (var star in starArray)
        {
            for (var i = 0; i < planetsPerStar; i++)
            {
                planetSetting = star.gameObject.transform.GetChild(i).gameObject;
                Vector3 pos = star.transform.position;
                planetSetting.transform.RotateAround(pos, rotationDirection[rotationCounter], Random.Range(5.0f, 30.0f) * Time.deltaTime);
                rotationCounter += 1;
            }
        }
        rotationCounter = 0;
    }
}
