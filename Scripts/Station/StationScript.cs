using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StationScript : MonoBehaviour
{
    public TextMeshProUGUI metalCountText;
    public List<int> purchasedShields = new List<int>();
    public List<int> purchasedThrusters = new List<int>();
    public List<int> purchasedBlasters = new List<int>();

    // Start is called before the first frame update
    void Start()
    {

        // Shields
        if (PlayerPrefs.GetInt("Pulsar Guard Purchased") == 1)
        {
            purchasedShields.Add(1);
        }
        if (PlayerPrefs.GetInt("Nebula Shield Purchased") == 1)
        {
            purchasedShields.Add(2);
        }
        if (PlayerPrefs.GetInt("Solar Protector Purchased") == 1)
        {
            purchasedShields.Add(3);
        }
        if (PlayerPrefs.GetInt("Astral Protector Purchased") == 1)
        {
            purchasedShields.Add(4);
        }

        // Thrusters
        if (PlayerPrefs.GetInt("Flare Thruster Purchased") == 1)
        {
            purchasedThrusters.Add(1);
        }
        if (PlayerPrefs.GetInt("Pulse Engine Purchased") == 1)
        {
            purchasedThrusters.Add(2);
        }
        if (PlayerPrefs.GetInt("Burst Engine Purchased") == 1)
        {
            purchasedThrusters.Add(3);
        }
        if (PlayerPrefs.GetInt("Supercharged Thruster Purchased") == 1)
        {
            purchasedThrusters.Add(4);
        }

        // Blasters
        if (PlayerPrefs.GetInt("Twin Phasers Purchased") == 1)
        {
            purchasedBlasters.Add(1);
        }
        if (PlayerPrefs.GetInt("Vacuum Torpedoes Purchased") == 1)
        {
            purchasedBlasters.Add(2);
        }

        metalCountText.text = PlayerPrefs.GetInt("Metal Count").ToString() + " credits";
    }

}
