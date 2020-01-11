using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject whoIsPlaying;
    public List<GameObject> players;
    public List<GameObject> aliados;
    public List<GameObject> enemigos;
    public GameStates gameState = GameStates.PlayerTurn;
    public Material playableMaterial;
    public Material selectedMaterial;

    public delegate void TurnChange(); 
    public static event TurnChange turnChanging;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        turnChanging();
        HighlightPlayables();
    }
    

    private void Update() 
    {

        if (aliados.Count == 0)
        {
            StartCoroutine(ReloadScene());
        } else if (enemigos.Count == 0)
        {
            StartCoroutine(LoadNextScene());
        }
        
        
        if (gameState == GameStates.PlayerTurn)
        {
            if (players.Count == 0)
            {
                gameState = GameStates.EnemyTurn;
                turnChanging?.Invoke();
                HighlightPlayables();
                players[0].GetComponent<EnemyController>().enabled = true;
            }
            if (Input.GetMouseButtonDown(0))
            {
                var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(mouseRay, out var mouseHit))
                {
                    if (mouseHit.transform.CompareTag("Player")
                        && mouseHit.transform.GetComponent<PlayerController>().canBePlayed)
                    {
                        SelectPlayer(mouseHit.transform.GetComponent<PlayerController>());
                        enabled = false;
                    }
                }
            }
        } else if (gameState == GameStates.EnemyTurn)
        {
            if (players.Count == 0)
            {
                gameState = GameStates.PlayerTurn;
                turnChanging?.Invoke();
                HighlightPlayables();
            }
        }
    }

    public void SelectPlayer(PlayerController player)
    {
        player.enabled = true;
        enabled = false; 
        player.currentCube.GetComponent<MeshRenderer>().sharedMaterial = selectedMaterial;
    }

    public void NextEnemyTurn()
    {
        if(players.Count > 0)
          players[0].GetComponent<EnemyController>().enabled = true;
    }

    private void HighlightPlayables()
    {
        foreach (var player in players)
        {
            //Debug.Log(player);
            if(player.GetComponent<PlayerController>() != null)
                player.GetComponent<PlayerController>().currentCube.GetComponent<MeshRenderer>().sharedMaterial = playableMaterial;
            if(player.GetComponent<EnemyController>() != null)
                player.GetComponent<EnemyController>().currentCube.GetComponent<MeshRenderer>().sharedMaterial = playableMaterial;
        }
    }

    private IEnumerator LoadNextScene()
    {
        yield return new WaitForSecondsRealtime(3);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private IEnumerator ReloadScene()
    {
        yield return new WaitForSecondsRealtime(3);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

    public enum GameStates
    {
        PlayerTurn, EnemyTurn
    }


