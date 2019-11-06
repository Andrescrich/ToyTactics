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

    private void Update()
    {
        if (player == null) return;
        if (player.GetComponent<PlayerController>().hasMoved)
            moveButton.interactable = false;
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
