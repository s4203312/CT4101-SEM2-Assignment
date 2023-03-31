using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class CreatingObjects : MonoBehaviour {
    //Universe
    public float sizeUniverse;
    public int numberOfStars;
    public int planetsPerStar;
    private static List<Star> galaxyStarList;

    //Stars
    [SerializeField] public GameObject star;
    [SerializeField] public Star[] starArray;
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
    Star currentSObject;

    //Performing checks
    public bool starChecked;

    void Start() {
        //Getting Set Up information
        sizeUniverse = SetUp.sizeUniverse;
        numberOfStars = SetUp.numberOfStars;
        planetsPerStar = SetUp.planetsPerStar;

        Creation();
        Rendering();
    }

    private void Update() {
        Orbiting();
    }

    private void Creation() {
        starArray = new Star[numberOfStars];
        planetArray = new GameObject[planetsPerStar * numberOfStars];
        rotationDirection = new List<Vector3>();
        galaxyStarList = new List<Star>();

        for (var i = 0; i < numberOfStars; i++) {
            var positionStar = new Vector3(Random.Range(0, sizeUniverse), Random.Range(0, sizeUniverse), Random.Range(0, sizeUniverse));

            CheckingSpawn(positionStar);
            if (starChecked) {
                starArray[i] = Instantiate(star, positionStar, Quaternion.identity).GetComponent<Star>();
                starArray[i].transform.tag = "Star";
                starArray[i].transform.name = starNames[Random.Range(0, starNames.Length)];
                starArray[i].starName = starArray[i].transform.name;
                galaxyStarList.Add(starArray[i]);

                for (var j = 0; j < planetsPerStar; j++) {
                    var positionPlanet = new Vector3(Random.Range(-100.0f, 100.0f), Random.Range(-100.0f, 100.0f), Random.Range(-100.0f, 100.0f));
                    planetArray[planetCounter] = Instantiate(planet, positionPlanet + positionStar, Quaternion.identity, starArray[i].transform);
                    planetArray[planetCounter].transform.tag = "Planet";
                    planetArray[planetCounter].transform.name = planetNames[Random.Range(0, planetNames.Length)];
                    planetCounter += 1;
                    rotationDirection.Add(rotations[Random.Range(0, rotations.Length)]);
                }
            }
            else {
                i--;
            }
        }
        Pathfinding_Daryl.SetGalaxyStarList(galaxyStarList);
        CreateRoutes();
    }

    private void Rendering() {
        foreach (var star in starArray) {
            star.GetComponent<Renderer>().material = mat[Random.Range(0, mat.Length)];
        }
        foreach (var planet in planetArray) {
            planet.GetComponent<Renderer>().material = mat[Random.Range(0, mat.Length)];
        }
    }

    private void Orbiting() {
        foreach (var star in starArray) {
            for (var i = 0; i <= planetsPerStar; i++) {
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
        if (nStars.Length == 0) {
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

        currentSDistance = int.MaxValue;

        foreach (Star star in starArray) {
            Collider[] nStars = Physics.OverlapSphere(star.transform.position, sizeUniverse / 4);
            if (nStars.Length != 1) {
                foreach (Collider i in nStars) {
                    if (i.transform != star.transform) {
                        if (i.TryGetComponent<Star>(out Star otherStar)) {
                            if (nStars.Length == 2) {
                                routes = GameObject.Instantiate(route, new Vector3(0, 0, 0), Quaternion.identity);
                                routes.GetComponent<RouteMesh>().Generate(star.transform.position, i.gameObject.transform.position);
                                SaveRoutes(star, otherStar);
                            }
                            else if (Random.Range(1, 4) == 1) {
                                routes = GameObject.Instantiate(route, new Vector3(0, 0, 0), Quaternion.identity);
                                routes.GetComponent<RouteMesh>().Generate(star.transform.position, i.gameObject.transform.position);
                                SaveRoutes(star, otherStar);
                            }
                        }
                    }
                }
            }

            else {
                Collider[] n2Stars = Physics.OverlapSphere(star.transform.position, sizeUniverse);
                if (n2Stars != null) {
                    foreach (Collider j in n2Stars) {
                        if (j.transform != star.transform) {
                            if (j.TryGetComponent<Star>(out Star otherStar)) {
                                float distance = Vector3.Distance(j.gameObject.transform.position, star.transform.position);
                                if (distance < currentSDistance) {
                                    currentSDistance = distance;
                                    currentSObject = otherStar;
                                }
                            }
                        }
                    }
                    if (currentSObject != null) {
                        routes = GameObject.Instantiate(route, new Vector3(0, 0, 0), Quaternion.identity);
                        routes.GetComponent<RouteMesh>().Generate(star.transform.position, currentSObject.gameObject.transform.position);
                        SaveRoutes(star, currentSObject);
                    }
                }
                currentSDistance = int.MaxValue;
            }
        }
    }

    //Saving the routes into the star information
    public void SaveRoutes(Star star, Star otherStar) {

        float distanceStars = (star.transform.position - otherStar.transform.position).magnitude;

        if (!star.starRoutes.ContainsKey(otherStar)) {
            star.starRoutes.Add(otherStar, distanceStars);
            Debug.Log("Route created from " + star.name + " to " + otherStar.name);
        }

        if (!otherStar.starRoutes.ContainsKey(star)) {
            otherStar.starRoutes.Add(star, distanceStars);
        }
    }
}
        
    

