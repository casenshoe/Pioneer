using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BoostButtonScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject controller;
    public bool buttonPressed;
    public float timeBetweenFiring = 0.3f;
    public float maxBoost, boost = 5f;
    public Image boostImage;

    public IEnumerator wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        buttonPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonPressed = false;
        //currently sitting in the pden eating moes and chasing dreams
    }

    public void Update()
    {
        if (buttonPressed)
        {
            if (boost > 0)
            {
                boost -= Time.deltaTime;
                boostImage.fillAmount = boost / maxBoost;
            }
        }
        else
        {
            if (boost < maxBoost)
            {
                boost += Time.deltaTime / 2;
                boostImage.fillAmount = boost / maxBoost;
            }
        }
    }
}
