using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using TreeEditor;
using UnityEngine;

public class Walkable : MonoBehaviour
{
    public List<GamePath> possiblePaths = new List<GamePath>();
    private Collider[] hitCollider;
    [HideInInspector] public Vector3 nodePos;
    public LayerMask ground;
    
    private void Awake()
    {
        nodePos = transform.position + transform.up * 1.5f + -transform.right;
        hitCollider = Physics.OverlapSphere(transform.position + -transform.right + transform.up * .5f, 1.5f, ground);
        foreach (var collision in hitCollider)
        {
            if (collision.gameObject != gameObject) 
                possiblePaths.Add(new GamePath(collision.transform, true));
        }
    }
    
    private void OnDrawGizmos()
    {
      //  Gizmos.DrawSphere(transform.position + -transform.right + transform.up*.5f, 1.5f);
        Gizmos.color = Color.gray;
        Gizmos.DrawSphere(transform.position + transform.up*1.5f + -transform.right, .1f);
        foreach (var nextNode in possiblePaths)
        {
            if (nextNode.active)
            {
                Gizmos.DrawLine(transform.position + transform.up + -transform.right, nextNode.target.position
             + nextNode.target.up + -nextNode.target.right);
            }
        }
    }
}

[Serializable]
public class GamePath
{
    public Transform target;
    public bool active = true;

    public GamePath(Transform auxTarget, bool auxActive)
    {
        target = auxTarget;
        active = auxActive;
    }
}
