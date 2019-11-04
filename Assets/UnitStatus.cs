using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class UnitStatus : MonoBehaviour
{
    public float maxHealth, currentHealth;
    public int specialMaxCD, specialCurrentCD;
    private bool passive = false;

    public Slider healthbar, cdbar;
    // Start is called before the first frame update
    void Start()
    {

        healthbar.value = currentHealth/maxHealth;

        if(specialMaxCD == 0){
            passive = true;
        }

        if(passive){
            cdbar.gameObject.SetActive(false);
        } else {
            updateCD((float)specialCurrentCD/specialMaxCD);
        }
    }


    public void ChangeHealth(float amount)
    {
        currentHealth = currentHealth + amount;
        currentHealth = Math.Min(currentHealth, maxHealth);
        currentHealth = Math.Max(currentHealth, 0);
        //change healthbar (Animation?)
        healthbar.value = currentHealth/maxHealth;
        if(currentHealth == 0){
            //deathThing
        }
    }

    public void startTurn()
    {
        if(!passive){
            specialCurrentCD = Math.Min(specialCurrentCD+1,specialMaxCD);
            updateCD((float)specialCurrentCD/specialMaxCD);
            if(specialCurrentCD == specialMaxCD){
                //Cambiar color del slider
            }
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
        }
    }
}
