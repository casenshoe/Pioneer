using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TextScroll : MonoBehaviour
{
    float speed = 50.0f;
    float textPosBegin = -670.0f;
    float boundaryTextEnd = 2400.0f;
    RectTransform myRectTransform;
    [SerializeField]
    TextMeshProUGUI mainText;
    [SerializeField]
    bool isLooping = false;
    // Start is called before the first frame update
    void Start()
    {
        myRectTransform = gameObject.GetComponent<RectTransform>();
        StartCoroutine(AutoScrollText());
    }
    public IEnumerator AutoScrollText()
    {
        while (myRectTransform.localPosition.y < boundaryTextEnd){
            myRectTransform.Translate(Vector3.up * speed * Time.deltaTime);
            yield return null;
        }
    }

}

