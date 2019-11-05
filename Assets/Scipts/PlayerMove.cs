using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {
	public GameObject Player;
	public GameObject Move;
	public GameObject Up;
	public GameObject Down;
	public GameObject Right;
	public GameObject Left;

    public AudioClip audioMove;
    private AudioSource audio;

    public GameObject p1;
    public GameObject p2;

    bool ispause = false;

    int mode = 0;

    int playerNum = 0;

	int touch = 1;

    private void Start()
    {
        audio =gameObject.AddComponent<AudioSource>();
    }

    public void ChangePlayer()
    {
        if(GameManager._instance.ThisTurn() == 0)
        {
            Player = GameObject.Find("P1");
            Move = Player.transform.Find("Move").gameObject;
            Up = Move.transform.Find("UP").gameObject;
            Down = Move.transform.Find("DOWN").gameObject;
            Right = Move.transform.Find("RIGHT").gameObject;
            Left = Move.transform.Find("LEFT").gameObject;
            playerNum = 0;
        }
        if (GameManager._instance.ThisTurn() == 1)
        {
            Player = GameObject.Find("P2");
            Move = Player.transform.Find("Move").gameObject;
            Up = Move.transform.Find("UP").gameObject;
            Down = Move.transform.Find("DOWN").gameObject;
            Right = Move.transform.Find("RIGHT").gameObject;
            Left = Move.transform.Find("LEFT").gameObject;
            playerNum = 1;
        }
    }

    IEnumerator WinMotion(int k)
    {
        for (int i = 0; i < 10; i++)
        {
            if(k==0)
                StartCoroutine(MoveChar("up"));
            else if(k==1)
                StartCoroutine(MoveChar("down"));
            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator Turn_to_see()
    {

        yield return new WaitForSeconds(0.01f);
    }

    IEnumerator MoveChar(string s)
    {
        
        Vector3 ame = Player.transform.position;
        Vector3 Original = ame;
        yield return new WaitForSeconds(0.1f);
        for (int i=0; i < 20; i++)
        {
            if (s == "up")
                ame.z = ame.z + 0.06f;
            else if (s == "right")
                ame.x = ame.x + 0.06f;
            else if (s == "left")
                ame.x = ame.x - 0.06f;
            else if (s == "down")
                ame.z = ame.z - 0.06f;
            ame.y = ame.y +(-0.1f*(i*0.06f)+0.06f);
            Player.transform.position = ame;
            yield return new WaitForSeconds(0.002f);
        }
        if (s == "up")
        {
            Player.transform.position = new Vector3(Original.x, Original.y, Original.z + 1.2f);
            GameManager._instance.CheckGameFinished();
        }
        else if (s == "right")
        {
            Player.transform.position = new Vector3(Original.x + 1.2f, Original.y, Original.z);
        }
        else if (s == "left")
        {
            Player.transform.position = new Vector3(Original.x - 1.2f, Original.y, Original.z);
        }
        else if (s == "down")
        {
            Player.transform.position = new Vector3(Original.x, Original.y, Original.z - 1.2f);
            GameManager._instance.CheckGameFinished();
        }
        GameManager._instance.NextTurn();
    }


    // Update is called once per frame
    void Update () {

        if (Input.GetKey(KeyCode.D))
        {
            for(int i=0; i<GameManager.Instance().checking.Length; i++)
            {
                Debug.Log(GameManager.Instance().checking[i]);
            }
        }

		if(Input.GetMouseButtonDown(0)){
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if(Physics.Raycast(ray,out hit)){
				
				Debug.Log(hit.transform.name);
                if((hit.transform.name == "P1" && GameManager._instance.ThisTurn() == 0) || (hit.transform.name == "P2" && GameManager._instance.ThisTurn() == 1))
                {
					touch = touch +1;
                    //게임 매니저의 turn_num 변수는 단순히 누구의 턴인지만 알려주며,
                    //이를 이용해 어떤 기능을 막을지는 알아서 코딩해주세요
				}

                if (touch % 2 == 0 ){
					Move.gameObject.SetActive(true);
					
					
				}
				if(touch % 2 == 1){
					Move.gameObject.SetActive(false);
				}
                
                if(hit.transform.name == "RIGHT" )
                {
                    if ((GameManager._instance.currentpos + 1 >= 0 && GameManager._instance.currentpos + 1 < 70) && GameManager._instance.checking[GameManager._instance.currentpos + 1] == 1 && GameManager._instance.currentpos + 1!= GameManager._instance.oppositepos)
                    {
                        Move.gameObject.SetActive(false);
                        GameManager._instance.currentpos = GameManager._instance.currentpos + 1;
                        audio.clip = audioMove;
                        audio.Play();
                        StartCoroutine(MoveChar("right"));
                        
                        Debug.Log("k");
                    }
                    else
                    {
                        Debug.Log("error");
                    }

				}
				else if(hit.transform.name == "UP"){
                    if ((GameManager._instance.currentpos +7 >= 0 && GameManager._instance.currentpos +7 < 70) && GameManager._instance.checking[GameManager._instance.currentpos +7]==1 && GameManager._instance.currentpos + 7 != GameManager._instance.oppositepos)
                    {
                        Move.gameObject.SetActive(false);
                        GameManager._instance.currentpos = GameManager._instance.currentpos + 7;
                        audio.clip = audioMove;
                        audio.Play();
                        StartCoroutine(MoveChar("up"));
                        //GameManager._instance.CheckGameFinished();
                        Debug.Log("k");
                        
                    }
                    else
                    {
                        Debug.Log("error");
                    }
                }
				else if(hit.transform.name == "DOWN"){
                    if ((GameManager._instance.currentpos - 7 >= 0 && GameManager._instance.currentpos - 7 < 70) && GameManager._instance.checking[GameManager._instance.currentpos - 7] == 1 && GameManager._instance.currentpos -7 != GameManager._instance.oppositepos)
                    {
                        Move.gameObject.SetActive(false);
                        GameManager._instance.currentpos = GameManager._instance.currentpos - 7;
                        audio.clip = audioMove;
                        audio.Play();
                        StartCoroutine(MoveChar("down"));
                        //GameManager._instance.CheckGameFinished();
                        Debug.Log("k");
                    }
                    else
                    {
                        Debug.Log("error");
                    }
                }
				else if(hit.transform.name == "LEFT"){
                    if ((GameManager._instance.currentpos -1 >= 0 && GameManager._instance.currentpos -1 < 70) && GameManager._instance.checking[GameManager._instance.currentpos -1] == 1 && GameManager._instance.currentpos - 1 != GameManager._instance.oppositepos)
                    {
                        Move.gameObject.SetActive(false);
                        GameManager._instance.currentpos = GameManager._instance.currentpos - 1;
                        audio.clip = audioMove;
                        audio.Play();
                        StartCoroutine(MoveChar("left"));
                        Debug.Log("k");
                        
                    }
                    else
                    {
                        Debug.Log("error");
                    }
                }
			}
		
		}

    }
}
