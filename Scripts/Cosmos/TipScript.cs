using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TipScript : MonoBehaviour
{
    public TextMeshProUGUI tipText;
    public bool isDisplayed;
    public TMP_Dropdown dropdown;
    private GameObject runtimeOptionsPanel;
    public Image dropdownImage;
    public TextMeshProUGUI planetName;
    public Image solusDeusImage;
    public GameObject loadBar;
    public GameObject novumInceptor;
    public GameObject blockRaycast;
    public bool hasOpened;
    public Image tipImage;

    private Dictionary<int, string> tips = new Dictionary<int, string>()
    {
        {0, "Welcome to space, Pioneer."},
        {1, "Humanity relies on you to explore this system." },
        {2, "Select a destination with the planet navigator." },
        {3, "Select the coordinates for Solus Deus."},
        {4, "Follow the indicator arrow to reach Solus Deus."},
        {5, "Warning: Planet is guarded by a tough enemy!"},
        {6, "Stay over the planet to spawn the Novum Inceptor."},
        {7, "Fly back to the station to extract!"},
    };

    public IEnumerator displayTipText(string tip)
    {
        for (int i = 0; i <= tip.Length - 1; i++)
        {
            tipText.text += tip[i];
            yield return new WaitForSeconds(0.03f);
        }
        tipText.text = tip;
    }

    public IEnumerator displayTip(int tipNum)
    {
        StartCoroutine(displayTipText(tips[tipNum]));
        if (tipNum == 2)
        {
            blockRaycast.SetActive(false);
            yield return StartCoroutine(LerpDropdownColor());
            dropdownImage.color = Color.white; // Ensure final color is white
        }
        else if (tipNum == 3)
        {
            yield return StartCoroutine(LerpSolusColor());
            solusDeusImage.color = Color.white; // Ensure final color is white
        }
        else if (tipNum == 4)
        {
            yield return StartCoroutine(WaitForPlanetContact());
        }
        else if (tipNum == 6)
        {
            yield return new WaitForSeconds(5);
            yield return StartCoroutine(WaitForSpawnBoss());
            this.gameObject.SetActive(false);
        }
        else
        {
            yield return new WaitForSeconds(5);
        }

        tipText.text = "";
        if (tips.ContainsKey(tipNum + 1))
        {
            StartCoroutine(displayTip(tipNum + 1));
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private IEnumerator LerpDropdownColor()
    {
        float lerpTime = 0f;
        while (!hasOpened)
        {
            // Use Mathf.PingPong with accumulated time
            lerpTime += Time.deltaTime;
            dropdownImage.color = Color.Lerp(Color.yellow, Color.white, Mathf.PingPong(lerpTime, 1));
            yield return null; // Yield to let Unity process the next frame
        }
    }

    private IEnumerator LerpSolusColor()
    {
        float lerpTime = 0f;
        while (planetName.text != "Solus Deus")
        {
            if (hasOpened)
            {
                Transform content = runtimeOptionsPanel.transform.Find("Viewport/Content/Item 1: Solus Deus");
                Transform item = content.GetChild(2);
                solusDeusImage = item.GetComponent<Image>();

                // Use Mathf.PingPong with accumulated time
                lerpTime += Time.deltaTime;
                solusDeusImage.color = Color.Lerp(Color.yellow, Color.white, Mathf.PingPong(lerpTime, 1));

                yield return null; // Yield to let Unity process the next frame
            }
            else
            {
                yield return null;
            }
        }
    }

    private IEnumerator WaitForPlanetContact()
    {
        while (!loadBar.activeSelf)
        {
            yield return null; // Yield to let Unity process the next frame
        }
    }

    private IEnumerator WaitForSpawnBoss()
    {
        while (!novumInceptor.activeInHierarchy)
        {
            yield return null; // Yield to let Unity process the next frame
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    public void Update()
    {
        if (runtimeOptionsPanel == null)
        {
            hasOpened = false;
            runtimeOptionsPanel = dropdown.transform.Find("Dropdown List")?.gameObject;
        }
        if (runtimeOptionsPanel != null && runtimeOptionsPanel.activeSelf)
        {
            hasOpened = true;
        }

        if (!isDisplayed && PlayerPrefs.GetString("Planet Completed") == "Solus Deus Completed")
        {
            StartCoroutine(displayTip(7));
            isDisplayed = true;
        }
    }
}
