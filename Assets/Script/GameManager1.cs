using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager1 : MonoBehaviour {

    public Transform[] Spawnpoint;
    //해당 코드는 임시로 만든 코드이므로, 실제 게임 매니져에서 수정을 권장.
    //MenuManager에 변수로 저장돼 있는 Characterskin을 불러와 게임이 시작될 떄 사용하도록 설정할 것, 
    public void Summon(GameObject T)
    {
        for(int i=0;i<Spawnpoint.Length;i++)
            Instantiate(T, Spawnpoint[i].position,Spawnpoint[i].rotation);
    }
}
