using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CameraControl : MonoBehaviour {

    public Vector3 MainScene_position ;
    public Quaternion MainScene_rotation ;
    public Vector3 GameScene_position ;
    public Quaternion GameScene_rotation ;
    public GameObject canvas;
    public Image Background;
    

    public void GameMode()
    {
        StartCoroutine("GameModeMovement");
    }
    public void MenuMode()
    {
        StartCoroutine("MainModeMovement");
        
    }

    IEnumerator GameModeMovement()
    {
        Background.gameObject.SetActive(true);
        Color temp = Background.color;
        for (int i = 0; i < 10; i++)
        {
            temp.a = Background.color.a + 0.1f;
            Background.color = temp;
            yield return new WaitForSeconds(0.02f);
        }
        SceneManager.LoadScene("InGame");

    }

    IEnumerator MainModeMovement()
    {
        Background.gameObject.SetActive(true);
        Color temp = Background.color;
        for (int i = 0; i < 10; i++)
        {
            temp.a = Background.color.a + 0.1f;
            Background.color = temp;
            yield return new WaitForSeconds(0.02f);
        }
        SceneManager.LoadScene("Tuto");
    }
}
