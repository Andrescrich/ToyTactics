using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject whoIsPlaying;
    public List<GameObject> players;
    public GameStates gameState = GameStates.PlayerTurn;
    public Material playableMaterial;
    public Material normalMaterial;

    public delegate void TurnChange(); 
    public static event TurnChange turnChanging;


    private void Awake()
    {
        instance = this;
     //   turnChanging += HighlightPlayables;
    }

    private void Start()
    {
        turnChanging();
        HighlightPlayables();
    }
    

    private void Update() 
    {
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
                    if (mouseHit.transform.GetChild(0).CompareTag("Player")
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
            player.currentCube.GetComponent<MeshRenderer>().sharedMaterial = normalMaterial;
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
            Debug.Log(player);
            if(player.GetComponent<PlayerController>() != null)
                player.GetComponent<PlayerController>().currentCube.GetComponent<MeshRenderer>().sharedMaterial = playableMaterial;
            if(player.GetComponent<EnemyController>() != null)
                player.GetComponent<EnemyController>().currentCube.GetComponent<MeshRenderer>().sharedMaterial = playableMaterial;
        }
    }
}

    public enum GameStates
    {
        PlayerTurn, EnemyTurn
    }
