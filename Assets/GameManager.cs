using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(mouseRay, out var mouseHit))
            {
                if (mouseHit.transform.GetChild(0).tag == "Player")
                {
                    mouseHit.transform.GetComponent<PlayerController>().enabled = true;
                    enabled = false;
                }
            }
        }
    }
}
