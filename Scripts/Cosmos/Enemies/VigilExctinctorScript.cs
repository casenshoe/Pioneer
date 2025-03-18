using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VigilExctinctorScript : MonoBehaviour
{

    public GameObject laser;
    public GameObject torpedo;
    public GameObject alienExplosion;
    public GameObject laserBlast;
    public Transform blaster;
    public Transform blasterTwo;
    public Transform torpedoBlaster;
    public GameObject muzzleBlast;
    public GameObject healthBar;
    public Image healthBarRed;
    public GameObject metal;

    public AudioClip[] laserBlasts = new AudioClip[3];
    public int health = 30;

    bool isShooting;
    bool isTorpedoing;

    public Transform target; // The target (player) to follow
    public GameObject ship;
    public float followSpeed = 12f; // Speed at which the object follows the target
    public float rotateSpeed = 2f; // Speed at which the object rotates towards the target
    public Vector2 offset; // Offset from the target
    public float targetTimer = 0f;
    public AudioClip hit;
    public AudioClip torpedoSound;
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
            PlayerPrefs.SetString("Planet Completed", "Pax Amor Completed");
            healthBar.SetActive(false);
            for (int i = 1; i <= 100; i++)
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
        Instantiate(torpedo, torpedoBlaster.position, transform.rotation);
        yield return new WaitForSeconds(waitTime);
        isTorpedoing = false;
    }

    IEnumerator shoot(float waitTime)
    {
        int randInt = Random.Range(0, 3);
        int otherRandInt = Random.Range(0, 2); // This is terrible naming
        isShooting = true;
        AudioSource.PlayClipAtPoint(laserBlasts[randInt], camera.position, PlayerPrefs.GetFloat("SFX Volume"));
        if (otherRandInt == 0)
        {
            Instantiate(laser, blaster.position, transform.rotation);
            Instantiate(muzzleBlast, blaster.position, transform.rotation);
        }
        else
        {
            Instantiate(laser, blasterTwo.position, transform.rotation);
            Instantiate(muzzleBlast, blasterTwo.position, transform.rotation);
        }
        yield return new WaitForSeconds(waitTime);
        isShooting = false;
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

    void Update()
    {
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
                    StartCoroutine(fireTorpedo(4f));
                }
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, targetPosition, (followSpeed) * Time.deltaTime);
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
    }

}
