using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static GameManager;

public class EleccionJuguete : MonoBehaviour
{
    // Start is called before the first frame update
    public static EleccionJuguete eleccionjuguete;
    public bool chooseDuck;
    public bool first = true;
    public GameObject player;
    public GameObject duckButton;

    public delegate void EndOfTurn(); 
    private void Awake()
    {
        if (eleccionjuguete == null)
        {
            eleccionjuguete = this;
            DontDestroyOnLoad(gameObject);
        }
        else if(eleccionjuguete!=this) Destroy(gameObject);

        if (SceneManager.GetActiveScene().buildIndex == 3 && first)
        {
            if (chooseDuck && first)
            {
                player = GameObject.Find("BlueRacket");
                player.SetActive(false);
                player.GetComponent<PlayerController>().canBePlayed = false;
                player.GetComponent<PlayerController>().currentCube.GetComponent<MeshRenderer>().sharedMaterial = player.GetComponent<PlayerController>().normal;
                player.GetComponent<UnitStatus>().currentHealth = 0;
                player.GetComponent<Player>().RemoveFromPlayable();
                first = false;
            }
            else if (!chooseDuck && first)
            {
                player = GameObject.Find("BlueDuck");
                player.SetActive(false);
                player.GetComponent<PlayerController>().canBePlayed = false;
                player.GetComponent<PlayerController>().currentCube.GetComponent<MeshRenderer>().sharedMaterial = player.GetComponent<PlayerController>().normal;
                player.GetComponent<UnitStatus>().currentHealth = 0;
                player.GetComponent<Player>().RemoveFromPlayable();

            }
        }

    }
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        // Esto lo hago para que dependiendo si se ha elegido o no un juguete que lo quite del juego
        if(SceneManager.GetActiveScene().buildIndex == 3 && first)
        {
            if (chooseDuck && first) {
                player = GameObject.Find("BlueRacket");
                player.SetActive(false);
                player.GetComponent<PlayerController>().canBePlayed = false;
                player.GetComponent<PlayerController>().currentCube.GetComponent<MeshRenderer>().sharedMaterial = player.GetComponent<PlayerController>().normal;
                player.GetComponent<UnitStatus>().currentHealth = 0;
                player.GetComponent<Player>().RemoveFromPlayable();
                first = false; 
            }
                else if(!chooseDuck && first){
                player = GameObject.Find("BlueDuck"); 
                player.SetActive(false);
                player.GetComponent<PlayerController>().canBePlayed = false;
                player.GetComponent<PlayerController>().currentCube.GetComponent< MeshRenderer > ().sharedMaterial = player.GetComponent<PlayerController>().normal;
                player.GetComponent<UnitStatus>().currentHealth=0;
                player.GetComponent<Player>().RemoveFromPlayable();
               
            }
        }
        
    }
    public void DuckSelect()
    {
        print("entro");
        chooseDuck = true;
        SceneManager.LoadScene(3);
    }

    public void RacketSelect()
    {
        chooseDuck = false;
        SceneManager.LoadScene(3);
    }

    


}
