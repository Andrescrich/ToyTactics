using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour
{

    public GameObject player;
    // Start is called before the first frame update
    public void moveButton(){
        PlayerController pc = player.GetComponent<PlayerController>();
        pc.moveAction();
    }
}
