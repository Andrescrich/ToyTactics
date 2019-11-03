using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class UnitStatus : MonoBehaviour
{
    public float maxHealth, currentHealth;
    public int specialMaxCD, specialCurrentCD;
    // Start is called before the first frame update
    void Start()
    {
        Slider Healthbar = gameObject.GetComponentInChildren(typeof(Slider)) as Slider;
        Healthbar.value = currentHealth/maxHealth;

        Text CoolDown = gameObject.GetComponentInChildren(typeof(Text)) as Text;
        if(specialMaxCD == 0){
            CoolDown.text = "P";
        } else {
            CoolDown.text = ""+specialCurrentCD;
        }
    }


    void ChangeHealth(float amount)
    {
        currentHealth -= amount;
        //change healthbar (Animation?)
        Slider Healthbar = gameObject.GetComponentInChildren(typeof(Slider)) as Slider;
        Healthbar.value = currentHealth/maxHealth;
        if(currentHealth <= 0){
            //call death
        }
    }

    void startTurn()
    {
        specialCurrentCD-= Math.Max(specialCurrentCD-1,0);
        Text CoolDown = gameObject.GetComponentInChildren(typeof(Text)) as Text;
        CoolDown.text = ""+specialCurrentCD;
    }

    void usedSpecial()
    {
        Text CoolDown = gameObject.GetComponentInChildren(typeof(Text)) as Text;
        CoolDown.text = ""+specialMaxCD;
    }
}
