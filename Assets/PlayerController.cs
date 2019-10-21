using System;
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
    private List<Transform> nextCubes = new List<Transform>();
    private List<Transform> nextNextCubes = new List<Transform>();
    private bool possiblePath;
    private bool enableInput = true;
    private bool moving = false;
    public int nTurns;
    
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
        if (nTurns == 0)
        {
            GameManager.instance.enabled = true;
            enabled = false;
        }
        
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

    private void MoveToClicked()
    {
        if (clickedCube == null) return;
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
        while (transform.position != firstCube.GetComponent<Walkable>().nodePos)
        {
            transform.position = Vector3.MoveTowards(transform.position, firstCube.GetComponent<Walkable>().nodePos,
                5f * Time.fixedDeltaTime);
            yield return new WaitForEndOfFrame();
        }
        while (transform.position != secondCube.GetComponent<Walkable>().nodePos)
        {
            transform.position = Vector3.MoveTowards(transform.position, secondCube.GetComponent<Walkable>().nodePos,
                5f * Time.fixedDeltaTime);
            yield return new WaitForEndOfFrame();
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
        moving = false;
        nTurns--;
    }

    private void OnEnable()
    {
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
