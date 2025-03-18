using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using TMPro;
public class SceneTransitions : MonoBehaviour
{
    public Animator transitionAnim;
    public string sceneName;
    public AudioSource music;
    public GameObject ship;
    public GameObject confirmResetButton;
    public GameObject audioSliders;
    private Scene scene;
    public TextMeshProUGUI outcomeText;
    public TextMeshProUGUI metalAcquiredText;
    public TextMeshProUGUI creditsText;
    public TextMeshProUGUI quipText;
    public TextMeshProUGUI confirmText;
    [SerializeField] private TMP_Dropdown shieldDropdown;
    [SerializeField] private TMP_Dropdown blasterDropdown;
    [SerializeField] private TMP_Dropdown thrusterDropdown;
    public GameObject stationScript;

    private Dictionary<int, string> extractionQuips = new Dictionary<int, string>()
    {
        {0, "Excellent work, Pioneer."},
        {1, "Thank you for leading Earth into the unknown."},
        {2, "Welcome back, pilot."},
        {3, "Welcome home, astronaut."},
        {4, "Spend those shiny credits on some new gear."},
        {5, "Welcome back from your mission, Pioneer."},
        {6, "Your name will be remembered, Pioneer." }
    };

    private void Start()
    {
        // If first time opening:
        /*
        if (!PlayerPrefs.HasKey("FIRSTTIMEOPENING"))
        {
            //Set first time opening to false
            PlayerPrefs.SetInt("FIRSTTIMEOPENING", 0);

            //Do your stuff here
            PlayerPrefs.SetInt("Metal Count", 0);
            PlayerPrefs.SetInt("Pulsar Guard Purchased", 0);
            PlayerPrefs.SetInt("Nebula Shield Purchased", 0);
            PlayerPrefs.SetInt("Solar Protector Purchased", 0);
            PlayerPrefs.SetInt("Astral Protector Purchased", 0);

            PlayerPrefs.SetInt("Flare Thruster Purchased", 0);
            PlayerPrefs.SetInt("Pulse Engine Purchased", 0);
            PlayerPrefs.SetInt("Burst Engine Purchased", 0);
            PlayerPrefs.SetInt("Supercharged Thruster Purchased", 0);

            PlayerPrefs.SetInt("Twin Phasers Purchased", 0);
            PlayerPrefs.SetInt("Vacuum Torpedoes Purchased", 0);
        }
        */

        scene = SceneManager.GetActiveScene();
        if (scene.name == "Station")
        {
            shieldDropdown.value = PlayerPrefs.GetInt("Equipped Shield");
            thrusterDropdown.value = PlayerPrefs.GetInt("Equipped Thruster");
        }
        else if (scene.name == "Extraction Screen")
        {
            StartCoroutine(showDebrief());
        }
    }

    public void startLoadCosmos()
    {
        sceneName = "Cosmos";
        if (scene.name == "Station")
        {
            if (stationScript.GetComponent<StationScript>().purchasedShields.Contains(shieldDropdown.value) || shieldDropdown.value == 0)
            {
                PlayerPrefs.SetInt("Equipped Shield", shieldDropdown.value);
            }

            if (stationScript.GetComponent<StationScript>().purchasedThrusters.Contains(thrusterDropdown.value) || thrusterDropdown.value == 0)
            {
                PlayerPrefs.SetInt("Equipped Thruster", thrusterDropdown.value);
            }
        }
        Time.timeScale = 1;
        StartCoroutine(LoadScene());
    }

    public void startLoadOptions()
    {
        sceneName = "Options";
        Time.timeScale = 1;
        StartCoroutine(LoadScene());
    }

    public void startLoadCredits()
    {
        sceneName = "Credits";
        Time.timeScale = 1;
        StartCoroutine(LoadScene());
    }

    public void startLoadMainMenu()
    {
        sceneName = "MainMenu";
        Time.timeScale = 1;
        StartCoroutine(LoadScene());
    }

    public void startLoadExtractionScreenButton()
    {
        PlayerPrefs.SetString("Outcome", "Extracting to Station");
        startLoadExtractionScreen();
    }

    public void startLoadExtractionScreen()
    {
        sceneName = "Extraction Screen";
        PlayerPrefs.SetInt("Metal Extracted", ship.GetComponent<GameController>().metalCount);
        Time.timeScale = 1;
        StartCoroutine(LoadScene());
    }

    public void startLoadStation()
    {
        sceneName = "Station";
        Time.timeScale = 1;
        StartCoroutine(LoadScene());
    }

    IEnumerator showDebrief()
    {
        string outcome = PlayerPrefs.GetString("Outcome");
        string metalAcquired = "Metal Acquired: ";
        string credits = ("Credits: " + PlayerPrefs.GetInt("Metal Count").ToString() + " + " + PlayerPrefs.GetInt("Metal Extracted").ToString() + " --> " + (PlayerPrefs.GetInt("Metal Count") + PlayerPrefs.GetInt("Metal Extracted")));

        for (int i = 0; i <= outcome.Length - 1; i++)
        {
            outcomeText.text += outcome[i];
            yield return new WaitForSeconds(0.02f);
        }

        if (PlayerPrefs.GetString("Outcome") == "Out of Fuel")
        {
            // print metal acquired
            for (int i = 0; i <= metalAcquired.Length - 1; i++)
            {
                metalAcquiredText.text += metalAcquired[i];
                yield return new WaitForSeconds(0.02f);
            }
            yield return new WaitForSeconds(0.5f);
            metalAcquiredText.text += "0";
            yield return new WaitForSeconds(0.5f);

            // print new credit count
            for (int i = 0; i <= credits.Length - 1; i++)
            {
                creditsText.text += credits[i];
                yield return new WaitForSeconds(0.02f);
            }
            yield return new WaitForSeconds(0.5f);

            string quip = "You have run out of fuel.\nA rescue crew will tow you back to the station.";

            for (int i = 0; i <= quip.Length - 1; i++)
            {
                quipText.text += quip[i];
                yield return new WaitForSeconds(0.02f);
            }
        }
        else if (PlayerPrefs.GetString("Outcome") == "Extracting to Station")
        {
            string quip = extractionQuips[Random.RandomRange(0, extractionQuips.Count - 1)];

            PlayerPrefs.SetInt(PlayerPrefs.GetString("Planet Completed"), 1);

            for (int i = 0; i <= metalAcquired.Length - 1; i++)
            {
                metalAcquiredText.text += metalAcquired[i];
                yield return new WaitForSeconds(0.02f);
            }

            metalAcquiredText.text += PlayerPrefs.GetInt("Metal Extracted");
            PlayerPrefs.SetInt("Metal Count", PlayerPrefs.GetInt("Metal Count") + PlayerPrefs.GetInt("Metal Extracted"));

            for (int i = 0; i <= credits.Length - 1; i++)
            {
                creditsText.text += credits[i];
                yield return new WaitForSeconds(0.02f);
            }

            for (int i = 0; i <= quip.Length - 1; i++)
            {
                quipText.text += quip[i];
                yield return new WaitForSeconds(0.02f);
            }
        }
        else // player shot down
        {
            credits = ("Credits: " + PlayerPrefs.GetInt("Metal Count").ToString() + " + 0 --> " + (PlayerPrefs.GetInt("Metal Count") + 0));
            for (int i = 0; i <= metalAcquired.Length - 1; i++)
            {
                metalAcquiredText.text += metalAcquired[i];
                yield return new WaitForSeconds(0.02f);
            }
            yield return new WaitForSeconds(0.5f);
            metalAcquiredText.text += "0";
            yield return new WaitForSeconds(0.5f);

            string quip = "You have been shot down and your ship needs repairs. A rescue crew will tow you back to the station.";

            for (int i = 0; i <= credits.Length - 1; i++)
            {
                creditsText.text += credits[i];
                yield return new WaitForSeconds(0.02f);
            }
            yield return new WaitForSeconds(0.5f);

            for (int i = 0; i <= quip.Length - 1; i++)
            {
                quipText.text += quip[i];
                yield return new WaitForSeconds(0.02f);
            }
        }
    }

    IEnumerator LoadScene()
    {
        music.volume = Mathf.Lerp(music.volume, 0, 0.9f); // start volume, target volume, time
        transitionAnim.SetTrigger("end");
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(sceneName);
    }

    public void resetProgress()
    {
        confirmResetButton.SetActive(true);
        audioSliders.SetActive(false);
    }

    public void confirmResetProgress()
    {
        confirmText.text = "Progress has been reset.";
        PlayerPrefs.DeleteAll();
    }

    public void showAudioSliders()
    {
        audioSliders.SetActive(true);
        confirmResetButton.SetActive(false);
    }

}