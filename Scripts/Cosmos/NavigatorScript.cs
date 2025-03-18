using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NavigatorScript : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown;
    public Sprite[] spriteImages;
    public TextMeshProUGUI planetName;
    public int planetNum = 0;
    public Image planetDisplay;

    public void GetDropDownValue()
    {
        planetNum = dropdown.value;
        planetDisplay.sprite = spriteImages[planetNum];
        if (planetNum == 0)
        {
            planetName.text = "Station";
        }
        else if (planetNum == 1)
        {
            planetName.text = "Solus Deus";
        }
        else
        {
            if (PlayerPrefs.GetInt(dropdown.options[planetNum - 1].text + " Completed") == 1)
            {
                planetName.text = dropdown.options[planetNum].text;
            }
            else
            {
                planetName.text = "???";
            }
        }
    }

}
