using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour {
    public static int xGrid = 7;
    public static int yGrid = 8;
    private GameObject[,] board = new GameObject[xGrid, yGrid];
    private bool[,] shownBlock = new bool[xGrid,yGrid]; //임시적으로 보여지는 블록 추적
    private int[] playerLoc;
    //public bool[,] renderedBlock = new bool[xGrid, yGrid]; //이미 올려진 블록인지

    public ParticleSystem m_ExplosionParticles;

    public AudioClip ExplosionAudio;
    public AudioClip audioFall;

    private AudioSource audio;

    public GameObject p1;
    public GameObject p2;
    public GameObject ground1;
    public GameObject ground2;

    void Start()    //초기화
    {
        audio = gameObject.AddComponent<AudioSource>();
        int index = 0;
        for (int i = 0; i < yGrid; i++)
        {
            for (int j = 0; j < xGrid; j++)
            {
                board[j, i] = this.transform.GetChild(index++).gameObject;
                shownBlock[j, i] = false;
            }
        }
        playerLoc = new int[2];
        playerLoc[0] = -1;
        playerLoc[1] = -1;
    }

    public bool AllBSOnBoard(GameObject BS)
    {
        int shownNum = 0;
        for (int i = 0; i < xGrid; i++)
        {
            for (int j = 0; j < yGrid; j++)
            {
                if (shownBlock[i, j])
                {
                    shownNum = shownNum + 1;
                }
            }
        }
        //Debug.Log("shownNum =" + shownNum);
        //Debug.Log("chilCount = " + BS.transform.childCount);
        return (shownNum == BS.transform.childCount);
    }

    public bool BlockOverlayed()    //이미 블록이 올라간 자리에 겹쳤는가
    {
        for (int i = 0; i < xGrid; i++)
        {
            for (int j = 0; j < yGrid; j++)
            {
                if (shownBlock[i, j] && (GameManager.Instance().checking[i + xGrid * (j+1)] == 1))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void ShowBlockOnBoard(GameObject block)
    {
        for (int i = 0; i < xGrid; i++)
        {
            if (board[i, 0].transform.position.x-0.6 < block.transform.position.x && block.transform.position.x <= board[i, 0].transform.position.x + 0.6)
            {
                for (int j = 0; j < yGrid; j++)
                {
                    if (board[0, j].transform.position.z - 0.6 < block.transform.position.z && block.transform.position.z <= board[0, j].transform.position.z + 0.6)
                    {
                        shownBlock[i, j] = true;
                        GameManager.Instance().blockmap[i + xGrid * j].SetActive(true);
                        return;
                    }
                }
            }
        }
    }

    public void HideShownBlocks()
    {
        for (int i = 0; i < xGrid; i++)
        {
            for (int j = 0; j < yGrid; j++)
            {
                if (shownBlock[i, j])
                {
                    shownBlock[i, j] = false;
                    if (GameManager.Instance().checking[i + xGrid * (j+1)] == 0)
                    {
                        GameManager.Instance().blockmap[i + xGrid * j].SetActive(false);
                    }
                }
            }
        }
    }

    public void ShowBS(GameObject BS) //원래 render되었던 Block 제거,block위치 체크하여 그 위치에 block render
    {
        HideShownBlocks();
        for (int i = 0; i < BS.transform.childCount; i++)
        {
            ShowBlockOnBoard(BS.transform.GetChild(i).gameObject);
        }
    }

    public void RenderBlockOnBoard()
    {
        for (int i = 0; i < xGrid; i++)
        {
            for (int j = 0; j < yGrid; j++)
            {
                if (shownBlock[i, j])
                {
                    shownBlock[i, j] = false;
                    GameManager.Instance().checking[i + xGrid * (j+1)] = 1;
                }
            }
        }
        for (int j=0; j < yGrid; j++)
        {
            if (LineCheck(j))
            {

                DeleteRow(j);
                
            }
        }
        

    }

    public bool LineCheck(int j)  //만약 i 라인이 채워져있으면->모두 checking 이면 라인을 없애고 메리트를 줌.
    {
        int blocks = 0;
        for (int i = 0; i < xGrid; i++)
        {
            if (GameManager.Instance().checking[i + xGrid * (j+1)] == 1)
            {
                blocks++;
            }
        }
        return (blocks == xGrid);
    }
    
    IEnumerator Falling(GameObject p)
    {
        float temp = p.transform.position.y;
        yield return new WaitForSeconds(0.5f);
        audio.clip = audioFall;
        audio.Play();
        for (int i=0; i<40; i++)
        {
            temp = temp-(0.05f*i );
            p.transform.position = new Vector3 (p.transform.position.x, temp, p.transform.position.z);
            yield return new WaitForSeconds(0.005f);
        }
        
        yield return new WaitForSeconds(0.4f);
        if (p.name == "P1")
        {
            p1.transform.position = new Vector3(0, 2, -5.4f);
            
        }
        else if (p.name == "P2")
        {
            p2.transform.position = new Vector3(0, 2, 5.4f);
            
        }

    }


    IEnumerator ExplodeBlock(int j)
    {


        for (int i = 0; i < xGrid; i++)
        {
            
            if (GameManager.Instance().checking[i + xGrid * (j + 1)] ==1)
            {
                ParticleSystem m_instance = Instantiate(m_ExplosionParticles, new Vector3(-3.6f + i * 1.2f, 1 + Random.Range(-20, 20) / 100, -5 + j * 1.2f + Random.Range(-20, 20) / 100), Quaternion.identity);
                m_instance.Play();
                
                Destroy(m_instance.gameObject, m_ExplosionParticles.duration);
            }
            GameManager.Instance().blockmap[i + xGrid * j].SetActive(false);
            GameManager.Instance().checking[i + xGrid * (j + 1)] = 0;

            int block = i + xGrid * (j + 1);

            if (GameManager.Instance().ThisTurn() == 0)
            {
                
                if (GameManager.Instance().currentpos == block)
                {
                    GameManager.Instance().currentpos = 3;
                    StartCoroutine(Falling(p1));
                }
                else if (GameManager.Instance().oppositepos == block)
                {
                    GameManager.Instance().oppositepos = 66;
                    StartCoroutine(Falling(p2));
                }
            }
            if (GameManager.Instance().ThisTurn() == 1)
            {
                
                if (GameManager.Instance().currentpos == block)
                {
                    GameManager.Instance().currentpos = 66;
                    StartCoroutine(Falling(p2));
                    
                }
                else if (GameManager.Instance().oppositepos == block)
                {
                    GameManager.Instance().oppositepos = 3;
                    StartCoroutine(Falling(p1));
                    
                }
            }
            yield return new WaitForSeconds(0.08f);
        }
        
    }

    public void DeleteRow(int j)
    {
        audio.clip = ExplosionAudio;
        audio.Play();
        if (GameManager.Instance().ThisTurn() == 0)
        {
            GameManager.Instance().p1merit+=2;
            
        }
        if (GameManager.Instance().ThisTurn() == 1)
        {
            GameManager.Instance().p2merit+=2;
        }
        for (int i = 0; i < xGrid; i++)
        {
            
            StartCoroutine(ExplodeBlock(j));
        }
        //merit();
    }

    public void DeleteAllRow()
    {
        audio.clip = ExplosionAudio;
        audio.Play();
        /*
        ParticleSystem m_instance = Instantiate(m_ExplosionParticles, new Vector3(GameManager.Instance().oppositepos%7*1.2f-3.6f, 1 ,-5+ (GameManager.Instance().oppositepos/7))*1.2f, Quaternion.identity);
        GameManager.Instance().loser.SetActive(false);
        m_instance.Play();

        Destroy(m_instance.gameObject, m_ExplosionParticles.duration);
        */
        for (int i = 0; i < yGrid; i++)
        {
            StartCoroutine(ExplodeBlock(i));
        }

    }

    //IE

    public void merit() //Finish zone이 넓어지는 메리트를 준다.
    {
        if (GameManager.Instance().ThisTurn() == 0)
        {
            GameManager.Instance().p1merit++;
            for (int i = 0; i < xGrid; i++)
            {
                GameManager.Instance().blockmap[i + xGrid * (8 - GameManager.Instance().p1merit)].SetActive(false);
                GameManager.Instance().checking[i + xGrid * (8 - GameManager.Instance().p1merit + 1)] = 0;
            }
        }
        else if (GameManager.Instance().ThisTurn() == 1)
        {
            GameManager.Instance().p2merit++;
            for (int i = 0; i < xGrid; i++)
            {
                GameManager.Instance().blockmap[i + xGrid * GameManager.Instance().p2merit - 1].SetActive(false);
                GameManager.Instance().checking[i + xGrid * ((GameManager.Instance().p2merit))] = 0;
            }
        }
    }
}
