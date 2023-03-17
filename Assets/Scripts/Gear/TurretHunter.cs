using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TurretHunter : Turret
{
    Transform target = null;
    SphereCollider sightTrigger;
    protected override void Awake()
    {
        base.Awake();
        sightTrigger = GetComponent<SphereCollider>();
    }
    private void Update()
    {
        LookTarget();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            target = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            target = null;
        }
    }
    private void LookTarget()
    {
        if (IsVisibleTarget())
        {

            Vector3 dir = target.transform.position - barrelBodyTransform.position;
            dir.y = 0;
            barrelBodyTransform.forward = dir;

            //Vector3 lookAt = other.transform.position + Vector3.up * barrelBodyTransform.position.y;
            //barrelBodyTransform.LookAt(lookAt);
        }
    }

    bool IsVisibleTarget()
    {
        bool result = false;
        if (target != null)
        {
            Ray ray = new Ray(barrelBodyTransform.position, barrelBodyTransform.forward);
            if(Physics.Raycast(ray, out RaycastHit hitInfo, sightTrigger.radius ))
            {
                //Debug.Log($"충돌:{ hitInfo.collider.gameObject.name}");
                if (hitInfo.transform == target)
                {
                    result = true;

                }
            }
        }
        return result;
    }
}
