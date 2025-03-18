using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlanetHologramScript : MonoBehaviour
{
    public Sprite[] spriteImages = new Sprite[9];
    public TextMeshProUGUI planetName;
    private Dictionary<int, string> planetNames = new Dictionary<int, string>()
    {
        {0, "Station"},
        {1, "Solus Deus"},
        {2, "Pax Amor"},
        {3, "Cohors"},
        {4, "Captiosus"},
        {5, "Fortis"},
        {6, "Magnus Frater"},
        {7, "Frater Minor"},
        {8, "Crinitus"}
    };
    public int count;

    private void Start()
    {
        GetComponent<Image>().sprite = spriteImages[0];
        count = 0;
    }

    public void changeSprite()
    {
        if (count < 8)
        {
            count++;
            GetComponent<Image>().sprite = spriteImages[count];
            planetName.GetComponent<TextMeshProUGUI>().text = planetNames[count];
        }
        else
        {
            count = 0;
            GetComponent<Image>().sprite = spriteImages[0];
            planetName.GetComponent<TextMeshProUGUI>().text = planetNames[0];
        }
    }
}
