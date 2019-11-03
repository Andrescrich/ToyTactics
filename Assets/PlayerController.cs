﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{

    public Transform currentCube;
    private Transform clickedCube;
    private List<Transform> finalPath; 
    public List<Transform> nextCubes = new List<Transform>();
    public List<Transform> nextNextCubes = new List<Transform>();
    private bool possiblePath;
    private bool enableInput = true;
    private bool moving = false;
    public bool canBePlayed;
    
    public Material highlighted;
    public Material normal;
    public Material highlighted2;
    public GameObject selectedTriangle;
    
    
    private void Awake()
    {
        selectedTriangle = transform.GetChild(1).gameObject;
    }

    private void Start()
    {
        RayCastDown();
        FindPath();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && enableInput)
        {
            var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(mouseRay, out var mouseHit))
            {
                if(mouseHit.transform.GetComponent<Walkable>() != null)
                {
                    clickedCube = mouseHit.transform;
                    foreach (var cube in nextCubes) 
                    { 
                        if (clickedCube == cube) 
                            possiblePath = true;
                    }

                    foreach (var cube in nextNextCubes)
                    {
                        if (clickedCube == cube) 
                            possiblePath = true;
                    }
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (possiblePath)
        {
            enableInput = false;
            foreach (var cube in nextCubes)
            {
                cube.gameObject.GetComponent<MeshRenderer>().sharedMaterial = normal;
            }

            foreach (var cube in nextNextCubes)
            {
                cube.gameObject.GetComponent<MeshRenderer>().sharedMaterial = normal;
            }
            if(!moving)
               MoveToClicked();
        }
        else
        {
            foreach (var cube in nextCubes)
            {
                cube.gameObject.GetComponent<MeshRenderer>().sharedMaterial = highlighted;
            }
            foreach (var cube in nextNextCubes)
            {
                cube.gameObject.GetComponent<MeshRenderer>().sharedMaterial = highlighted2;
            }
        }
    }


    private void RayCastDown()
    {
        var playerRay = new Ray(transform.GetChild(0).position, -transform.up);
        if (Physics.Raycast(playerRay, out var playerHit))
        {
            Debug.Log(playerHit.transform.name);
            if (playerHit.transform.GetComponent<Walkable>() != null)
            {
                currentCube = playerHit.transform;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.GetChild(0).position, -transform.up);
    }

    private void FindPath()
    {
        foreach (var path in currentCube.GetComponent<Walkable>().possiblePaths)
        {
            if (path.active && path.target.GetComponent<Walkable>().pieceOnNode.Length == 0)
                nextCubes.Add(path.target);
           
        }
        foreach (var p in nextCubes)
        {
            foreach (var path2 in p.GetComponent<Walkable>().possiblePaths)
            {
                if (!nextCubes.Contains(path2.target) && path2.active && currentCube != path2.target
                    && path2.target.GetComponent<Walkable>().pieceOnNode.Length == 0)
                    nextNextCubes.Add(path2.target);
                else 
                    nextNextCubes.Remove(path2.target);
            }
        }
    }

    private void MoveToClicked()
    {
        if (clickedCube == null) return;
        var lookPos = new Vector3(clickedCube.position.x, transform.position.y, clickedCube.position.z);
        transform.LookAt(lookPos);
        if (nextCubes.Contains(clickedCube))
            transform.position = Vector3.MoveTowards(transform.position, clickedCube.GetComponent<Walkable>().nodePos,
                5f * Time.fixedDeltaTime);
        
        if (nextNextCubes.Contains(clickedCube))
        {
            var auxCube = nextCubes.First();
            foreach (var cube in nextCubes)
            {
                if (Vector3.Distance(clickedCube.position, auxCube.position) > Vector3.Distance(
                        clickedCube.position, cube.position))
                {
                    auxCube = cube;
                }
            }

            moving = true;
            StartCoroutine(Move2Blocks(auxCube, clickedCube));
        }
        ReachedClicked();
    }
    
    private IEnumerator Move2Blocks(Transform firstCube, Transform secondCube)
    {
        var lookPos = new Vector3(firstCube.position.x, transform.position.y, firstCube.position.z);
        transform.LookAt(lookPos);
        
        while (transform.position != firstCube.GetComponent<Walkable>().nodePos)
        {
            transform.position = Vector3.MoveTowards(transform.position, firstCube.GetComponent<Walkable>().nodePos,
                5f * Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
        }
        
        var lookPos2 = new Vector3(secondCube.position.x, transform.position.y, secondCube.position.z);
        transform.LookAt(lookPos2);
        while (transform.position != secondCube.GetComponent<Walkable>().nodePos)
        {
            transform.position = Vector3.MoveTowards(transform.position, secondCube.GetComponent<Walkable>().nodePos,
                5f * Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
        }
        ReachedClicked();
    }
    
    private void Clear()
    { 
        nextCubes.Clear();
        nextNextCubes.Clear();
    }
    private void OnMouseEnter()
    {
        if(GameManager.instance.gameState == GameStates.PlayerTurn && canBePlayed)
          selectedTriangle.SetActive(true);
    }
    
    private void OnMouseExit()
    {
        if(gameObject.GetComponent<PlayerController>().enabled != true)
         selectedTriangle.SetActive(false);
    }

    private void ReachedClicked()
    {
        if (transform.position != clickedCube.GetComponent<Walkable>().nodePos) return;
        possiblePath = false;
        enableInput = true;
        RayCastDown();
        Clear();
        FindPath();
        GetComponent<Player>().RemoveFromPlayable();
        moving = false;
        canBePlayed = false;
        GameManager.instance.enabled = true;
        enabled = false;
    }

    private void OnEnable()
    {
        Clear();
        FindPath();
        selectedTriangle.SetActive(true);
        GameManager.instance.whoIsPlaying = gameObject;
    }

    private void OnDisable()
    {
        selectedTriangle.SetActive(false);
        GameManager.instance.whoIsPlaying = null;
        foreach (var cube in nextCubes)
        {
            cube.gameObject.GetComponent<MeshRenderer>().sharedMaterial = normal;
        }

        foreach (var cube in nextNextCubes)
        {
            cube.gameObject.GetComponent<MeshRenderer>().sharedMaterial = normal;
        }
    }
}