using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlanetNameScript : MonoBehaviour
{
    public TextMeshProUGUI planetName;
    [SerializeField] private TMP_Dropdown dropdown;
    int planetNum;

    // Start is called before the first frame update
    void Start()
    {
        if (transform.parent.name.Substring(8) == "Station")
        {
            planetName.text = "Station";
        }
        else if (transform.parent.name.Substring(8) == "Solus Deus")
        {
            planetName.text = "Solus Deus";
        }
        else
        {
            planetNum = transform.parent.name[5] - '0';
            if (PlayerPrefs.GetInt(dropdown.options[planetNum - 1].text + " Completed") == 1)
            {
                planetName.text = transform.parent.name.Substring(8);
            }
            else
            {
                planetName.text = "???";
            }
        }
    }
}
