using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DoorKey : MonoBehaviour
{
    public DoorBase target;
    Transform keyModel;

    private void Awake()
    {
        keyModel = transform.GetChild(0);
    }
    private void Update()
    {
        transform.RotateAround(transform.position, transform.up, Time.deltaTime*180.0f);
    }
}
