using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BlasterHologramScript : MonoBehaviour
{
    public Sprite[] spriteImages = new Sprite[3];
    public TextMeshProUGUI blasterName;
    public Transform previewPosition;
    public GameObject blasterPreview;
    public GameObject blasterPreview1;
    public GameObject blasterPreview2;
    public LineRenderer line;
    public Camera cam;
    public Vector3 tempPosition;
    public int count = 0;
    public GameObject stationScript;
    public GameObject buyOption;
    public TextMeshProUGUI metalCountText;
    public TextMeshProUGUI price;
    public Slider blasterSlider;

    private Dictionary<int, string> blasterNames = new Dictionary<int, string>()
    {
        {0, "Plasma Cannon"},
        {1, "Twin Phasers"},
        {2, "Vacuum Torpedoes"},
    };

    private Dictionary<int, int> blasterPrices = new Dictionary<int, int>()
    {
        {1, 125},
        {2, 200},
    };

    [SerializeField] private TMP_Dropdown dropdown;
    public int blasterNum = 0;
    public Image planetDisplay;

    private void Start()
    {
        if (PlayerPrefs.GetInt("Twin Phasers Purchased") == 1) 
        {
            blasterPreview1.SetActive(true);
            dropdown.value = 1;
        }
        else
        {
            blasterPreview2.SetActive(false);
        }

        if (PlayerPrefs.GetInt("Vacuum Torpedoes Purchased") == 2)
        {
            blasterPreview2.SetActive(true);
            dropdown.value = 2;
        }
        else
        {
            blasterPreview2.SetActive(false);
        }
        tempPosition = cam.ScreenToWorldPoint(transform.position);
        tempPosition += new Vector3(-0.8f, -0.3f, 0);
        line.SetPosition(0, tempPosition);
        tempPosition = previewPosition.position;
        tempPosition += new Vector3(0.25f, 0.6f, 0);
        line.SetPosition(1, tempPosition);
        blasterSlider.value = .33f * (dropdown.value + 1);
    }

    public void GetDropDownValue()
    {
        blasterNum = dropdown.value;
        if (blasterNum != 0)
        {
            if (blasterNum == 1)
            {
                if (PlayerPrefs.GetInt("Twin Phasers Purchased") == 1)
                {
                    PlayerPrefs.SetInt("Blaster Selection", blasterNum);
                    buyOption.SetActive(false);
                }
                else
                {
                    price.text = blasterPrices[blasterNum].ToString() + " cr.";
                    buyOption.SetActive(true);
                }
            }
            else
            {
                if (PlayerPrefs.GetInt("Vacuum Torpedoes Purchased") == 1)
                {
                    PlayerPrefs.SetInt("Blaster Selection", blasterNum);
                    buyOption.SetActive(false);
                }
                else
                {
                    price.text = blasterPrices[blasterNum].ToString() + " cr.";
                    buyOption.SetActive(true);
                }
            }

        }
        else
        {
            PlayerPrefs.SetInt("Blaster Selection", blasterNum);
            buyOption.SetActive(false);
        }
        planetDisplay.sprite = spriteImages[blasterNum];
        blasterName.text = dropdown.options[blasterNum].text;
        blasterName.enabled = true;

        if (blasterNum == 1)
        {
            blasterPreview1.SetActive(true);
            if (!stationScript.GetComponent<StationScript>().purchasedBlasters.Contains(2))
            {
                blasterPreview2.SetActive(false);
            }
        }
        else if (blasterNum == 2)
        {
            blasterPreview2.SetActive(true);
            if (!stationScript.GetComponent<StationScript>().purchasedBlasters.Contains(1))
            {
                blasterPreview1.SetActive(false);
            }
        }
        blasterSlider.value = .33f * (dropdown.value + 1);
    }

    public void buyBlaster()
    {
        if (PlayerPrefs.GetInt("Metal Count") >= blasterPrices[dropdown.value])
        {
            if (dropdown.value == 1)
            {
                PlayerPrefs.SetInt("Twin Phasers Purchased", 1);
            }
            else if (dropdown.value == 2)
            {
                PlayerPrefs.SetInt("Vacuum Torpedoes Purchased", 1);
            }

            buyOption.SetActive(false);
            stationScript.GetComponent<StationScript>().purchasedShields.Add(dropdown.value);
            PlayerPrefs.SetInt("Metal Count", PlayerPrefs.GetInt("Metal Count") - blasterPrices[dropdown.value]);
            metalCountText.text = PlayerPrefs.GetInt("Metal Count").ToString() + " credits";
        }
    }
}