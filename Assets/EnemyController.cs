﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int nTurns;
    public Transform currentCube;
    public List<Transform> nextCubes = new List<Transform>();
    public List<Transform> nextNextCubes = new List<Transform>();
    private bool moving;
    private Transform objectiveCube;
    private bool possiblePath;
    private bool enableInput;
    public List<PlayerController> playersToAttack;
    public GameObject objective;
    
   /* private void Start()
    {
        RayCastDown();
        FindPath();
        SelectObjective();
    }*/

    // Update is called once per frame
    private void Update()
    {
        
    }

    private void OnEnable()
    {
        RayCastDown();
        FindPath();
        SelectObjective();
        StartCoroutine(Action());
    }

    private IEnumerator Action()
    {
        if (nextCubes.Contains(objective.GetComponent<PlayerController>().currentCube))
        {
            objective.SetActive(false);
        }
        
        yield return new WaitForSeconds(2);
        TurnOver();
    }
    
    
    
    private void RayCastDown()
    {
        var playerRay = new Ray(transform.GetChild(0).position, -transform.up);
        if (Physics.Raycast(playerRay, out var playerHit))
        {
            if (playerHit.transform.GetComponent<Walkable>() != null)
            {
                currentCube = playerHit.transform;
            }
        }
    }
    
    private void FindPath()
    {
        foreach (var path in currentCube.GetComponent<Walkable>().possiblePaths)
        {
            if (path.active)
                nextCubes.Add(path.target);
           
        }
        foreach (var p in nextCubes)
        {
            foreach (var path2 in p.GetComponent<Walkable>().possiblePaths)
            {
                if (!nextCubes.Contains(path2.target) && path2.active && currentCube != path2.target)
                    nextNextCubes.Add(path2.target);
                else  
                    nextNextCubes.Remove(path2.target);
            }
        }
    }
    
    private void ReachedClicked()
    {
        if (transform.position != objectiveCube.GetComponent<Walkable>().nodePos) return;
        possiblePath = false;
        enableInput = true;
        RayCastDown();
        Clear();
        FindPath();
        moving = false;
        nTurns--;
    }
    
    private void Clear()
    { 
        nextCubes.Clear();
        nextNextCubes.Clear();
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
        enabled = false;
    }
}