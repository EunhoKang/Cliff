using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {

    public GameObject[] canvases;
    private GameObject current= null;
    public GameObject[] characters;
    private GameObject Characterskin;
    public CameraControl camera_control;
    public GameManager1 game_manager;
    public RectTransform rt;
 
    void Start () {
		for(int i=0; i < canvases.Length; i++)
        {
            canvases[i].SetActive(false);
        }
        canvases[3].SetActive(true);
        current = canvases[2];
        Characterskin = characters[0];

	}

    public void MenuChange(int k)
    {
        current.SetActive(false);
        canvases[k].SetActive(true);
        current = canvases[k];
    }

    public void charactor_change(int k)
    {
        Characterskin = characters[k];
        Debug.Log("current : "+Characterskin);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            BacktoMenu();
            MenuChange(2);
        }
    }

    public void GetReadyGame()
    {
        camera_control.GameMode();
        
    }
    public void BacktoMenu()
    {
        camera_control.MenuMode();
    }

}
