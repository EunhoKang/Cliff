using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager _instance = null;
    public GameObject player1;
    public GameObject player2;
    public GameObject[] blockmap;
    public GameObject[] BlockSets;  //
    public GameBoard gameBoard; //
    public PlayerMove playerMove;
    public GameObject holders1;
    public GameObject holders2;

    public GameObject p1win;
    public GameObject p2win;

    public RawImage verse;
    public Image Background;

    public float p1GameFinishZone = 5.4f;  //
    public float p2GameFinishZone = -5.4f;  //

    int check;
    public int[] checking;
    public GameObject[] boom;

    public Text turnleft;
    public Text turnleft2;

    public Text wintext;
    public GameObject winCanvas;

    public int turn_num = 0;

    public int currentpos;
    public int oppositepos;
    private int temp;

    public int p1merit=0;
    public int p2merit = 0;

    public Camera camera;
    public float requiredSize;
    private float m_ZoomSpeed;
    private Vector3 m_ZoomSpeed2;
    public float m_DampTime=0.5f;
    public Transform[] cameraPoint;

    public GameObject loser=null;

    public static GameManager Instance()
    {
        return _instance;
    }

    private void Awake()
    {
        if(_instance == null )
        {
            _instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        //DontDestroyOnLoad(this);
    }

    public void SetUp()
    {
        //초기 상황 세팅
        for(int i=0;i < 8; i++)
        {
            check = 3 +( 7 * i);
            checking[check+7] = 1;
            blockmap[check].gameObject.SetActive(true);
        }
        for(int i = 0; i < 7; i++)
        {
            checking[i] = 1;
            checking[63 + i] = 1;
        }

        currentpos = 3;
        oppositepos = 66;
        temp = 0;

        verse.gameObject.SetActive(true);
        Color temper = verse.color;
        temper.a = 0;
        verse.color = temper;
        StartCoroutine(Starting());

    }

    IEnumerator Starting()
    {
        Color temper = verse.color;
        temper.a = 0;
        for (int i=0; i<10; i++)
        {   
            temper.a = temper.a+0.1f;
            verse.color = temper;
            yield return new WaitForSeconds(0.02f);
        }
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < 10; i++)
        {
            temper.a = temper.a - 0.1f;
            verse.color = temper;
            yield return new WaitForSeconds(0.02f);
        }
        TurnStart();
    }

    public void Current()
    {
        temp = currentpos;
        currentpos = oppositepos;
        oppositepos = temp;
    }

    public int ThisTurn()   //P1차례 0 P2차례 1
    {
        return turn_num%2;
    }

    public void NextTurn()
    {

        if ( ThisTurn() == 0)
        {
            if (p1merit > 0)
            {
                p1merit--;
                turnleft.text = "Opponent Turn" + "\n" + (Instance().p1merit + 1) + " Move Left";//p2
                turnleft2.text = "Your Turn " + "\n" + (Instance().p1merit + 1) + " Move Left";//p1
                return;
            }


        }
        if (ThisTurn() == 1)
        {
            if (p2merit > 0)
            {
                p2merit--;
                turnleft.text = "Your Turn " + "\n" + (p2merit + 1) + " Move Left";
                turnleft2.text = "Opponent Turn " + "\n" + (p2merit + 1) + " Move Left";
                return;
            }


        }
        Current();
        Debug.Log(p1merit);
        Debug.Log(p2merit);

        playerMove.Move.gameObject.SetActive(false);
        turn_num++;
        TurnStart();
        //StartCoroutine(ChangeTurn(ThisTurn()));
        
    }

    public void TurnStart() //
    {
        playerMove.ChangePlayer();
        if (ThisTurn() == 0)
        {
            //holders1.SetActive(true);
            //holders2.SetActive(false);
            for (int i = 0; i < holders1.transform.childCount; i++)
            {
                holders1.transform.GetChild(i).GetComponent<BSHolder>().spawnBS();
                holders2.transform.GetChild(i).GetComponent<BSHolder>().spawnBS();
            }
            
            turnleft.text = "Opponent Turn" + "\n" + (Instance().p1merit + 1) + " Move Left";//p2
            turnleft2.text = "Your Turn " + "\n" + (Instance().p1merit + 1) + " Move Left";//p1
        }
        if (ThisTurn() == 1)
        {
            //holders1.SetActive(false);
            //holders2.SetActive(true);
            for (int i = 0; i < holders2.transform.childCount; i++)
            {
                holders1.transform.GetChild(i).GetComponent<BSHolder>().spawnBS();
                holders2.transform.GetChild(i).GetComponent<BSHolder>().spawnBS();
                turnleft.text = "Your Turn " + "\n" + (p2merit + 1) + " Move Left";
                turnleft2.text = "Opponent Turn " + "\n" + (p2merit + 1) + " Move Left";
            }
        }
        
    }

    public void Resetholder()
    {
        for (int i = 0; i < holders2.transform.childCount; i++)
        {
            holders1.transform.GetChild(i).GetComponent<BSHolder>().spawnBS();
            holders2.transform.GetChild(i).GetComponent<BSHolder>().spawnBS();
        }
    }

    public void CheckGameFinished() //게임 끝났나 검사. nextturn 전에 해주기.
    {
        
        if ( ThisTurn()==0 && currentpos>62)
        {
            Debug.Log("플레이어 1의 승리");
            winCanvas.gameObject.SetActive(true);
            loser = player2;
            gameBoard.DeleteAllRow();

            
            //StartCoroutine(CameraZoom(0));
            
            wintext.text = "Kitty Beat the Doggy!";
        }
        
        if (ThisTurn() == 1 && currentpos < 7)
        {
            Debug.Log("플레이어 2의 승리");
            winCanvas.gameObject.SetActive(true);
            loser = player1;
            gameBoard.DeleteAllRow();

            
            //StartCoroutine(CameraZoom(1));
            wintext.text = "Doggy Beat the Kitty!";
            //플레이어 2의 승리
        }
        
    }

    // Use this for initialization
    void Start () {
        checking = new int[70];
        for(int i=0;i < 70; i++)
        {
            checking[i] = 0;
        }

        StartCoroutine(Fadein());

    }   

    IEnumerator Fadein()
    {
        Background.gameObject.SetActive(true);
        for (int i = 0; i < 10; i++)
        {
            Color temp = Background.color;
            temp.a = Background.color.a - 0.1f;
            Background.color = temp;
            yield return new WaitForSeconds(0.02f);
        }
        Background.gameObject.SetActive(false);
        SetUp();
    }
    /*
    IEnumerator CameraZoom(int k)
    {
        for (int i=0; i < 200; i++)
        {
            camera.orthographicSize = Mathf.SmoothDamp(camera.orthographicSize, requiredSize, ref m_ZoomSpeed, m_DampTime);
            camera.transform.position = Vector3.SmoothDamp(camera.transform.position, winpoint[k].position, ref m_ZoomSpeed2,(float)m_DampTime);
            yield return new WaitForSeconds(0.001f);
        }
    }
    */

    IEnumerator ChangeTurn(int k)
    {
        Transform temp = camera.transform;//1.73,9.84
        
        if (k == 1)
        {
            for (int i = 0; i < 20; i++)
            {
                temp.position = Vector3.Lerp(cameraPoint[1].position, cameraPoint[0].position, (i + 1) / 20);
                temp.rotation = Quaternion.Lerp(cameraPoint[1].rotation,cameraPoint[0].rotation,(i+1)/20);
                camera.transform.position = temp.position;
                camera.transform.rotation = temp.rotation;
                yield return new WaitForSeconds(0.01f);
            }
            camera.transform.position = cameraPoint[1].position;
            camera.transform.rotation = cameraPoint[1].rotation;
        }
        else if (k == 0)
        {
            for (int i = 0; i < 20; i++)
            {
                temp.position = Vector3.Lerp(cameraPoint[0].position, cameraPoint[1].position, (i + 1) / 20);
                temp.rotation = Quaternion.Lerp(cameraPoint[0].rotation, cameraPoint[1].rotation, (i + 1) / 20);
                camera.transform.position = temp.position;
                camera.transform.rotation = temp.rotation;
                yield return new WaitForSeconds(0.01f);
            }
            camera.transform.position = cameraPoint[0].position;
            camera.transform.rotation = cameraPoint[0].rotation;
        }
    }
}
