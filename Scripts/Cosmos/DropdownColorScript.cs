using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownColorScript : MonoBehaviour
{
    public Color lightRed = new Color();
    public Color lightBlue = new Color();
    public Color lightGreen = new Color();
    // Start is called before the first frame update
    void Start()
    {
        if (transform.parent.name.Substring(8) == "Station")
        {
            GetComponent<Image>().color = lightBlue;
        }
        else
        {
            if (PlayerPrefs.GetInt(transform.parent.name.Substring(8) + " Completed") == 1)
            {
                GetComponent<Image>().color = lightGreen;
            }
            else
            {
                GetComponent<Image>().color = lightRed;
            }
        }
    }
}
