using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Station_Main_Menu_Script : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.back * 2 * Time.deltaTime);
    }
}
