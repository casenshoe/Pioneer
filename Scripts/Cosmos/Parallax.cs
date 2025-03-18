using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float startingXPos; //This is starting position of the sprites.
    private float startingYPos;
    public float AmountOfParallax;  //This is amount of parallax scroll. 
    public Camera MainCamera;

    // Start is called before the first frame update
    void Start()
    {
        //Getting the starting X position of sprite.
        startingXPos = transform.position.x;
        startingYPos = transform.position.y;
        //Getting the length of the sprites.
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 Position = MainCamera.transform.position;
        float Temp = Position.x * (1 - AmountOfParallax);
        float XDistance = Position.x * AmountOfParallax;
        float YDistance = Position.y * AmountOfParallax;

        Vector3 NewPosition = new Vector3(startingXPos + XDistance, startingYPos + YDistance, transform.position.z);

        transform.position = NewPosition;
    }
}
