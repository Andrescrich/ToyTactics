using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public enum State {None,Menu,MenuAfterMove,SelectMove,SelectAttack,SelectSpecial};

    private Animator anim;
    [HideInInspector] public State state;
    public Transform currentCube;
    private Transform clickedCube;
    private List<Transform> finalPath; 
    private List<Transform> nextCubes = new List<Transform>();
    private List<Transform> nextNextCubes = new List<Transform>();
    private List<Transform> attackCubes = new List<Transform>();
    public Transform clickedEnemy;

    private bool possiblePath;
    [HideInInspector] public bool hasMoved;
    private bool enableInput = true;
    private bool moving;
    [HideInInspector] public bool canBePlayed;
    private bool attacking;
    
    public Material normal;
    public Material highlighted;
    public Material enemyHighlighter;
    public Material selectedMaterial;
    private  GameObject selectedTriangle;
    public Canvas UI;

    private void Awake()
    {
        anim = transform.GetChild(0).GetComponent<Animator>();
        selectedTriangle = transform.GetChild(1).gameObject;
        state = State.None;
        RayCastDown();
    }
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && enableInput)
        {
            var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(mouseRay, out var mouseHit))
            {
                if (mouseHit.transform.GetComponent<Walkable>() != null && state == State.SelectMove)
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

            if (state == State.SelectAttack && mouseHit.transform.GetComponent<EnemyController>() != null &&
                attackCubes.Contains(mouseHit.transform.GetComponent<EnemyController>().currentCube))
            {
                if (attacking) return;
                attacking = true;
                clickedEnemy = mouseHit.transform;
                var lookPos = new Vector3(clickedEnemy.position.x, transform.position.y, clickedEnemy.position.z);
                transform.LookAt(lookPos);
                anim.SetTrigger("Attack");
            }

            if (state == State.SelectAttack && mouseHit.transform.GetComponent<Walkable>() != null &&
                mouseHit.transform.GetComponent<Walkable>().pieceOnNode.Length > 0 &&
                attackCubes.Contains(mouseHit.transform))
            {
                if (attacking) return;
                attacking = true;
                clickedEnemy = mouseHit.transform.GetComponent<Walkable>().pieceOnNode.First().transform;
                var lookPos = new Vector3(clickedEnemy.position.x, transform.position.y, clickedEnemy.position.z);
                transform.LookAt(lookPos);
                anim.SetTrigger("Attack");
            }
        }
        anim.SetBool("Walk", moving);
    }
    
    private void RayCastDown()
    {
        var playerRay = new Ray(transform.position + transform.up * 0.5f, -transform.up);
        if (Physics.Raycast(playerRay, out var playerHit) && playerHit.transform.GetComponent<Walkable>() != null)
            currentCube = playerHit.transform;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.GetChild(0).position, -transform.up);
    }

    private void FindAttack()
    {
        foreach (var path in currentCube.GetComponent<Walkable>().possiblePaths)
        {
            if (path.active && path.target.GetComponent<Walkable>().pieceOnNode.Length > 0 && 
                    path.target.GetComponent<Walkable>().pieceOnNode.First().gameObject.GetComponent<EnemyController>()!=null)
                attackCubes.Add(path.target);
        }
    }
    
    public void MoveAction(int range)
    {
        StartCoroutine(MoveActionCor(range));
    }

    private IEnumerator MoveActionCor(int range)
    {
        state = State.SelectMove;
        var listaDeListasDeBlocks = new List<List<Transform>>();
        var listaTotalDeBlocks = new List<Transform>();
        foreach (var path in currentCube.GetComponent<Walkable>().possiblePaths)
        {
            if (path.active && path.target.GetComponent<Walkable>().pieceOnNode.Length == 0)
            {
                listaDeListasDeBlocks.Add(new List<Transform>());
                listaDeListasDeBlocks[0].Add(path.target);
            }
        }
        foreach (var p in listaDeListasDeBlocks[0])
        {
            listaTotalDeBlocks.Add(p);
        }
        var k = 0;
        for (var i = 1; i < range; i++)
        {
            listaDeListasDeBlocks.Add(new List<Transform>());
            foreach (var p in listaDeListasDeBlocks[k])
            {
                foreach (var path2 in p.GetComponent<Walkable>().possiblePaths)
                {
                    if (!listaDeListasDeBlocks[k].Contains(path2.target) && path2.active && currentCube != path2.target &&
                        !listaDeListasDeBlocks[i].Contains(path2.target) &&
                        path2.target.GetComponent<Walkable>().pieceOnNode.Length == 0 && !listaTotalDeBlocks.Contains(path2.target))
                    {
                        listaDeListasDeBlocks[i].Add(path2.target);
                        listaTotalDeBlocks.Add(path2.target);
                    }
                }
            } 
            k++;
        }

        foreach (var p in listaTotalDeBlocks)
        {
            p.gameObject.GetComponent<MeshRenderer>().sharedMaterial = highlighted;
            Debug.Log(p);
            yield return new WaitForSecondsRealtime(0.025f);
        }

        enableInput = true;
        while (state == State.SelectMove)
        {
            var Index = -1;
            Transform validClickedCube = null;
            if (clickedCube != null && enableInput)
            {
                foreach (var listaDeBlock in listaDeListasDeBlocks)
                {
                    
                    foreach (var block in listaDeBlock)
                    {
                        if (block == clickedCube)
                        {
                            enableInput = false;
                            validClickedCube = block;
                            
                        }
                    }

                    Index++;
                    if (!enableInput) break;
                }
            }

            if (validClickedCube != null)
            {
                moving = true;
                UI.GetComponent<ActionButton>().waitButton.interactable = false;
                UI.GetComponent<ActionButton>().attackButton.interactable = false;
                UI.GetComponent<ActionButton>().specialButton.interactable = false;
                
                var listaDePaths = new List<Transform>(new Transform[Index]);
                for (var i = 0; i < Index; i++)
                {
                    listaDePaths[i] = listaDeListasDeBlocks[i].First();
                    foreach (var block in listaDeListasDeBlocks[i])
                    {
                        if (Vector3.Distance(validClickedCube.position, listaDePaths[i].position) > Vector3.Distance(
                                validClickedCube.position,
                                block.position))
                        {
                            listaDePaths[i] = block;
                            
                        }
                    }
                }

                foreach (var path in listaDePaths)
                {
                    var lookPos = new Vector3(path.position.x, transform.position.y, path.position.z);
                    transform.LookAt(lookPos);
                    while (transform.position != path.GetComponent<Walkable>().nodePos)
                    {
                        transform.position = Vector3.MoveTowards(transform.position,
                            path.GetComponent<Walkable>().nodePos,
                            5f * Time.fixedDeltaTime);
                        yield return new WaitForFixedUpdate();
                    }
                }
                
                var lookPos2 = new Vector3(validClickedCube.position.x, transform.position.y, validClickedCube.position.z);
                transform.LookAt(lookPos2);
                while (transform.position != validClickedCube.GetComponent<Walkable>().nodePos)
                {
                    transform.position = Vector3.MoveTowards(transform.position,
                        validClickedCube.GetComponent<Walkable>().nodePos,
                        5f * Time.fixedDeltaTime);
                    yield return new WaitForFixedUpdate();
                }
                ReachedClicked();
            }
            
            yield return new WaitForFixedUpdate();
        }
        foreach (var p in listaTotalDeBlocks)
        {
            p.gameObject.GetComponent<MeshRenderer>().sharedMaterial = normal;
            
        }
        currentCube.GetComponent<Renderer>().sharedMaterial = selectedMaterial;
    }
    
    private void Clear()
    { 
        nextCubes.Clear();
        nextNextCubes.Clear();
        attackCubes.Clear();
    }
    private void OnMouseEnter()
    {
        if(GameManager.instance.gameState == GameStates.PlayerTurn && canBePlayed)
          selectedTriangle.SetActive(true);
    }
    
    private void OnMouseExit()
    {
        if (gameObject.GetComponent<PlayerController>().enabled != true)
            selectedTriangle.SetActive(false);
    }

    private void ReachedClicked()
    {
        if (transform.position != clickedCube.GetComponent<Walkable>().nodePos) return;
        UI.GetComponent<ActionButton>().waitButton.interactable = true;
        UI.GetComponent<ActionButton>().attackButton.interactable = true;
        UI.GetComponent<ActionButton>().specialButton.interactable = true;
        possiblePath = false;
        hasMoved = true;
        enableInput = true;
        currentCube.GetComponent<Renderer>().sharedMaterial = normal;
        RayCastDown();
        
        moving = false;
        state = State.MenuAfterMove;
    }

    private void EndAction(){
        possiblePath = false;
        enableInput = true;
        RayCastDown();
        GetComponent<Player>().RemoveFromPlayable();
        moving = false;
        canBePlayed = false;
        GameManager.instance.enabled = true;
        enabled = false;
        hasMoved = false;
        UI.GetComponent<ActionButton>().player = null;
        state = State.None;
    }

    public void WaitAction()
    { 
        currentCube.GetComponent<Renderer>().sharedMaterial = normal;
        EndAction();
    }

    public void AttackAction(){
        state = State.SelectAttack;
        FindAttack();
        foreach (var cube in attackCubes)
        {
            if(cube.GetComponent<Walkable>().pieceOnNode.Length > 0)
                cube.gameObject.GetComponent<MeshRenderer>().sharedMaterial = enemyHighlighter;
        }
    }
    
    private void OnEnable()
    {
        Clear();
        attacking = false;
        hasMoved = false;
        state = State.Menu;
        selectedTriangle.SetActive(true);
        GameManager.instance.whoIsPlaying = gameObject;
        UI.GetComponent<ActionButton>().player = gameObject;
        UI.GetComponent<ActionButton>().activateAll();
        UI.GetComponent<UIManager>().ShowInfoWindow(gameObject.GetComponent<UnitStatus>());
    }

    private void OnDisable()
    {
        selectedTriangle.SetActive(false);
        UI.GetComponent<UIManager>().stopShowInfoWindow();
        GameManager.instance.whoIsPlaying = null;
        UI.GetComponent<ActionButton>().moveButton.interactable = true;
        UI.GetComponent<ActionButton>().attackButton.interactable = true;
    }

    //Llamada en el frame de impacto de la animación de ataque
    public void Attack(Transform objective)
    {
        objective.GetComponent<UnitStatus>().ChangeHealth(-1*gameObject.GetComponent<UnitStatus>().damage);
        currentCube.GetComponent<Renderer>().sharedMaterial = normal;
        EndAction();
    }
}
