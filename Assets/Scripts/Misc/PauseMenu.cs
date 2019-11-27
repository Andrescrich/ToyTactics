using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("desplegado", false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void DisplayPauseMenu()
    {
        //Debug.Log("Desplegando");
        anim.SetBool("desplegado", true);
    }

    public void HidePauseMenu()
    {
        //Debug.Log("Ocultando");
        anim.SetBool("desplegado", false);
    }

    public void RestartLevel()
    {
        //Debug.Log("Reiniciando");
    }

    public void ExitGame()
    {
        //Debug.Log("Saliendo");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
