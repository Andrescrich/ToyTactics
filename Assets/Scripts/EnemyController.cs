﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using TMPro;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class EnemyController : MonoBehaviour
{
    public Transform currentCube;
    //Estas listas son los posibles nodos en los que se puede mover (se quitan los que tienen otra pieza en ellos)
    public List<Transform> nextCubesToMove = new List<Transform>();
    public List<Transform> nextNextCubesToMove = new List<Transform>();
    //Estas listas son el recuento de todos los nodos (se usa para comprobar que otros enemigos estan en x rango)
    public List<Transform> cubesOnRange1 = new List<Transform>();
    public List<Transform> cubesOnRange2 = new List<Transform>();
    private bool moving;
    public Transform objectiveCube;
    private bool possiblePath;
    private bool enableInput;
    public List<PlayerController> playersToAttack;
    public GameObject objective;
    public Material normalMaterial;
    public Material enemyMaterial;
    private Animator anim;
    private bool isMoving;
    private static readonly int Walk = Animator.StringToHash("Walk");
    private static readonly int Attack1 = Animator.StringToHash("Attack");

    private void Awake()
    {
        RayCastDown();
        anim = GetComponentInChildren<Animator>();
    }

    private void OnEnable()
    {
        Clear();
        RayCastDown();
        FindPath();
        StartCoroutine(Action());
    }

    private void Update()
    {
        anim.SetBool(Walk, isMoving);
    }

    private IEnumerator Action()
    {
        yield return new WaitForSeconds(1);
        SelectObjective();
        if (cubesOnRange1.Contains(objective.GetComponent<PlayerController>().currentCube))
        {
            transform.LookAt(new Vector3(objective.transform.position.x, transform.transform.position.y, objective.transform.position.z));
            anim.SetTrigger(Attack1);
            yield return new WaitForSeconds(1);
            yield break;
        }
        currentCube.GetComponent<MeshRenderer>().sharedMaterial = normalMaterial;
        var objectiveCube1 = nextCubesToMove.First();
        objectiveCube = nextCubesToMove.First();
        foreach (var nextCube in nextCubesToMove)
        {
            if (Vector3.Distance(objective.GetComponent<PlayerController>().currentCube.transform.position, objectiveCube.position) >
                Vector3.Distance(objective.GetComponent<PlayerController>().currentCube.transform.position, nextCube.position))
            {
                objectiveCube = nextCube;
                objectiveCube1 = nextCube;
            }
        }
        foreach (var nextCube in nextNextCubesToMove)
        {
            if (Vector3.Distance(objective.GetComponent<PlayerController>().currentCube.transform.position, objectiveCube.position) >
                Vector3.Distance(objective.GetComponent<PlayerController>().currentCube.transform.position, nextCube.position))
            {
                objectiveCube = nextCube;
            }
        }

        isMoving = true;
        var lookPos = new Vector3(objectiveCube1.position.x, transform.position.y, objectiveCube1.position.z);
        transform.LookAt(lookPos);

        while (transform.position != objectiveCube1.GetComponent<Walkable>().nodePos)
        {
            transform.position = Vector3.MoveTowards(transform.position, objectiveCube1.GetComponent<Walkable>().nodePos,
                5f * Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
        }

        var lookPos2 = new Vector3(objectiveCube.position.x, transform.position.y, objectiveCube.position.z);
        transform.LookAt(lookPos2);
        while (transform.position != objectiveCube.GetComponent<Walkable>().nodePos)
        {
            transform.position = Vector3.MoveTowards(transform.position, objectiveCube.GetComponent<Walkable>().nodePos,
                5f * Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
        }

        isMoving = false;
        Clear();
        RayCastDown();
        FindPath();
        SelectObjective();
        if (cubesOnRange1.Contains(objective.GetComponent<PlayerController>().currentCube))
        {
            transform.LookAt(new Vector3(objective.transform.position.x, transform.transform.position.y, objective.transform.position.z));
            anim.SetTrigger(Attack1);
            yield return new WaitForSeconds(1);
            yield break;
        }
        yield return new WaitForSeconds(1);
        TurnOver();
    }
    
    
    
    private void RayCastDown()
    {
        var playerRay = new Ray(transform.position, -transform.up);
        if (Physics.Raycast(playerRay, out var playerHit))
        {
            if (playerHit.transform.GetComponent<Walkable>() != null)
            {
                currentCube = playerHit.transform;
            }
        }

        currentCube.GetComponent<Renderer>().sharedMaterial = enemyMaterial;
    }
    
    private void FindPath()
    {
        foreach (var path in currentCube.GetComponent<Walkable>().possiblePaths)
        {
            if (path.active)
            {
                cubesOnRange1.Add(path.target);
                if (path.target.GetComponent<Walkable>().pieceOnNode.Length == 0)
                    nextCubesToMove.Add(path.target);
            }

        }
        foreach (var p in nextCubesToMove)
        {
            foreach (var path2 in p.GetComponent<Walkable>().possiblePaths)
            {
                if (!nextCubesToMove.Contains(path2.target) && path2.active && currentCube != path2.target &&
                    !cubesOnRange2.Contains(path2.target))
                {
                    cubesOnRange2.Add(path2.target);
                    if (path2.target.GetComponent<Walkable>().pieceOnNode.Length == 0 && !nextNextCubesToMove.Contains(path2.target)) 
                        nextNextCubesToMove.Add(path2.target);
                }
            }
        }
    }

    public void Attack()
    {
        objective.gameObject.GetComponent<UnitStatus>().ChangeHealth(-gameObject.GetComponent<UnitStatus>().damage);
        TurnOver();
    }

    private void Clear()
    { 
        cubesOnRange1.Clear();
        cubesOnRange2.Clear();
        nextCubesToMove.Clear();
        nextNextCubesToMove.Clear();
        playersToAttack.Clear();
    }

    private void SelectObjective()
    {
        playersToAttack.AddRange(FindObjectsOfType<PlayerController>());
        objective = playersToAttack.First().gameObject;
        foreach (var playerToAttack in playersToAttack)
        {
            if (Vector3.Distance(playerToAttack.gameObject.transform.position, gameObject.transform.position) <
                Vector3.Distance(objective.transform.position, gameObject.transform.position))
            {
                objective = playerToAttack.gameObject;
            }
                
        }
    }

    private void TurnOver()
    {
        GetComponent<Player>().RemoveFromPlayable();
        GameManager.instance.NextEnemyTurn();
        enabled = false;
    }

    private void OnDestroy()
    {
        currentCube.GetComponent<Renderer>().sharedMaterial = normalMaterial;
    }
}
