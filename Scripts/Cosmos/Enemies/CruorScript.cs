using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CruorScript : MonoBehaviour
{

    public GameObject laser;
    public GameObject blueLaser;
    public GameObject torpedo;
    public GameObject alienExplosion;
    public GameObject laserBlast;
    public Transform blaster;
    public Transform blasterTwo;
    public Transform blasterThree;
    public GameObject muzzleBlast;
    public GameObject healthBar;
    public Image healthBarRed;
    public GameObject metal;

    public AudioClip[] laserBlasts = new AudioClip[3];
    public int health = 40;

    bool isShooting;
    bool isTorpedoing;

    public Transform target; // The target (player) to follow
    public GameObject ship;
    public float followSpeed = 24f; // Speed at which the object follows the target
    public float rotateSpeed = 2f; // Speed at which the object rotates towards the target
    public Vector2 offset; // Offset from the target
    public float targetTimer = 0f;
    public AudioClip hit;
    public float iFramesCounter;
    public bool isInvincible;
    public int whichBarrel;
    public int moveOnFrame;
    public AudioClip torpedoSound;
    public Transform camera;

    private void Start()
    {
        camera = Camera.main.GetComponent<Transform>();
        ship = GameObject.Find("Ship");
        target = ship.transform;

        blueLaser.name = "Blue Laser(Clone)";
    }

    public IEnumerator damageShip(int dmg)
    {
        health -= dmg;
        AudioSource.PlayClipAtPoint(hit, camera.position, PlayerPrefs.GetFloat("SFX Volume"));
        healthBarRed.fillAmount = health / 40f;
        this.GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.1f);
        this.GetComponent<SpriteRenderer>().color = Color.white;
        if (health <= 0)
        {
            Instantiate(alienExplosion, transform.position, transform.rotation);
            PlayerPrefs.SetString("Planet Completed", "Cohors Completed");
            healthBar.SetActive(false);
            for (int i = 1; i <= 150; i++)
            {
                Instantiate(metal, transform.position, transform.rotation);
            }
            ship.GetComponent<GameController>().aliensAlive -= 1;
            Destroy(this.gameObject);
        }
    }

    IEnumerator fireTorpedo(float waitTime)
    {
        AudioSource.PlayClipAtPoint(torpedoSound, camera.position, PlayerPrefs.GetFloat("SFX Volume"));
        isTorpedoing = true;
        if (whichBarrel % 2 == 0)
        {
            Instantiate(torpedo, blasterTwo.position, transform.rotation);
        }
        else
        {
            Instantiate(torpedo, blasterThree.position, transform.rotation);
        }
        whichBarrel++;
        yield return new WaitForSeconds(waitTime);
        isTorpedoing = false;
    }

    IEnumerator shoot(float waitTime)
    {
        int randInt = Random.Range(0, 3);
        isShooting = true;
        AudioSource.PlayClipAtPoint(laserBlasts[randInt], camera.position, PlayerPrefs.GetFloat("SFX Volume"));
        Instantiate(laser, blaster.position, transform.rotation);
        Instantiate(muzzleBlast, blaster.position, transform.rotation);
        yield return new WaitForSeconds(waitTime);
        isShooting = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Blue Laser"))
        {
            if (!isInvincible)
            {
                StartCoroutine(damageShip(1));
            }
            else
            {
                Destroy(collision.gameObject);
                Instantiate(blueLaser, collision.transform.position, transform.rotation);
            }
        }
        if (collision.gameObject.CompareTag("Vacuum Torpedo"))
        {
            if (!isInvincible)
            {
                StartCoroutine(damageShip(3));
            }
            else
            {
                Instantiate(torpedo, collision.transform.position, transform.rotation);
            }
        }
    }

    void Update()
    {
        if (iFramesCounter < 10 && isInvincible == false)
        {
            iFramesCounter += Time.deltaTime;
        }
        else
        {
            isInvincible = true;
            this.GetComponent<SpriteRenderer>().color = Color.yellow;
            if (iFramesCounter < 15)
            {
                iFramesCounter += Time.deltaTime;
            }
            else
            {
                isInvincible = false;
                this.GetComponent<SpriteRenderer>().color = Color.white;
                iFramesCounter = 0;
            }
        }

        if (targetTimer >= 3f)
        {
            offset = new Vector2(Random.Range(0f, 0.3f), Random.Range(0f, 0.3f));
            targetTimer = 0f;
        }
        else
        {
            targetTimer += Time.deltaTime;
        }

        Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);

        if (ship.GetComponent<Rigidbody2D>().velocity.x >= 0)
        {
            targetPosition = new Vector3(target.position.x + offset.x, target.position.y + offset.y, transform.position.z);
        }
        else
        {
            targetPosition = new Vector3(target.position.x - offset.x, target.position.y - offset.y, transform.position.z);
        }


        Vector2 direction = (targetPosition - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, target.position) < 16)
        {
            // if player closest range to boss
            if (Vector2.Distance(transform.position, target.position) < 10)
            {
                if (!isShooting)
                {
                    StartCoroutine(shoot(0.5f));
                }

                if (!isTorpedoing)
                {
                    StartCoroutine(fireTorpedo(2f));
                }
            }
            else
            {
                if (moveOnFrame % 10 == 0){
                    transform.position = Vector3.Lerp(transform.position, targetPosition, (followSpeed) * Time.deltaTime);
                }
                if (!isShooting)
                {
                    StartCoroutine(shoot(1f));
                }
                if (!isTorpedoing)
                {
                    StartCoroutine(fireTorpedo(4f));
                }
            }
        }
        moveOnFrame++;
    }

}
