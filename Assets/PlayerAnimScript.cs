using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimScript : MonoBehaviour
{
    private PlayerController pComp;

    private void Awake()
    {
        pComp = GetComponentInParent<PlayerController>();
    }

    public void Attack()
    {
        pComp.Attack(pComp.clickedEnemy);
    }
}
