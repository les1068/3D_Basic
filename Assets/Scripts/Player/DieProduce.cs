using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieProduce : MonoBehaviour
{
    Player player;
    CinemachineVirtualCamera vCamera;
    CinemachineDollyCart dollyCart;
    private void Start()
    {
        player = FindObjectOfType<Player>();
        player.OnDie += ProduceStart;

        vCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        vCamera.LookAt = player.transform;
        dollyCart = GetComponentInChildren<CinemachineDollyCart>();
    }

    private void ProduceStart()
    {
        transform.position = player.transform.position;
        vCamera.Priority = 100;
        dollyCart.m_Speed = 10;
    }
}
