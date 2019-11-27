using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour
{ 
    public GameObject player;

    public Button moveButton;
    public Button attackButton;

    public Button specialButton;

    public Button waitButton;


    private void Start() {
        deactivateAll();
    }
    private void Update()
    {
        if (player == null){
            deactivateAll();
            return;
        } else {
            activateButtons(!player.GetComponent<PlayerController>().hasMoved,true,(!player.GetComponent<UnitStatus>().IsPassive() && player.GetComponent<UnitStatus>().SpecialReady()),true);
        }
    }

    public void MoveButton()
    {
        if (player == null) return;
        attackButton.interactable = true;
        player.GetComponent<PlayerController>().MoveAction();
        moveButton.interactable = false;

    }

    public void AttackButton()
    {
        if (player == null) return;
        moveButton.interactable = true;
        player.GetComponent<PlayerController>().AttackAction();
        attackButton.interactable = false;
    }

    public void SpecialButton(){
        PlayerController pc = player.GetComponent<PlayerController>();
        //pc.specialAction();
    }

    public void WaitButton()
    {
        if (player == null) return;
        player.GetComponent<PlayerController>().WaitAction();
        player = null;
    }

    public void deactivateAll(){
        moveButton.interactable = false;
        attackButton.interactable = false;
        specialButton.interactable = false;
        waitButton.interactable = false;
    }

    public void activateAll(){
        moveButton.interactable = true;
        attackButton.interactable = true;
        specialButton.interactable = true;
        waitButton.interactable = true;
    }

    public void activateButtons(bool move, bool attack, bool special, bool wait){
        moveButton.interactable = move;
        attackButton.interactable = attack;
        specialButton.interactable = special;
        waitButton.interactable = wait;
    }

    
}
