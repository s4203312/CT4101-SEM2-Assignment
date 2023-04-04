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
    public List<GameObject> starRoutes;
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
        //Predefining all of my arrays and lists
        starArray = new Star[numberOfStars];
        planetArray = new GameObject[planetsPerStar * numberOfStars];
        rotationDirection = new List<Vector3>();
        galaxyStarList = new List<Star>();
        starRoutes = new List<GameObject>();

        for (var i = 0; i < numberOfStars; i++) {
            var positionStar = new Vector3(Random.Range(0, sizeUniverse), Random.Range(0, sizeUniverse), Random.Range(0, sizeUniverse));        //Randomly finds position in universe

            CheckingSpawn(positionStar);                    //Checks if the position is good
            if (starChecked) {
                //Instantiates a star with a tag and a random name. Adds star to the galaxy list for pathfinding
                starArray[i] = Instantiate(star, positionStar, Quaternion.identity).GetComponent<Star>();
                starArray[i].transform.tag = "Star";
                starArray[i].transform.name = starNames[Random.Range(0, starNames.Length)] + " " + i;         //Naming the stars with a name and random number to differeniate between same name stars
                starArray[i].starName = starArray[i].transform.name;
                galaxyStarList.Add(starArray[i]);

                for (var j = 0; j < planetsPerStar; j++) {
                    //Instantiates a planet with a tag and a random name.
                    var positionPlanet = new Vector3(Random.Range(-100.0f, 100.0f), Random.Range(-100.0f, 100.0f), Random.Range(-100.0f, 100.0f));
                    planetArray[planetCounter] = Instantiate(planet, positionPlanet + positionStar, Quaternion.identity, starArray[i].transform);
                    planetArray[planetCounter].transform.tag = "Planet";
                    planetArray[planetCounter].transform.name = planetNames[Random.Range(0, planetNames.Length)];
                    planetCounter += 1;
                    rotationDirection.Add(rotations[Random.Range(0, rotations.Length)]);                        //Finding random rotation for the planet to rotate around star with
                }
            }
            else {
                i--;
            }
        }
        Pathfinding_Daryl.SetGalaxyStarList(galaxyStarList);            //Sets the star list when all stars have been created
        CreateRoutes();                                                 //Creates routes between all of the stars
    }

    //Creates random colours for the stars and planets
    private void Rendering() {
        foreach (var star in starArray) {
            star.GetComponent<Renderer>().material = mat[Random.Range(0, mat.Length)];
        }
        foreach (var planet in planetArray) {
            planet.GetComponent<Renderer>().material = mat[Random.Range(0, mat.Length)];
        }
    }

    //Sets the rotations of the planets around the stars
    private void Orbiting() {
        foreach (var star in starArray) {
            for (var i = 0; i <= planetsPerStar; i++) {
                planetSetting = star.gameObject.transform.GetChild(i).gameObject;
                if (planetSetting.tag != "Cam") {                                   //Disallowing the rotation of the preview camera
                    Vector3 pos = star.transform.position;
                    planetSetting.transform.RotateAround(pos, rotationDirection[rotationCounter], Random.Range(5.0f, 30.0f) * Time.deltaTime);      //Setting a random rotation direction
                    rotationCounter += 1;
                }
            }
        }
        rotationCounter = 0;
    }

    //Checks that no stars are overlapping
    private void CheckingSpawn(Vector3 position) {
        Collider[] nStars = Physics.OverlapSphere(position, 50);            //Doesnt allow stars to spawn within 50 of eachother
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

        currentSDistance = int.MaxValue;                //Resets the current closest distance

        foreach (Star star in starArray) {
            Collider[] nStars = Physics.OverlapSphere(star.transform.position, sizeUniverse / 4);       //Initial small overlap sphere to check if star has any close neighbours
            if (nStars.Length != 1) {                                                                   //Making sure it isnt just finding itself
                foreach (Collider i in nStars) {
                    if (i.transform != star.transform) {                                                //Ignoring itself
                        if (i.TryGetComponent<Star>(out Star otherStar)) {                              //Ignoring all other objects other than stars
                            if (nStars.Length == 2) {                                                   //Checking if it only has on neighbour
                                routes = GameObject.Instantiate(route, new Vector3(0, 0, 0), Quaternion.identity);
                                routes.gameObject.name = "RouteTo " + otherStar.gameObject.name;                        //Names the routes referencing the stars for later use
                                routes.GetComponent<RouteMesh>().Generate(star.transform.position, i.gameObject.transform.position);
                                SaveRoutes(star, otherStar, routes);                                            //Saving route for pathfinding uses
                            }
                            else if (Random.Range(1, 4) == 1) {                                         //Giving a random 1 in 3 chance that it spawns a route if star has more than 1 neighbouring star
                                routes = GameObject.Instantiate(route, new Vector3(0, 0, 0), Quaternion.identity);
                                routes.gameObject.name = "RouteTo " + otherStar.gameObject.name;                        //Names the routes referencing the stars for later use
                                routes.GetComponent<RouteMesh>().Generate(star.transform.position, i.gameObject.transform.position);
                                SaveRoutes(star, otherStar, routes);                                            //Saving route for pathfinding uses
                            }
                        }
                    }
                }
            }

            else {
                Collider[] n2Stars = Physics.OverlapSphere(star.transform.position, sizeUniverse);      //If no star are found in initial check then closest one is found using bigger check
                if (n2Stars != null) {
                    foreach (Collider j in n2Stars) {
                        if (j.transform != star.transform) {
                            if (j.TryGetComponent<Star>(out Star otherStar)) {
                                float distance = Vector3.Distance(j.gameObject.transform.position, star.transform.position);        //Finding closest neighbouring star
                                if (distance < currentSDistance) {
                                    currentSDistance = distance;
                                    currentSObject = otherStar;
                                }
                            }
                        }
                    }
                    if (currentSObject != null) {
                        routes = GameObject.Instantiate(route, new Vector3(0, 0, 0), Quaternion.identity);
                        routes.gameObject.name = "RouteTo " + currentSObject.gameObject.name;                       //Names the routes referencing the stars for later use
                        routes.GetComponent<RouteMesh>().Generate(star.transform.position, currentSObject.gameObject.transform.position);
                        SaveRoutes(star, currentSObject, routes);                                               //Saving route for pathfinding uses
                    }
                }
                currentSDistance = int.MaxValue;        //Reseting the distance for the next star
            }
        }
    }

    //Saving the routes into the star information
    public void SaveRoutes(Star star, Star otherStar, GameObject routes) {

        float distanceStars = (star.transform.position - otherStar.transform.position).magnitude;       //Finding distance between stars

        if (!star.starRoutes.ContainsKey(otherStar)) {                                                  //Add the other star and the distance of route into my dictionary
            star.starRoutes.Add(otherStar, distanceStars);
        }
        if (!otherStar.starRoutes.ContainsKey(star)) {                                                  //Add the star and the distance of route into my dictionary
            otherStar.starRoutes.Add(star, distanceStars);
        }

        star.routes.Add(routes);
    }
}
        
    

