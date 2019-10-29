using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        GameManager.instance.players.Add(gameObject);
    }

    private void Update()
    {
        if (GetComponent<PlayerController>().nTurns == 0)
        {
            GameManager.instance.players.Remove(gameObject);
        }
    }
}
