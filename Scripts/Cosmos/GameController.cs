using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    // Movement
    public float speed = 5;
    public float turnSpeed = 0.3f;
    public float acceleration = 2f;
    private Vector3 pos1;
    private Vector3 pos2;
    private Vector2 currentRotation;
    private Vector2 currentVelocity;
    Transform mainCamera;

    // Game Objects
    [SerializeField] private GameObject laser;
    public GameObject torpedo;
    [SerializeField] private GameObject pauseMenu;
    public Sprite[] spriteImages = new Sprite[4];
    public AudioClip[] laserBlasts = new AudioClip[3];
    public GameObject blasterOne, blasterOneFlash;
    public GameObject twinPhasers, leftBarrel, rightBarrel, leftTorpedoBarrel, rightTorpedoBarrel;
    public GameObject vacuumTorpedoes;
    public GameObject thruster;
    public GameObject shield;
    public CircleCollider2D stationBay, solusDeus, paxAmor, cohors, captiosus, fortis, magnusFrater, fraterMinor, crinitus;
    public GameObject novumInceptor, vigilExtinctor, cruor, ultusDreadnought, constantem, nomadRanger, nuclearInsectum, cruiserUltimus;
    public GameObject healthBar, planetLoadBar, tipBar;
    public GameObject stationButton;
    public ParticleSystem thrust;
    public GameObject station;
    public GameObject scout;
    public GameObject dogFighter;
    public GameObject brute;
    public GameObject boostButton;
    public Sprite[] shieldSprites;
    public Sprite[] thrusterSprites;
    public GameObject[] thrusts;
    public GameObject blockRaycast;
    public SpriteRenderer healthDisplay;
    public GameObject sceneManager;
    public GameObject guide;

    // Audio
    public AudioSource source;
    public GameObject musicObject; //to access the script
    [SerializeField] AudioClip thrusters, blipOne, blipTwo, torpedoSound;
    [SerializeField] AudioSource music;

    // UI
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private FloatingJoystick joystick;
    public TextMeshProUGUI coordinates;
    public TextMeshProUGUI metalCountDisplay;
    public TextMeshProUGUI healthBarText;
    public Image planetLoaderFiller;
    public Image fuelFiller;

    // Counters
    public int metalCount; //change this
    public int health = 5;
    public int aliensAlive;
    private float fireTimer, spawnTimer, spawnTimerMax;
    private float timeBetweenFiring = 0.3f;
    private float slope;
    public int maxFuel = 180;
    public float fuel;
    public int whichBarrel, whichTorpedoBarrel;

    // Bools
    public bool canFire, attachCamera, frame;
    public bool isSpawningAliens, canSpawnAlien;
    public bool leaveToStation, firstTimePlaying;
    public bool isBoosting;
    public bool canMove = true;

    private void Start()
    {
        health += 5 * PlayerPrefs.GetInt("Equipped Shield");
        maxFuel += 60 * PlayerPrefs.GetInt("Equipped Thruster");
        fuel = maxFuel;
        pauseMenu.SetActive(false);
        mainCamera = Camera.main.transform;
        spawnTimerMax = UnityEngine.Random.Range(50f, 60f);

        if (PlayerPrefs.GetInt("FIRSTTIMEOPENING", 1) == 1)
        {
            firstTimePlaying = true;
            PlayerPrefs.SetInt("FIRSTTIMEOPENING", 0);
            tipBar.SetActive(true);
            blockRaycast.SetActive(true);
            StartCoroutine(tipBar.GetComponent<TipScript>().displayTip(0));
        }

        if (PlayerPrefs.GetInt("Twin Phasers Purchased") == 1)
        {
            twinPhasers.SetActive(true);
        }
        else
        {
            twinPhasers.SetActive(false);
        }
        if (PlayerPrefs.GetInt("Vacuum Torpedoes Purchased") == 1)
        {
            vacuumTorpedoes.SetActive(true);
        }
        else
        {
            vacuumTorpedoes.SetActive(false);
        }

        if (PlayerPrefs.GetInt("Equipped Thruster") == 1)
        {
            thruster.transform.position = new Vector3(0f, -0.24f, 0f);
        }
        else if (PlayerPrefs.GetInt("Equipped Thruster") == 2)
        {
            thruster.transform.position = new Vector3(0f, -0.28f, 0f);
        }
        else if (PlayerPrefs.GetInt("Equipped Thruster") == 3)
        {
            thruster.transform.position = new Vector3(0f, -0.06f, 0f);
        }
        else
        {
            thruster.transform.position = new Vector3(0f, -0.16f, 0f);
        }

        

        speed += PlayerPrefs.GetInt("Equipped Thruster");
        acceleration -= PlayerPrefs.GetInt("Equipped Thruster") * 0.2f;

        thrusts[PlayerPrefs.GetInt("Equipped Thruster")].SetActive(true);
        thrust = thrusts[PlayerPrefs.GetInt("Equipped Thruster")].GetComponent<ParticleSystem>();
        thruster.GetComponent<SpriteRenderer>().sprite = thrusterSprites[PlayerPrefs.GetInt("Equipped Thruster")];
        shield.GetComponent<SpriteRenderer>().sprite = shieldSprites[PlayerPrefs.GetInt("Equipped Shield")];

        music.Play();
        source.clip = thrusters;
        source.volume = 0.05f * PlayerPrefs.GetFloat("SFX Volume");
        source.Play();
    }

    public void shoot()
    {
        if (Input.GetMouseButton(0) && canFire)
        {
            whichBarrel += 1;
            canFire = false;
            int randInt = UnityEngine.Random.Range(0, 3);
            AudioSource.PlayClipAtPoint(laserBlasts[randInt], mainCamera.position, PlayerPrefs.GetFloat("SFX Volume"));
            Instantiate(laser, blasterOne.transform.position, this.transform.rotation);
            Instantiate(blasterOneFlash, blasterOne.transform.position, this.transform.rotation);

            if (PlayerPrefs.GetInt("Twin Phasers Purchased") == 1)
            {
                if (whichBarrel % 3 == 0)
                {
                    Instantiate(laser, leftBarrel.transform.position, this.transform.rotation);
                    Instantiate(blasterOneFlash, leftBarrel.transform.position, this.transform.rotation);
                    Instantiate(laser, rightBarrel.transform.position, this.transform.rotation);
                    Instantiate(blasterOneFlash, rightBarrel.transform.position, this.transform.rotation);
                }
            }
            if (PlayerPrefs.GetInt("Vacuum Torpedoes Purchased") == 1)
            {   
                if (whichBarrel % 5 == 0)
                {
                    whichTorpedoBarrel += 1;
                    AudioSource.PlayClipAtPoint(torpedoSound, mainCamera.position, PlayerPrefs.GetFloat("SFX Volume"));
                    if (whichTorpedoBarrel % 2 == 0)
                    {
                        Instantiate(torpedo, leftTorpedoBarrel.transform.position, this.transform.rotation);
                    }
                    else
                    {
                        Instantiate(torpedo, rightTorpedoBarrel.transform.position, this.transform.rotation);
                    }
                }
            }
        }
    }

    public void pause()
    {
        if (Time.timeScale != 0)
        {
            music.Pause();
            source.Pause();
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
        }
        else
        {
            music.Play();
            source.Play();
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
        }
    }

    public void toggleGuide()
    {
        if (guide.activeSelf)
        {
            healthDisplay.enabled = true;
            guide.SetActive(false);
        }
        else
        {
            healthDisplay.enabled = false;
            guide.SetActive(true);
        }
    }

    public IEnumerator damageShip(int damage)
    {
        health -= damage;
        healthDisplay.color = Color.red;
        this.GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.1f);
        this.GetComponent<SpriteRenderer>().color = Color.white;
        if (health > ((5 + (5 * PlayerPrefs.GetInt("Equipped Shield"))) * .66f)) //was .8f
        {
            healthDisplay.color = Color.green;
        }
        else if(health > ((5 + (5 * PlayerPrefs.GetInt("Equipped Shield"))) * .33f)) //was .3f
        {
            healthDisplay.color = Color.yellow;
        }
        else
        {
            healthDisplay.color = Color.red;
        }

        if (health > 4 * .75f)
        {
            GetComponent<SpriteRenderer>().sprite = spriteImages[0];
        }
        else if (health > 4 * .50f)
        {
            GetComponent<SpriteRenderer>().sprite = spriteImages[1];
        }
        else if (health > 4 * .25f)
        {
            GetComponent<SpriteRenderer>().sprite = spriteImages[2];
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = spriteImages[3];
            if (health <= 0)
            {
                PlayerPrefs.SetInt("Metal Extracted", 0);
                PlayerPrefs.SetString("Outcome", "Shot Down");
                PlayerPrefs.SetString("Planet Completed", "None");
                sceneManager.GetComponent<SceneTransitions>().startLoadExtractionScreen();
            }
        }
    }

    public IEnumerator freezeShip(int seconds)
    {
        canMove = false;
        this.GetComponent<SpriteRenderer>().color = Color.green;
        yield return new WaitForSeconds(seconds);
        this.GetComponent<SpriteRenderer>().color = Color.white;
        canMove = true;
    }

    public void spawnSquad(Vector3 position)
    {
        if (!firstTimePlaying)
        {
            spawnAlien(position + new Vector3(-5, 5), scout);
            spawnAlien(position + new Vector3(5, 5), scout);
            spawnAlien(position + new Vector3(-5, -5), dogFighter);
            spawnAlien(position + new Vector3(5, -5), brute);
            aliensAlive += 4;
            musicObject.GetComponent<MusicScript>().FadeToActionMusic();
        }
    }

    public void spawnAlien(Vector3 position, GameObject alienType)
    {
        Instantiate(alienType, position, transform.rotation);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Metal"))
        {
            if (UnityEngine.Random.Range(0, 2) == 0)
            {
                AudioSource.PlayClipAtPoint(blipOne, mainCamera.position, PlayerPrefs.GetFloat("SFX Volume"));
            }
            else
            {
                AudioSource.PlayClipAtPoint(blipTwo, mainCamera.position, PlayerPrefs.GetFloat("SFX Volume"));
            }
            Destroy(collision.gameObject);
            metalCount++;
            metalCountDisplay.text = metalCount.ToString();
        }
        else if (collision.gameObject.CompareTag("Laser"))
        {
            StartCoroutine(damageShip(1));
        }
        else if (collision.gameObject.CompareTag("Torpedo"))
        {
            StartCoroutine(damageShip(2));
        }
        else if (collision.gameObject.CompareTag("Acidic Spore"))
        {
            Destroy(collision.gameObject);
            StartCoroutine(freezeShip(3));
        }
    }

    public void checkBoss(GameObject boss, CircleCollider2D planet, int secs)
    {
        if (boss != null && Vector2.Distance(transform.position, planet.ClosestPoint(transform.position)) < 20)
        {
            if (!boss.activeInHierarchy)
            {
                if (GetComponent<PolygonCollider2D>().IsTouching(planet))
                {
                    planetLoadBar.SetActive(true);
                    planetLoaderFiller.fillAmount += Time.deltaTime * 2 / secs;
                    if (planetLoaderFiller.fillAmount == 1)
                    {
                        aliensAlive += 1;
                        boss.SetActive(true);
                        musicObject.GetComponent<MusicScript>().FadeToActionMusic();
                        healthBar.SetActive(true);
                        healthBarText.text = boss.name;
                        planetLoadBar.SetActive(false);
                    }
                }
                else
                {
                    if (planetLoaderFiller.fillAmount > 0)
                    {
                        planetLoaderFiller.fillAmount -= Time.deltaTime * 2 / secs;
                    }

                    if (planetLoaderFiller.fillAmount <= 0)
                    {
                        planetLoadBar.SetActive(false);
                    }
                    else
                    {
                        planetLoaderFiller.fillAmount -= Time.deltaTime * 2 / secs;
                    }
                }
            }
        }
    }

    void Update()
    {
        coordinates.text = "X: " + Mathf.Round(transform.position.x).ToString() + ", " + "Y: " + Mathf.Round(transform.position.y).ToString();

        if (!canFire)
        {
            fireTimer += Time.deltaTime;
            if (fireTimer > timeBetweenFiring)
            {
                canFire = true;
                fireTimer = 0;
            }
        }

        if (frame)
        {
            frame = false;
            // spawn aliens on a timer depending on how far from station the player is
            if (spawnTimer >= spawnTimerMax)
            {
                spawnTimer = 0;

                pos2 = transform.position;
                slope = (pos1.x - pos2.x) / (pos1.y - pos2.y);
                if (pos1.x > pos2.x)
                {
                    spawnSquad(new Vector3(pos2.x - 15, (pos2.x - 15) * slope, -90));
                }
                else
                {
                    spawnSquad(new Vector3(pos2.x + 15, (pos2.x + 15) * slope, -90));
                }

                if (Vector3.Distance(transform.position, station.transform.position) >= 200)
                {
                    spawnTimerMax = UnityEngine.Random.Range(15f, 25f);
                }
                else if (Vector3.Distance(transform.position, station.transform.position) >= 150)
                {
                    spawnTimerMax = UnityEngine.Random.Range(20f, 30f);
                }
                else if (Vector3.Distance(transform.position, station.transform.position) >= 100)
                {
                    spawnTimerMax = UnityEngine.Random.Range(30f, 40f);
                }
                else if (Vector3.Distance(transform.position, station.transform.position) >= 50)
                {
                    spawnTimerMax = UnityEngine.Random.Range(40f, 50f);
                }
                else
                {
                    spawnTimerMax = UnityEngine.Random.Range(50f, 60f);
                }
            }
            else
            {
                if (spawnTimer > spawnTimerMax - 0.5f && spawnTimer < spawnTimerMax - 0.3f)
                {
                    pos1 = transform.position;
                }
                spawnTimer += Time.deltaTime * 2;
            }

            checkBoss(novumInceptor, solusDeus, 10);
            checkBoss(vigilExtinctor, paxAmor, 15);
            checkBoss(cruor, cohors, 20);
            checkBoss(ultusDreadnought, captiosus, 25);
            checkBoss(constantem, fortis, 30);
            checkBoss(nomadRanger, magnusFrater, 35);
            checkBoss(nuclearInsectum, fraterMinor, 40);
            checkBoss(cruiserUltimus, crinitus, 45);
        }
        else
        {
            frame = true;
        }


        // check music
        if (aliensAlive == 0 && !musicObject.GetComponent<MusicScript>().isFadingToBackground)
        {
            musicObject.GetComponent<MusicScript>().FadeToBackgroundMusic();
        }

        // check for extractions
        if (fuel > 0)
        {
            fuel -= Time.deltaTime;
            fuelFiller.fillAmount = fuel / maxFuel;
        }
        else
        {
            PlayerPrefs.SetString("Outcome", "Out of Fuel");
            PlayerPrefs.SetString("Planet Completed", "None");
            sceneManager.GetComponent<SceneTransitions>().startLoadExtractionScreen();
        }

        if (GetComponent<PolygonCollider2D>().IsTouching(stationBay))
        {
            stationButton.SetActive(true);
        }
        else
        {
            stationButton.SetActive(false);
        }

    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            var mainsettings = thrust.main;
            var emission = thrust.emission;
            Vector2 targetVelocity;
            isBoosting = boostButton.GetComponent<BoostButtonScript>().buttonPressed && boostButton.GetComponent<BoostButtonScript>().boost > 0;

            targetVelocity = new Vector2(joystick.Horizontal, joystick.Vertical) * speed;
            if (isBoosting)
            {
                rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity * 2, ref currentVelocity, 1);
                source.volume = 0.5f * PlayerPrefs.GetFloat("SFX Volume");
            }
            else
            {
                rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref currentVelocity, acceleration);
            }

            // If no input is recieved from joystick, ship should face last known direction
            if (joystick.Horizontal != 0 || joystick.Vertical != 0)
            {
                source.volume = ((Mathf.Abs(joystick.Horizontal) + Mathf.Abs(joystick.Vertical)) / 7) * PlayerPrefs.GetFloat("SFX Volume");
                emission.rateOverTime = ((Mathf.Abs(joystick.Horizontal) + Mathf.Abs(joystick.Vertical) + Convert.ToInt16(isBoosting)) * 20);
                mainsettings.startLifetime = ((Mathf.Abs(joystick.Horizontal) + Mathf.Abs(joystick.Vertical) + Convert.ToInt16(isBoosting)));
                Vector2 targetposition = joystick.Direction;
                transform.up = Vector2.SmoothDamp(transform.up, targetposition, ref currentRotation, turnSpeed);
            }
            else
            {
                if (isBoosting)
                {
                    rb.velocity = Vector2.SmoothDamp(rb.velocity, transform.up * 10, ref currentVelocity, 1);
                }

                if (source.volume > 0.05f * PlayerPrefs.GetFloat("SFX Volume"))
                {
                    source.volume -= 0.005f;
                }
                mainsettings.startLifetime = 0.1f + Convert.ToInt16(isBoosting);
            }
        }
        else
        {
            rb.velocity = Vector2.SmoothDamp(rb.velocity, new Vector2(0, 0), ref currentVelocity, 1);
        }


        // Camera follow
        Vector3 newPos = transform.position;
        newPos.z = mainCamera.position.z;
        mainCamera.position = newPos;
    }

}
