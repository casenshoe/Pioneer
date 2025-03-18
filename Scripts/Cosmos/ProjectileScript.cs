using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    // Game Objects
    public GameObject asteroid;
    public GameObject asteroidExplosion;
    public GameObject alienExplode;
    public GameObject blueLaserBlast;
    public GameObject redLaserBlast;
    public GameObject torpedoBlast;
    public GameObject controller;
    public GameObject metalPrefab;
    public GameObject torpedoBlastCollider;
    public Sprite redLaser;
    public Sprite blueLaser;
    [SerializeField] Rigidbody2D rb;
    public GameObject virtualCam;
    public Transform camera;

    // Audio
    public AudioSource audioSrc;
    public AudioClip explosion, hit;

    // Counters and Bools
    private float projectileSpeed;
    public bool shotByPlayer;
    public float torpedoFuse;

    private void Start()
    {
        Destroy(gameObject, 2);
        controller = GameObject.Find("Ship");
        virtualCam = GameObject.Find("Virtual Camera");
        camera = GameObject.Find("Main Camera").GetComponent<Transform>();

        if (this.name == "Blue Laser(Clone)")
        {
            projectileSpeed = Mathf.Max(Mathf.Abs(rb.velocity.x), Mathf.Abs(rb.velocity.y)) + Random.Range(10, 20);
        }
        else if (this.name == "Red Laser(Clone)")
        {
            projectileSpeed = (Mathf.Abs(rb.velocity.x) + Mathf.Abs(rb.velocity.y)) / 2 + 8;
        }
        else if (this.name == "Torpedo(Clone)")
        {
            projectileSpeed = 8;
        }
        else if (this.name == "Acidic Spore(Clone)")
        {
            projectileSpeed = 10;
        }
        else if (this.name == "Vacuum Torpedo(Clone)")
        {
            projectileSpeed = 13;
        }
        rb.velocity = transform.up * projectileSpeed;
    }

    public void setVelocity(float speed)
    {
        projectileSpeed = speed;
        rb.velocity = transform.up * projectileSpeed;
    }

    public IEnumerator wait(int seconds)
    { 
        yield return new WaitForSeconds(seconds);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Alien") && this.name == "Blue Laser(Clone)")
        {
            AudioSource.PlayClipAtPoint(hit, camera.position, PlayerPrefs.GetFloat("SFX Volume"));
            Instantiate(blueLaserBlast, collision.gameObject.transform.position, transform.rotation);
            Destroy(this.gameObject);
        }
        else if (collision.gameObject.CompareTag("Player") && this.name == "Red Laser(Clone)" || this.name == "Other Blue Laser(Clone)(Clone)")
        {
            AudioSource.PlayClipAtPoint(hit, camera.position, PlayerPrefs.GetFloat("SFX Volume"));
            Instantiate(redLaserBlast, transform.position, transform.rotation);
            Destroy(this.gameObject);
        }
        else if (collision.gameObject.CompareTag("Boss") && this.name == "Blue Laser(Clone)")
        {
            AudioSource.PlayClipAtPoint(hit, camera.position, PlayerPrefs.GetFloat("SFX Volume"));
            Instantiate(blueLaserBlast, transform.position, transform.rotation);
            Destroy(this.gameObject);
        }
        else if (collision.gameObject.CompareTag("Player") && (this.name == "Torpedo(Clone)"))
        {
            AudioSource.PlayClipAtPoint(explosion, camera.position, PlayerPrefs.GetFloat("SFX Volume"));
            virtualCam.GetComponent<CamShakeScript>().ShakeCamera();
            Instantiate(torpedoBlast, transform.position, new Quaternion(0, 0, 0, 1));
            Destroy(this.gameObject);
        }
        else if ((collision.gameObject.CompareTag("Boss") || collision.gameObject.CompareTag("Alien")) && (this.name == "Vacuum Torpedo(Clone)"))
        {
            AudioSource.PlayClipAtPoint(explosion, camera.position, PlayerPrefs.GetFloat("SFX Volume"));
            virtualCam.GetComponent<CamShakeScript>().ShakeCamera();
            Instantiate(torpedoBlast, transform.position, new Quaternion(0, 0, 0, 1));
            Destroy(this.gameObject);
        }
    }
    private void Update()
    {
        if (this.name == "Torpedo(Clone)")
        {
            if (torpedoFuse >= 1.5f)
            {
                torpedoBlastCollider.SetActive(true);
                AudioSource.PlayClipAtPoint(explosion, camera.position, PlayerPrefs.GetFloat("SFX Volume"));
                Instantiate(torpedoBlast, transform.position, transform.rotation);
                virtualCam.GetComponent<CamShakeScript>().ShakeCamera();
                torpedoFuse = 0f;
                torpedoBlastCollider.SetActive(false);
                Destroy(this.gameObject);
            }
            else
            {
                torpedoFuse += Time.deltaTime;
            }
        }
        else if (this.name == "Vacuum Torpedo(Clone)")
        {
            if (torpedoFuse >= 1.5f)
            {
                torpedoBlastCollider.SetActive(true);
                AudioSource.PlayClipAtPoint(explosion, camera.position, PlayerPrefs.GetFloat("SFX Volume"));
                Instantiate(torpedoBlast, transform.position, transform.rotation);
                virtualCam.GetComponent<CamShakeScript>().ShakeCamera();
                torpedoFuse = 0f;
                torpedoBlastCollider.SetActive(false);
                Destroy(this.gameObject);
            }
            else
            {
                torpedoFuse += Time.deltaTime;
            }
        }
        else if (this.name == "Acidic Spore(Clone)")
        {
            transform.Rotate(Vector3.forward * (projectileSpeed * 100) * Time.deltaTime);
        }
    }

}
