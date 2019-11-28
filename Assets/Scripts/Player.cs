    using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    using UnityEngine.SceneManagement;

    public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    public delegate void EndOfTurn();

    public static event EndOfTurn EndOfTurnEvent;
    private void Awake()
    {
        GameManager.turnChanging += AddToPlayable;
    }

    private void AddToPlayable()
    {
        if (gameObject.CompareTag("Player") && GameManager.instance.gameState == GameStates.PlayerTurn)
        {
            GameManager.instance.players.Add(gameObject);
            gameObject.GetComponent<PlayerController>().canBePlayed = true;
            gameObject.GetComponent<UnitStatus>().startTurn();
        } else if (gameObject.CompareTag("Enemy") && GameManager.instance.gameState == GameStates.EnemyTurn)
        {
            GameManager.instance.players.Add(gameObject);
            gameObject.GetComponent<UnitStatus>().startTurn();
        }

        if (gameObject.CompareTag("Player") && !GameManager.instance.aliados.Contains(gameObject)) {
            GameManager.instance.aliados.Add(gameObject);
        }
        else if (gameObject.CompareTag("Enemy") && !GameManager.instance.enemigos.Contains(gameObject)) {
            GameManager.instance.enemigos.Add(gameObject); }
    }

    public void RemoveFromPlayable()
    {
        EndOfTurnEvent?.Invoke();
        GameManager.instance.players.Remove(gameObject);
    }

    private void OnDestroy()
    {
        RemoveFromPlayable();
        GameManager.turnChanging -= AddToPlayable;
        if (GameManager.instance.aliados.Contains(gameObject)) { GameManager.instance.aliados.Remove(gameObject); }
        else if (GameManager.instance.enemigos.Contains(gameObject)) { GameManager.instance.enemigos.Remove(gameObject); }
    }

    private void OnDisable()
    {
        GameManager.turnChanging -= AddToPlayable;
    }
}
