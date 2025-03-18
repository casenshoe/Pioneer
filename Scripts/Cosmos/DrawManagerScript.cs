using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawManagerScript : MonoBehaviour
{
    private Vector3 playerPosition;
    public GameObject ship;
    public GameObject[] backgrounds = new GameObject[4];
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        playerPosition = ship.GetComponent<Transform>().position;

        for (int i = 0; i <= 3; i++)
        {
            if (Vector3.Distance(playerPosition, backgrounds[i].GetComponent<Transform>().position) > 10)
            {
                backgrounds[i].SetActive(false);
            }
            else
            {
                backgrounds[i].SetActive(true);
            }
        }
        
    }
}
