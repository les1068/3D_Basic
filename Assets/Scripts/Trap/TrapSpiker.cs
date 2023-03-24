using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSpiker : TrapBase
{
    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    protected override void OnTrapActivate(GameObject target)
    {
        base.OnTrapActivate(target);
        anim.SetTrigger("Activate");
    }
}
