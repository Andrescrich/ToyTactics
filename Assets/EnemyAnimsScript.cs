using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimsScript : MonoBehaviour
{
    private EnemyController eComp;

    private void Awake()
    {
        eComp = GetComponentInParent<EnemyController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack()
    {
        eComp.Attack();
    }
}
