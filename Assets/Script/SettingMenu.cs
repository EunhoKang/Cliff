using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingMenu : MonoBehaviour {

    public MenuManager menu_manager;
    public Texture[] ButtonImages;
    public RawImage Current_Image;
    public CameraControl cameracon;
    public Text Current_Text;

    public AudioClip audioUI;
    private AudioSource audio;
    public int menu_num = 3;

    private void Start()
    {
        Current_Text.text = 0.ToString();
        Current_Image.texture = ButtonImages[0];
        Screen.SetResolution(Screen.width, Screen.width * 16/9, true);
        audio = gameObject.AddComponent<AudioSource>();
    }

    public void Onclick_GameStrat()
    {
        Sound();
        Debug.Log("Clicked Game Start");
        menu_manager.GetReadyGame();
    }
    public void Onclick_Quit()
    {
        Sound();
        Debug.Log("Clicked Quit");
        Application.Quit();
    }

    public void Onclick_Howtoplay()
    {
        Sound();
        Debug.Log("Clicked tuto");
        cameracon.MenuMode();
    }

    public void Setup(int k)
    {
        menu_manager.charactor_change(k);
        Current_Text.text = k.ToString();
        Current_Image.texture = ButtonImages[k];

        Debug.Log("Character Changed");
    }

    public void Sound()
    {
        audio.clip = audioUI;
        audio.Play();
    }
}
