using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

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

    //RouteCreating
    public GameObject previousStar;
    public GameObject route;
    public GameObject routes;
    
    float currentSDistance;
    GameObject currentSObject;

    //Performing checks
    public bool starChecked;

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

    private void Creation()
    {
        starArray = new GameObject[numberOfStars];
        planetArray = new GameObject[planetsPerStar * numberOfStars];
        rotationDirection = new List<Vector3>();

        for (var i = 0; i < numberOfStars; i++)
        {
            var positionStar = new Vector3(Random.Range(0, sizeUniverse), Random.Range(0, sizeUniverse), Random.Range(0, sizeUniverse));
            
            CheckingSpawn(positionStar);
            if (starChecked) {
                starArray[i] = Instantiate(star, positionStar, Quaternion.identity);
                starArray[i].transform.tag = "Star";
                starArray[i].transform.name = starNames[Random.Range(0, starNames.Length)];

                for (var j = 0; j < planetsPerStar; j++) {
                    var positionPlanet = new Vector3(Random.Range(-100.0f, 100.0f), Random.Range(-100.0f, 100.0f), Random.Range(-100.0f, 100.0f));
                    planetArray[planetCounter] = Instantiate(planet, positionPlanet + positionStar, Quaternion.identity, starArray[i].transform);
                    planetArray[planetCounter].transform.tag = "Planet";
                    planetArray[planetCounter].transform.name = planetNames[Random.Range(0, planetNames.Length)];
                    planetCounter += 1;
                    rotationDirection.Add(rotations[Random.Range(0, rotations.Length)]);
                }

                //Routes

                //if (previousStar != null) {
                //    routes = GameObject.Instantiate(route, new Vector3(0, 0, 0), Quaternion.identity);
                //    routes.GetComponent<RouteMesh>().Generate(starArray[i].transform.position, previousStar.transform.position);
                //    Debug.Log(i);
                //}
                //previousStar = starArray[i];
            }
            else {
                i--;
            }
        }

        //Routes better?
        CreateRoutes();
        //CreateRoutesAgain();

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
            for (var i = 0; i <= planetsPerStar; i++)
            {
                planetSetting = star.gameObject.transform.GetChild(i).gameObject;
                if (planetSetting.tag != "Cam") {
                    Vector3 pos = star.transform.position;
                    planetSetting.transform.RotateAround(pos, rotationDirection[rotationCounter], Random.Range(5.0f, 30.0f) * Time.deltaTime);
                    rotationCounter += 1;
                } 
            }
        }
        rotationCounter = 0;
    }

    //Checks that no stars are overlapping
    private void CheckingSpawn(Vector3 position) {
        Collider[] nStars = Physics.OverlapSphere(position, 50);
        if(nStars.Length == 0) {
            starChecked = true;
            return;
        }
        else {
            starChecked = false;
            return;
        }
    }

    //Creates routes
    private void CreateRoutes() {

        currentSDistance = 100000;

        foreach (GameObject star in starArray) {
            Collider[] nStars = Physics.OverlapSphere(star.transform.position, 300);
            if (nStars.Length != 1) {
                foreach (Collider i in nStars) {
                    if (i.transform != star.transform) {
                        if (i.gameObject.transform.tag != "Planet") {
                            if (nStars.Length == 2) {
                                routes = GameObject.Instantiate(route, new Vector3(0, 0, 0), Quaternion.identity);
                                routes.GetComponent<RouteMesh>().Generate(star.transform.position, i.gameObject.transform.position);
                            }
                            else if (Random.Range(1, 4) == 1) {
                                routes = GameObject.Instantiate(route, new Vector3(0, 0, 0), Quaternion.identity);
                                routes.GetComponent<RouteMesh>().Generate(star.transform.position, i.gameObject.transform.position);

                            }
                        }
                    }
                }
            }

            else {
                Collider[] n2Stars = Physics.OverlapSphere(star.transform.position, sizeUniverse);
                if (n2Stars != null) {
                    foreach (Collider i in n2Stars) {
                        if (i.transform != star.transform) {
                            if (i.gameObject.transform.tag != "Planet") {
                                float distance = Vector3.Distance(i.gameObject.transform.position, star.transform.position);
                                if (distance < currentSDistance) {
                                    currentSDistance = distance;
                                    currentSObject = i.gameObject;
                                }
                            }
                        }
                    }
                    if (currentSObject != null) {
                        routes = GameObject.Instantiate(route, new Vector3(0, 0, 0), Quaternion.identity);
                        routes.GetComponent<RouteMesh>().Generate(star.transform.position, currentSObject.gameObject.transform.position);
                    }
                }
                currentSDistance = 100000;
            }
        }
    }

    //Creates routes
    private void CreateRoutesAgain() {

        float currentSDistance = 100000;
        
        foreach (GameObject star in starArray) {
            Collider[] nStars = Physics.OverlapSphere(star.transform.position, sizeUniverse);
            if (nStars != null) {
                foreach (Collider i in nStars) {
                    if (i.transform != star.transform) {
                        if (i.gameObject.transform.tag != "Planet") {
                            float distance = Vector3.Distance(i.gameObject.transform.position, star.transform.position);
                            if (distance < currentSDistance) {
                                currentSDistance = distance;
                                currentSObject = i.gameObject;
                            }
                        }
                    }
                }
                routes = GameObject.Instantiate(route, new Vector3(0, 0, 0), Quaternion.identity);
                routes.GetComponent<RouteMesh>().Generate(star.transform.position, currentSObject.gameObject.transform.position);
            }
            currentSDistance = 100000;
        }
    }
}
        
    

