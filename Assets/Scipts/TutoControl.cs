using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutoControl : MonoBehaviour {

	public GameObject[] image;
	private int check;

	public Text text;

	// Use this for initialization
	void Start () {
		check = 0;
		
	}
	
	// Update is called once per frame

	public void Changeimage() {
		check = check + 1;
		if(check == 9){
			SceneManager.LoadScene("SampleScene");
			

		}
		else{
			image[check].gameObject.SetActive(true);
		}

	}
}
