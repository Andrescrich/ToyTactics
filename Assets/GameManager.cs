using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject whoIsPlaying;
    public List<GameObject> players;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        
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
                    SelectPlayer(mouseHit.transform.GetComponent<PlayerController>());
                    enabled = false;
                }
            }
        }
    }

    public void SelectPlayer(PlayerController player)
    {
        player.enabled = true;
        enabled = false;
        player.nTurns = 1;
    }
}
