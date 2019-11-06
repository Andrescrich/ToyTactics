using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour
{

    public GameObject player;

    public Button moveButton;


    public void MoveButton()
    {
        if (player == null) return;
        if (player.GetComponent<PlayerController>().hasMoved) return;
        player.GetComponent<PlayerController>().MoveAction();
        moveButton.interactable = false;

    }

    public void AttackButton(){
        if (player == null) return;
        player.GetComponent<PlayerController>().AttackAction();
        player = null;
    }

    public void specialButton(){
        PlayerController pc = player.GetComponent<PlayerController>();
        //pc.specialAction();
    }

    public void WaitButton()
    {
        if (player == null) return;
        player.GetComponent<PlayerController>().WaitAction();
        player = null;
    }
}
