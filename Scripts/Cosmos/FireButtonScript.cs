using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class FireButtonScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject controller;
    public bool buttonPressed;
    public float timeBetweenFiring = 0.3f;

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
            controller.GetComponent<GameController>().shoot();
            wait(timeBetweenFiring);
        }
    }
}
