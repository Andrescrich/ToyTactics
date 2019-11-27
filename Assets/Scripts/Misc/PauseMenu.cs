using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene("SampleScene");
    }

    public void ExitGame()
    {
        //Debug.Log("Saliendo");
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
