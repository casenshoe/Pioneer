using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ThrusterHologramScript : MonoBehaviour
{
    public Camera cam;
    public LineRenderer line;
    public Transform previewPosition;
    public TextMeshProUGUI thrusterName;
    public Sprite[] spriteImages = new Sprite[4];
    public SpriteRenderer thrusterPreview;
    public Vector3 tempPosition;
    public int count = 0;
    public GameObject stationScript;
    public GameObject buyOption;
    public TextMeshProUGUI metalCountText;
    public TextMeshProUGUI price;
    public Slider speedSlider;
    public Slider fuelSlider;

    public List<GameObject> thrustPreviews;

    private Dictionary<int, string> thrusterNames = new Dictionary<int, string>()
    {
        {0, "Nebula Voyager Rocket"},
        {1, "Flare Thruster"},
        {2, "Pulse Engine"},
        {3, "Burst Engine"},
        {4, "Supercharged Thruster"},
    };

    private Dictionary<int, int> thrusterPrices = new Dictionary<int, int>()
    {
        {1, 50},
        {2, 100},
        {3, 150},
        {4, 200}
    };

    [SerializeField] private TMP_Dropdown dropdown;
    public int thrusterNum = 0;
    public Image planetDisplay;

    public void GetDropDownValue()
    {
        for(int i = 0; i <= thrustPreviews.Count - 1; i++)
        {
            thrustPreviews[i].SetActive(false);
        }
        thrusterNum = dropdown.value;
        thrustPreviews[thrusterNum].SetActive(true);
        if (!stationScript.GetComponent<StationScript>().purchasedThrusters.Contains(thrusterNum) && thrusterNum != 0)
        {
            price.text = thrusterPrices[thrusterNum].ToString() + " cr.";
            buyOption.SetActive(true);
        }
        else
        {
            PlayerPrefs.SetInt("Thruster Selection", thrusterNum);
            buyOption.SetActive(false);
        }
        planetDisplay.sprite = spriteImages[thrusterNum];
        thrusterName.text = dropdown.options[thrusterNum].text;
        thrusterName.enabled = true;
        if (thrusterNum != 0)
        {
            if (thrusterNum == 1)
            {
                thrusterPreview.transform.position = new Vector3(0f, -0.24f, 0f);
            }
            else if (thrusterNum == 2)
            {
                thrusterPreview.transform.position = new Vector3(0f, -0.28f, 0f);
            }
            else if (thrusterNum == 3)
            {
                thrusterPreview.transform.position = new Vector3(0f, -0.06f, 0f);
            }
            else
            {
                thrusterPreview.transform.position = new Vector3(0f, -0.16f, 0f);
            }
            thrusterPreview.sprite = spriteImages[thrusterNum];
        }
        else
        {
            thrusterPreview.sprite = null;
        }
        speedSlider.value = .2f * (dropdown.value + 1);
        fuelSlider.value = .2f * (dropdown.value + 1);
    }

    public void buyThruster()
    {
        if (PlayerPrefs.GetInt("Metal Count") >= thrusterPrices[dropdown.value])
        {
            if (dropdown.value == 1)
            {
                PlayerPrefs.SetInt("Flare Thruster Purchased", 1);
            }
            else if (dropdown.value == 2)
            {
                PlayerPrefs.SetInt("Pulse Engine Purchased", 1);
            }
            else if (dropdown.value == 3)
            {
                PlayerPrefs.SetInt("Burst Engine Purchased", 1);
            }
            else if (dropdown.value == 4)
            {
                PlayerPrefs.SetInt("Supercharged Thruster Purchased", 1);
            }
            buyOption.SetActive(false);
            stationScript.GetComponent<StationScript>().purchasedShields.Add(dropdown.value);
            PlayerPrefs.SetInt("Metal Count", PlayerPrefs.GetInt("Metal Count") - thrusterPrices[dropdown.value]);
            metalCountText.text = PlayerPrefs.GetInt("Metal Count").ToString() + " credits";
        }
    }

    private void Start()
    {
        tempPosition = cam.ScreenToWorldPoint(transform.position);
        tempPosition += new Vector3(1f, 0.5f, 0);
        line.SetPosition(0, tempPosition);
        tempPosition = previewPosition.position;
        tempPosition += new Vector3(-0.1f, -0.7f, 0);
        line.SetPosition(1, tempPosition);
        speedSlider.value = .2f * (dropdown.value + 1);
        fuelSlider.value = .2f * (dropdown.value + 1);
    }
}
