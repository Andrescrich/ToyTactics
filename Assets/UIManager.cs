using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public PlayerController player1;

    public PlayerController player2;
    // private Image healthBar;
    public Text player1Name;
    public Text player2Name;
    private bool playerSelected;
    public Image panelVidas;
    public Text selectedPlayerNameText;
    public Image selectedPlayerPanel;
    public Text TurnText;


    private void Start()
    {
        player1Name.text = GameManager.instance.players[0].name;
        player2Name.text = GameManager.instance.players[1].name;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.whoIsPlaying != null)
        {
            selectedPlayerPanel.gameObject.SetActive(true);
            panelVidas.gameObject.SetActive(false);
            selectedPlayerNameText.text = GameManager.instance.whoIsPlaying.name;
        }
        else
        {
            selectedPlayerPanel.gameObject.SetActive(false);
            panelVidas.gameObject.SetActive(true);
        }


        if (GameManager.instance.gameState == GameStates.PlayerTurn)
        {
            TurnText.text = "PlayerTurn";
        } else if (GameManager.instance.gameState == GameStates.EnemyTurn)
        {
            TurnText.text = "EnemyTurn";
        }
    }

    public void SelectPlayer1()
    {
        GameManager.instance.SelectPlayer(GameManager.instance.players[0].GetComponent<PlayerController>());
    }
    
    public void SelectPlayer2()
    {
        GameManager.instance.SelectPlayer(GameManager.instance.players[1].GetComponent<PlayerController>());
    }
}
