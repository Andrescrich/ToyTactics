using System;
using System.Collections.Generic;
using UnityEngine;

public class Walkable : MonoBehaviour
{
    public List<GamePath> possiblePaths = new List<GamePath>();
    private Collider[] hitCollider;
    public Collider[] pieceOnNode;
    [HideInInspector] public Vector3 nodePos;
    public LayerMask ground;

    private void Awake()
    {
        Player.EndOfTurnEvent += CheckWhoIsOnNode;
        nodePos = transform.position + transform.forward * .5f;
        hitCollider = Physics.OverlapSphere(transform.position, 1.5f, ground);
        CheckWhoIsOnNode();
        foreach (var collision in hitCollider)
        {
            if (collision.gameObject != gameObject) 
                possiblePaths.Add(new GamePath(collision.transform, true));
        }
    }
    
    private void OnDrawGizmos()
    {
    //    Gizmos.DrawLine(transform.position, transform.up);
        Gizmos.color = Color.gray;
        Gizmos.DrawSphere(transform.position + transform.forward * .5f, .1f);
        foreach (var nextNode in possiblePaths)
        {
            if (nextNode.active)
            {
                Gizmos.DrawLine(transform.position, nextNode.target.position);
            }
        }
    }

    private void CheckWhoIsOnNode()
    {
        pieceOnNode = Physics.OverlapSphere(transform.position + transform.forward * .5f, .1f);
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
