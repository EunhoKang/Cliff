using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour {

    public AudioClip audioUI;
    private AudioSource audio;

    public void Title()
    {
        Sound();
        SceneManager.LoadScene("SampleScene");
    }

    public void Quit()
    {
        Sound();
        Application.Quit();
    }

    public void Start()
    {
        Screen.SetResolution(Screen.width, Screen.width * 16 / 9, true);
        audio = gameObject.AddComponent<AudioSource>();
    }

    public void Sound()
    {
        audio.clip = audioUI;
        audio.Play();
    }
}
