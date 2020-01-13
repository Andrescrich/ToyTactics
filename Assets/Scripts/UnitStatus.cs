using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class UnitStatus : MonoBehaviour
{
    public string UnitType;
    public int damage,maxHealth,specialMaxCD;
    [SerializeField] private int movementRange;
    public int currentHealth,specialCurrentCD;
    private bool passive = false;
    public Slider healthbar, cdbar;
    public GameObject quickUIInfo;

    public int MovementRange => movementRange;
    private Animator anim;
    public Canvas UI;
    private static readonly int Death = Animator.StringToHash("Death");
    private static readonly int Damage = Animator.StringToHash("Damage");

    private void Awake()
    {
        anim = transform.GetChild(0).GetComponent<Animator>();
    }

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
        healthbar.value = (float)currentHealth/maxHealth;
        updateHealthText();
        if(currentHealth == 0)
            DeathAnim();
        else 
            DamageAnim();
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

    private void OnMouseEnter()
    {
        quickUIInfo.SetActive(true);
    }
    
    private void OnMouseExit()
    {
        quickUIInfo.SetActive(false);
    }

    private void DeathAnim()
    {
        anim.SetTrigger(Death);
    }

    public bool IsPassive()
    {
        return passive;
    }

    public bool SpecialReady()
    {
        return specialMaxCD == specialCurrentCD;
    }

    private void DamageAnim()
    {
        anim.SetTrigger(Damage);
    }
}
