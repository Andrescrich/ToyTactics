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
    public Text NombreUnidadInfo;
    public Text ataqueUnidadInfo;
    public Text rangoMovUnidadInfo;
    public Text SpecialInfo;
    public GameObject animationPanel;
    public GameObject informationPanel;
    public GameObject moveButton;
    public GameObject attackButton;
    public GameObject speButton;
    public GameObject waitButton;

    public PauseMenu pauseMenu;
    bool paused;

    private void Start()
    {
        paused = false;
        player1Name.text = GameManager.instance.players[0].name;
        player2Name.text = GameManager.instance.players[1].name;
        animationPanel = GameObject.Find("ActionWindow");
        informationPanel = GameObject.Find("InfoWindow");
        moveButton = GameObject.Find("MoveButton");
        attackButton = GameObject.Find("AttackButton");
        speButton = GameObject.Find("SpecButton");
        waitButton = GameObject.Find("WaitButton");
        moveButton.gameObject.SetActive(false);
        attackButton.gameObject.SetActive(false);
        speButton.gameObject.SetActive(false);
        waitButton.gameObject.SetActive(false);
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

        if (GameManager.instance.enemigos.Count == 0) { TurnText.text = "LEVEL COMPLETED"; }
        else if (GameManager.instance.aliados.Count == 0) { TurnText.text = "LEVEL FAILED"; }

        if (Input.GetKeyDown(KeyCode.U)) {
            if (paused) { pauseMenu.DisplayPauseMenu(); paused = false; }
            else { pauseMenu.HidePauseMenu(); paused = true; }
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

    public void ShowInfoWindow(UnitStatus status)
    {
        moveButton.gameObject.SetActive(true);
        attackButton.gameObject.SetActive(true);
        speButton.gameObject.SetActive(true);
        waitButton.gameObject.SetActive(true);
        NombreUnidadInfo.text = status.gameObject.name + "   " + status.currentHealth + "/" + status.maxHealth + " PS";
        ataqueUnidadInfo.text = "Damage: "+status.damage;
        rangoMovUnidadInfo.text = "Movment Range: " + status.MovementRange;
        SpecialInfo.text = "Special: " + status.specialCurrentCD + "/" + status.specialMaxCD + " CD";

    }
    public void stopShowInfoWindow()
    {
        
        NombreUnidadInfo.text = "";
        ataqueUnidadInfo.text = "";
        rangoMovUnidadInfo.text = "";
        SpecialInfo.text = "";
        moveButton.gameObject.SetActive(false);
        attackButton.gameObject.SetActive(false);
        speButton.gameObject.SetActive(false);
        waitButton.gameObject.SetActive(false);

    }
}
