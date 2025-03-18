using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShieldHologramScript : MonoBehaviour
{
    public Camera cam;
    public LineRenderer line;
    public Transform previewPosition;
    public TextMeshProUGUI shieldName;
    public Sprite[] spriteImages = new Sprite[4];
    public SpriteRenderer shieldPreview;
    public Vector3 tempPosition;
    public int count = 0;
    public GameObject stationScript;
    public GameObject buyOption;
    public TextMeshProUGUI metalCountText;
    public TextMeshProUGUI price;
    public Slider shieldSlider;

    private Dictionary<int, string> shieldNames = new Dictionary<int, string>()
    {
        {0, "No Shield"},
        {1, "Pulsar Guard"},
        {2, "Nebula Shield"},
        {3, "Solar Protector"},
        {4, "Astral Protector"},
    };

    private Dictionary<int, int> shieldPrices = new Dictionary<int, int>()
    {
        {1, 50},
        {2, 100},
        {3, 150},
        {4, 200}
    };

    [SerializeField] private TMP_Dropdown dropdown;
    public int shieldNum = 0;
    public Image planetDisplay;

    public void GetDropDownValue()
    {
        shieldNum = dropdown.value;
        if (!stationScript.GetComponent<StationScript>().purchasedShields.Contains(shieldNum) && shieldNum != 0)
        {
            price.text = shieldPrices[shieldNum].ToString() + " cr.";
            buyOption.SetActive(true);
        }
        else
        {
            PlayerPrefs.SetInt("Shield Selection", shieldNum);
            buyOption.SetActive(false);
        }
        planetDisplay.sprite = spriteImages[shieldNum];
        shieldName.text = dropdown.options[shieldNum].text;
        shieldName.enabled = true;
        shieldPreview.sprite = spriteImages[shieldNum];
        shieldSlider.value = .2f * (dropdown.value + 1);
    }

    public void buyShield()
    {
        if (PlayerPrefs.GetInt("Metal Count") >= shieldPrices[dropdown.value])
        {
            if (dropdown.value == 1)
            {
                PlayerPrefs.SetInt("Pulsar Guard Purchased", 1);
            }
            else if (dropdown.value == 2)
            {
                PlayerPrefs.SetInt("Nebula Shield Purchased", 1);
            }
            else if (dropdown.value == 3)
            {
                PlayerPrefs.SetInt("Solar Protector Purchased", 1);
            }
            else if (dropdown.value == 4)
            {
                PlayerPrefs.SetInt("Astral Protector Purchased", 1);
            }
            buyOption.SetActive(false);
            stationScript.GetComponent<StationScript>().purchasedShields.Add(dropdown.value);
            PlayerPrefs.SetInt("Metal Count", PlayerPrefs.GetInt("Metal Count") - shieldPrices[dropdown.value]);
            metalCountText.text = PlayerPrefs.GetInt("Metal Count").ToString() + " credits";
        }
    }

    private void Start()
    {
        tempPosition = cam.ScreenToWorldPoint(transform.position);
        tempPosition += new Vector3(1.3f, -1.6f, 0);
        line.SetPosition(0, tempPosition);
        tempPosition = previewPosition.position;
        tempPosition += new Vector3(-0.5f, 0.4f, 0);
        line.SetPosition(1, tempPosition);
        shieldSlider.value = .2f * (dropdown.value + 1);
    }
}
