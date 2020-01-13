using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuEleccion : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject player;
    EleccionJuguete elec;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DuckSelect()
    {
        player = GameObject.Find("EleccionJuguete");
        player.GetComponent<EleccionJuguete>().DuckSelect();
        
    }

    public void RacketSelect()
    {
        player = GameObject.Find("EleccionJuguete");
        player.GetComponent<EleccionJuguete>().RacketSelect();
    }

    public void AnquiloSelect()
    {
        player = GameObject.Find("EleccionJuguete");
        player.GetComponent<EleccionJuguete>().AniquiloSelect();
    }

    public void ClownSelect()
    {
        player = GameObject.Find("EleccionJuguete");
        player.GetComponent<EleccionJuguete>().ClownSelect();
    }

}
