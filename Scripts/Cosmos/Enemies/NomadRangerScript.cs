using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NomadRangerScript : MonoBehaviour
{

    public GameObject laser;
    public GameObject alienExplosion;
    public GameObject laserBlast;
    public GameObject muzzleFlash;
    public Transform blaster;
    public GameObject healthBar;
    public Image healthBarRed;
    public GameObject metal;

    public AudioClip[] laserBlasts = new AudioClip[3];
    public int health = 30;

    bool isShooting;
    bool isTorpedoing;

    public Transform target; // The target (player) to follow
    public GameObject ship;
    public float speed = 10f; // Speed at which the object follows the target
    public Vector2 offset; // Offset from the target
    public float targetTimer = 0f;
    public AudioClip hit;
    public bool isStrafing;
    public int shotsFired;
    public Transform camera;

    private void Start()
    {
        camera = Camera.main.GetComponent<Transform>();
        ship = GameObject.Find("Ship");
        target = ship.transform;
    }

    public IEnumerator damageShip(int dmg)
    {
        health -= dmg;
        AudioSource.PlayClipAtPoint(hit, camera.position, PlayerPrefs.GetFloat("SFX Volume"));
        healthBarRed.fillAmount = health / 30f;
        this.GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.1f);
        this.GetComponent<SpriteRenderer>().color = Color.white;
        if (health <= 0)
        {
            Instantiate(alienExplosion, transform.position, transform.rotation);
            PlayerPrefs.SetString("Planet Completed", "Magnus Frater Completed");
            healthBar.SetActive(false);
            for (int i = 1; i <= 300; i++)
            {
                Instantiate(metal, transform.position, transform.rotation);
            }
            ship.GetComponent<GameController>().aliensAlive -= 1;
            Destroy(this.gameObject);
        }
    }

    IEnumerator shoot()
    {
        int randInt = Random.Range(0, 3);
        isShooting = true;
        AudioSource.PlayClipAtPoint(laserBlasts[randInt], camera.position, PlayerPrefs.GetFloat("SFX Volume"));
        var laserObject = Instantiate(laser, blaster.position, transform.rotation);
        laserObject.GetComponent<Rigidbody2D>().velocity = laserObject.transform.up * 20;
        Instantiate(muzzleFlash, blaster.position, transform.rotation);
        yield return new WaitForSeconds(0.25f);
        isShooting = false;
    }

    IEnumerator wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Blue Laser"))
        {
            Instantiate(laserBlast, collision.transform.position, transform.rotation);
            Destroy(collision.gameObject);
            StartCoroutine(damageShip(1));
        }
        if (collision.gameObject.CompareTag("Vacuum Torpedo"))
        {
            StartCoroutine(damageShip(3));
        }
    }

    private void strafeRun()
    {
        shotsFired = 0;
        isStrafing = true;
        int leftOrRight = Random.Range(0, 2);
        int upOrDown = Random.Range(0, 2);
        if (leftOrRight == 0)
        {
            if (upOrDown == 0)
            {
                transform.position = new Vector2(target.position.x - Random.Range(0, 15), target.position.y + 6);
            }
            else
            {
                transform.position = new Vector2(target.position.x - Random.Range(0, 15), target.position.y - 6);
            }
        }
        else
        {
            if (upOrDown == 0)
            {
                transform.position = new Vector2(target.position.x + Random.Range(0, 15), target.position.y + 6);
            }
            else
            {
                transform.position = new Vector2(target.position.x + Random.Range(0, 15), target.position.y - 6);
            }
        }

        //transform.LookAt(target, transform.forward);
        Vector2 direction = (target.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.rotation = targetRotation;
        GetComponent<Rigidbody2D>().velocity = transform.up * speed;
    }

    void Update()
    {
        if (!isStrafing)
        {
            strafeRun();
        }

        if (!isShooting && shotsFired < 3)
        {
            StartCoroutine(shoot());
            shotsFired++;
        }

        if (Vector2.Distance(transform.position, target.position) > 20)
        {
            StartCoroutine(wait(Random.Range(1, 4)));
            isStrafing = false;
        }
    }

}
