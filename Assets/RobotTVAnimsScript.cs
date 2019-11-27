using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotTVAnimsScript : MonoBehaviour
{
    private PlayerController pComp;

    private void Awake()
    {
        pComp = GetComponentInParent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack()
    {
        pComp.Attack(pComp.clickedEnemy);
    }
}
