using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickInfoUI : MonoBehaviour
{
    
    public Text NombreUnidadInfo;
    public Text ataqueUnidadInfo;
    public Text rangoMovUnidadInfo;
    public Text SpecialInfo;

    private void Update()
    {
        ShowInfoWindow(transform.root.gameObject.GetComponent<UnitStatus>());
    }

    public void ShowInfoWindow(UnitStatus status)
    {
        NombreUnidadInfo.text = transform.root.name + "   " + status.currentHealth + "/" + status.maxHealth + " PS";
        ataqueUnidadInfo.text = "Damage: "+status.damage;
        rangoMovUnidadInfo.text = "Movment Range: " + status.movementRange;
        SpecialInfo.text = "Special: " + status.specialCurrentCD + "/" + status.specialMaxCD + " CD";

    }
}
