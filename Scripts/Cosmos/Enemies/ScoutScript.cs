using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoutScript : MonoBehaviour
{

    public GameObject laser;

    public AudioClip[] laserBlasts = new AudioClip[3];

    bool isShooting;


    //chatgpt variables
    public Transform target; // The target (player) to follow
    public GameObject ship;
    public Vector3 lastPosition;
    public float followSpeed = 3f; // Speed at which the object follows the target
    public float rotateSpeed = 10f; // Speed at which the object rotates towards the target
    public Vector2 offset; // Offset from the target
    public float targetTimer = 0f;
    public Transform camera;
    public int health = 1;
    public GameObject alienExplosion;


    private void Start()
    {
        ship = GameObject.Find("Ship");
        camera = GameObject.Find("Main Camera").GetComponent<Transform>();
        target = ship.transform;
    }

    IEnumerator shoot()
    {
        int randInt = Random.Range(0, 3);
        isShooting = true;
        AudioSource.PlayClipAtPoint(laserBlasts[randInt], camera.position, PlayerPrefs.GetFloat("SFX Volume"));
        Instantiate(laser, transform.position, transform.rotation);
        yield return new WaitForSeconds(4);
        isShooting = false;
    }

    public IEnumerator damageShip(int dmg)
    {
        health -= dmg;
        this.GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.1f);
        this.GetComponent<SpriteRenderer>().color = Color.white;
        if (health <= 0)
        {
            Instantiate(alienExplosion, transform.position, transform.rotation);
            ship.GetComponent<GameController>().aliensAlive -= 1;
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Blue Laser"))
        {
            Destroy(collision.gameObject);
            StartCoroutine(damageShip(1));
        }
        else if (collision.gameObject.CompareTag("Vacuum Torpedo"))
        {
            StartCoroutine(damageShip(1));
        }
    }

    // Update is called once per frame

    void Update()
    {
        if (targetTimer >= 3f)
        {
            offset = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));
            targetTimer = 0f;
        }
        else
        {
            targetTimer += Time.deltaTime;
        }

        Vector3 targetPosition = new Vector3(target.position.x + offset.x, target.position.y + offset.y, transform.position.z);

        if (Vector2.Distance(transform.position, target.position) < 6)
        {
            if (Vector2.Distance(transform.position, target.position) < 3)
            {
                if (!isShooting)
                {
                    StartCoroutine(shoot());
                }
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, targetPosition, (followSpeed + 1) * Time.deltaTime);
                if (!isShooting)
                {
                    StartCoroutine(shoot());
                }
            }

            Vector2 direction = (targetPosition - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, (followSpeed + 1) * Time.deltaTime);
            //transform.position = Vector3.MoveTowards(transform.position, targetPosition, (followSpeed + 1) * Time.deltaTime);
        }
    }

}
