using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour
{

    public GameObject player;
    // Start is called before the first frame update

    private void Update() {
        if(player != null && player.GetComponent<PlayerController>().hasMoved)
        {
            GameObject myButton = GameObject.Find("MoveButton");
            myButton.GetComponent<Button>().interactable = false;
        } else {
            GameObject myButton = GameObject.Find("MoveButton");
            myButton.GetComponent<Button>().interactable = true;
        }
    }
    public void moveButton(){
        
        PlayerController pc = player.GetComponent<PlayerController>();
        pc.moveAction();
    }

    public void attackButton(){
        PlayerController pc = player.GetComponent<PlayerController>();
        pc.attackAction();
    }

    public void specialButton(){
        PlayerController pc = player.GetComponent<PlayerController>();
        //pc.specialAction();
    }

    public void waitButton(){
        PlayerController pc = player.GetComponent<PlayerController>();
        pc.waitAction();
    }
}
