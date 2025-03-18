using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MetalScript : MonoBehaviour
{
    public Sprite[] spriteImages = new Sprite[3];
    public float speed;
    int randomOffset;
    Vector3 startPosition;
    Vector3 randomPosition;
    public float timer = 0f;
    public Transform player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Ship").GetComponent<Transform>();
        startPosition = transform.position;
        randomOffset = Random.Range(-30, 30);
        randomPosition = new Vector3(startPosition.x + randomOffset, startPosition.y + randomOffset, startPosition.z);
        speed = Random.Range(0.2f, 0.6f);
        int randomSprite = Random.Range(0, 3);
        GetComponent<SpriteRenderer>().sprite = spriteImages[randomSprite];
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPosition = player.transform.position;
        if (timer < speed * 2) // using (speed * 2) because the value is where I want it and i dont want another variable
        {
            transform.position = Vector3.MoveTowards(transform.position, randomPosition, speed * Time.deltaTime);
            transform.Rotate(Vector3.forward * (speed * 100) * Time.deltaTime);
            timer += Time.deltaTime;
        }
        else
        {
            float t = Random.Range(1f, 3f);
            transform.DOMove(playerPosition, t);
            transform.DOScale(0.25f, t);
        }
    }
}
