﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class UnitStatus : MonoBehaviour
{
    public string UnitType;
    public int damage,movementRange,maxHealth,specialMaxCD;
    public int currentHealth,specialCurrentCD;
    private bool passive = false;
    public Slider healthbar, cdbar;
    // Start is called before the first frame update
    void Start()
    {

        healthbar.value = (float)currentHealth/maxHealth;
        updateHealthText();

        if(specialMaxCD == 0){
            passive = true;
        }

        if(passive){
            cdbar.gameObject.SetActive(false);
        } else {
            updateCD((float)specialCurrentCD/specialMaxCD);
        }
    }


    public void ChangeHealth(int amount)
    {
        currentHealth = currentHealth + amount;
        currentHealth = Math.Min(currentHealth, maxHealth);
        currentHealth = Math.Max(currentHealth, 0);
        //change healthbar (Animation?)
        healthbar.value = (float)currentHealth/maxHealth;
        updateHealthText();
        if(currentHealth == 0){
            //deathThing
        }
    }

    public void startTurn()
    {
        if(!passive){
            specialCurrentCD = Math.Min(specialCurrentCD+1,specialMaxCD);
            updateCD((float)specialCurrentCD/specialMaxCD);
        }
    }

    public void usedSpecial()
    {
        updateCD(0);
    }

    private void updateCD(float valueNew)
    {
        if(!passive)
        {
            cdbar.value = valueNew;
            updateCDText();
        }
    }

    private void updateCDText()
    {
        Text cdText = cdbar.gameObject.GetComponentInChildren<Text>();
        if(specialMaxCD == specialCurrentCD)
            cdText.text = "READY";
        else
            cdText.text = "" + specialCurrentCD;
    }

    private void updateHealthText()
    {
        Text hlthText = healthbar.gameObject.GetComponentInChildren<Text>();
        hlthText.text = "" + currentHealth;
    }
}