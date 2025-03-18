using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceStationScript : MonoBehaviour
{
    public GameObject[] stationModules;

    // Start is called before the first frame update
    void Start()
    {
        // Show activated modules
        for (int i = 0; i <= stationModules.Length - 1; i++)
        {
            if (PlayerPrefs.GetInt(stationModules[i].name) == 1)
            {
                stationModules[i].SetActive(true);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
