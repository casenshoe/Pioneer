using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienAI : MonoBehaviour
{ 
    // Game Objects
    public GameObject player;
    public GameObject laser;

    // Audio
    public AudioClip[] laserBlasts = new AudioClip[3];

    // Components
    public Transform playerTransform;
    public Sprite[] spriteImages = new Sprite[9];
    public Vector3[] patrolPoints = new Vector3[3];
    public SpriteRenderer alienSprite;
    Vector2 currentRotation;

    // Counters and bools
    public float speed;
    public float waitTime;
    public float turnSpeed;
    public string behaviorType;
    int currentPointIndex;
    bool waiting;
    bool isShooting;
    bool isSkirmishing;

    void Start()
    {
        alienSprite = GetComponent<SpriteRenderer>();
        player = GameObject.Find("Ship");
        playerTransform = player.transform;
        int randomSprite = Random.Range(0, 8);
        GetComponent<SpriteRenderer>().sprite = spriteImages[randomSprite];
        patrolPoints[0] = transform.position;
        patrolPoints[1] = new Vector3(transform.position.x + Random.Range(-10, 10), transform.position.y, transform.position.z);
        patrolPoints[2] = new Vector3(transform.position.x + Random.Range(-10, 10), transform.position.y + Random.Range(-10, 10), transform.position.z);
        if (randomSprite == 0 || randomSprite == 3 || randomSprite == 6)
        {
            behaviorType = "Skirmisher";
            speed = 2.5f;
        }
        else if (randomSprite == 1 || randomSprite == 4 || randomSprite == 7)
        {
            behaviorType = "Dogfighter";
            speed = 2f;
        }
        else
        {
            behaviorType = "Pincher";
            speed = 1.75f;
        }
    }

    IEnumerator skirmish()
    {
        yield return new WaitForSeconds(4);
    }

    IEnumerator shoot()
    {
        int randInt = Random.Range(0, 3);
        isShooting = true;
        AudioSource.PlayClipAtPoint(laserBlasts[randInt], transform.position);
        Instantiate(laser, transform.position, transform.rotation);
        yield return new WaitForSeconds(2);
        isShooting = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, playerTransform.position) < 6)
        {
            // engage
            if (behaviorType == "Pincher")
            {
                if (Vector2.Distance(transform.position, playerTransform.position) < 6)
                {
                    transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
                    Vector2 current = transform.position;
                    Vector2 target = playerTransform.position;
                    var direction = target - current;
                    var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
                    transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                }
                else
                {
                    Vector2 current = transform.position;
                    Vector2 target = playerTransform.position;
                    var direction = target - current;
                    var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
                    transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                    //transform.up = Vector2.SmoothDamp(transform.up, player.transform.position, ref currentRotation, turnSpeed);
                }
            }
            else if (behaviorType == "Skirmisher")
            {
                // patrol
                Vector2 targetposition = patrolPoints[currentPointIndex];
                transform.up = Vector2.SmoothDamp(transform.up, targetposition, ref currentRotation, turnSpeed);
                if (transform.position != patrolPoints[currentPointIndex])
                {
                    transform.position = Vector2.MoveTowards(transform.position, patrolPoints[currentPointIndex], speed * Time.deltaTime);
                }
                else
                {
                    if (!waiting)
                    {
                        waiting = true;
                        StartCoroutine(Wait());
                    }
                }
            }
            else
            {
                if (Vector2.Distance(transform.position, playerTransform.position) < 10)
                {
                    transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
                    if (!isShooting)
                    {
                        StartCoroutine(shoot());
                    }
                }
                Vector2 current = transform.position;
                Vector2 target = playerTransform.position;
                var direction = target - current;
                var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, (speed + 1) * Time.deltaTime);
            Vector2 current = transform.position;
            Vector2 target = playerTransform.position;
            var direction = target - current;
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(waitTime);
        if (currentPointIndex + 1 < patrolPoints.Length)
        {
            currentPointIndex++;
        }
        else
        {
            currentPointIndex = 0;
        }
        waiting = false;
    }
}
