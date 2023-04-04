using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Numerics;

public class UserInterface : MonoBehaviour {
    
    //Screen
    [SerializeField] private CanvasGroup panel;
    private TextMeshProUGUI Details;
    public Camera cam;

    //Description
    private string objName;
    private string description;

    private void Start() {
        if (panel != null) {
            Details = panel.gameObject.GetComponentInChildren<TextMeshProUGUI>();        //Finding the textbox
        }
    }

    //Performing a ray cast
    void Update() 
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) {
            if (hit.transform.CompareTag("Planet")) {                           //Only interacting with planets
                PlanetDetails(hit.transform.name);
                Details.text = "Planet Name: " + objName + "\nDescription:" + description;          //Updating UI with planet information
            }
            else if (hit.transform.CompareTag("Star")) {                           //Only interacting with stars
                
                //Removing the referance number from the name of the star
                int spacePos = hit.transform.name.IndexOf(" ");
                string starName = hit.transform.name.Substring(0, spacePos);

                StarDetails(starName);
                Details.text = "Star Name: " + objName + "\nDescription:" + description;            // Updating UI with star information
            }
        }
        else {
            Details.text = "";      //Clearing UI when nothing is in player sights
        }
    }
    //Details of planets
    private void PlanetDetails(string planet) {
        switch (planet) {
            case "Mercury":
                objName = "Mercury";
                description = "Mercury is a rocky planet, also known as a terrestrial planet. Mercury has a solid, cratered surface, much like the Earth's moon.";
                break;
            case "Venus":
                objName = "Venus";
                description = "Venus is a little smaller than Earth, and is similar to Earth inside.";
                break;
            case "Earth":
                objName = "Earth";
                description = "Earth is a rocky planet with a solid and dynamic surface of mountains, canyons, plains and more. Most of our planet is covered in water.";
                break;
            case "Mars":
                objName = "Mars";
                description = "Mars is a dusty, cold, desert world with a very thin atmosphere. Mars is also a dynamic planet with seasons, polar ice caps, canyons, extinct volcanoes.";
                break;
            case "Jupiter":
                objName = "Jupiter";
                description = "Jupiter's familiar stripes and swirls are actually cold, windy clouds of ammonia and water, floating in an atmosphere of hydrogen and helium.";
                break;
            case "Saturn":
                objName = "Saturn";
                description = "Saturn is a massive ball made mostly of hydrogen and helium. Saturn is not the only planet to have rings, but none are as spectacular or as complex as Saturn's.";
                break;
            case "Uranus":
                objName = "Uranus";
                description = "Uranus's mass is made up of a hot dense fluid of icy materials (water, methane, and ammonia) above a small rocky core.";
                break;
            case "Neptune":
                objName = "Neptune";
                description = "Neptune is made of a thick soup of water, ammonia, and methane flowing over a solid core about the size of Earth. Neptune has a thick, windy atmosphere.";
                break;
            default:
                objName = "";
                description = "";
                break;
        }
    }
    //Details of stars
    private void StarDetails(string star) {
        switch (star) {
            case "Sun":
                objName = "Sun";
                description = "The Sun is the only star in our solar system. It has an avarge temperature of 5537°C and is a Yellow Dwarf star";
                break;
            case "Pollux":
                objName = "Pollux";
                description = "Pollux is a star that lies in the constellation Gemini. It has an avarge temperature of 4591°C and is a Red Giant star";
                break;
            case "Betelgeuse":
                objName = "Betelgeuse";
                description = "Betelgeuse is a red supergiant and one of the largest stars visible to the naked eye. It has an avarge temperature of 3226°C";
                break;
            case "Sirius":
                objName = "Sirius";
                description = "Sirius is the brightest star in the night sky. It is 25.4 times as luminous as the Sun. It has an avarge temperature of 9982°C";
                break;
            case "Altair":
                objName = "Altair";
                description = " Altair is located at a distance of 16.7 light-years (5.1 parsecs) from the Sun. It has an avarge temperature of 8000°C";
                break;
            case "Antares":
                objName = "Antares";
                description = "Antares is a red supergiant, a large evolved massive star and one of the largest stars visible to the naked eye. It has an avarge temperature of 3200°C";
                break;
            default:
                objName = "";
                description = "";
                break;
        }
    }
}
