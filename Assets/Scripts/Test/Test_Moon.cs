using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Test_Moon : MonoBehaviour
{
    public Transform Earth;

    private void Update()
    {
        //transform.LookAt(Earth);
        transform.rotation = Quaternion.LookRotation(Earth.position - transform.position);
        //Earth.RotateAround(Earth.position, Earth.up, Time.deltaTime * 120.0f);
    }
}
