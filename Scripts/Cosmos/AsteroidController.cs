using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    public GameObject ship;
    public int speed;
    public GameObject asteroidExplosion;
    SpriteRenderer sr;

    private void Start()
    {
        ship = GameObject.Find("Ship");
        speed = 25;
        sr = GetComponent<SpriteRenderer>();
        float randomScale = Random.Range(0.5f, 1);
        transform.localScale = new Vector3(randomScale, randomScale, randomScale);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, ship.GetComponent<Transform>().position, 1.15f * Time.deltaTime);
        transform.Rotate(Vector3.forward * speed * Time.deltaTime);
    }
}
