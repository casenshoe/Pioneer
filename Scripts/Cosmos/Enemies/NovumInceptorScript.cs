using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NovumInceptorScript : MonoBehaviour
{

    public GameObject laser;
    public GameObject torpedo;
    public GameObject alienExplosion;
    public GameObject laserBlast;
    public GameObject muzzleFlash;
    public Transform blaster;
    public GameObject healthBar;
    public GameObject tipBar;
    public Image healthBarRed;
    public GameObject metal;

    public AudioClip[] laserBlasts = new AudioClip[3];
    public int health = 20;

    bool isShooting;
    bool isTorpedoing;

    public Transform target; // The target (player) to follow
    public GameObject ship;
    public float followSpeed = 1f; // Speed at which the object follows the target
    public float rotateSpeed = 4f; // Speed at which the object rotates towards the target
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
        healthBarRed.fillAmount = health / 20f;
        this.GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.1f);
        this.GetComponent<SpriteRenderer>().color = Color.white;
        if (health <= 0)
        {
            Instantiate(alienExplosion, transform.position, transform.rotation);
            PlayerPrefs.SetString("Planet Completed", "Solus Deus Completed");
            healthBar.SetActive(false);
            for (int i = 1; i <= 50; i++)
            {
                Instantiate(metal, transform.position, transform.rotation);
            }
            ship.GetComponent<GameController>().aliensAlive -= 1;
            tipBar.SetActive(true);
            Destroy(this.gameObject);
        }
    }

    IEnumerator fireTorpedo(float waitTime)
    {
        AudioSource.PlayClipAtPoint(torpedoSound, camera.position, PlayerPrefs.GetFloat("SFX Volume"));
        isTorpedoing = true;
        Instantiate(torpedo, blaster.position, transform.rotation);
        Instantiate(muzzleFlash, blaster.position, transform.rotation);
        yield return new WaitForSeconds(waitTime);
        isTorpedoing = false;
    }

    IEnumerator shoot()
    {
        int randInt = Random.Range(0, 3);
        isShooting = true;
        AudioSource.PlayClipAtPoint(laserBlasts[randInt], camera.position, PlayerPrefs.GetFloat("SFX Volume"));
        Instantiate(laser, blaster.position, transform.rotation);
        Instantiate(muzzleFlash, blaster.position, transform.rotation);
        yield return new WaitForSeconds(0.75f);
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
            Debug.Log("Boss hit by torpedo");
            StartCoroutine(damageShip(3));
        }
    }

    void Update()
    {
        if (targetTimer >= 3f)
        {
            offset = new Vector2(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f));
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
            if (Vector2.Distance(transform.position, target.position) < 8)
            {
                if (!isShooting)
                {
                    StartCoroutine(shoot());
                }

                if (!isTorpedoing)
                {
                    StartCoroutine(fireTorpedo(2f));
                }
            }
            else
            {
                if (!isTorpedoing)
                {
                    StartCoroutine(fireTorpedo(1f));
                }
            }
        }
    }

}
