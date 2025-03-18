using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TargetIndicator : MonoBehaviour
{
    public Transform Target;
    public GameObject navigator;
    //public TextMeshProUGUI distanceTracker;
    public Transform[] Targets = new Transform[9];

    void Update()
    {
        Target = Targets[navigator.GetComponent<NavigatorScript>().planetNum];
        var dir = Target.position - transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        //distanceTracker.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        //distanceTracker.text = Mathf.Round(Vector3.Distance(this.transform.position, Target.position)).ToString();
    }
}
