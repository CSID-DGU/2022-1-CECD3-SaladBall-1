using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Mirror;

public class CreateRoomUI : MonoBehaviour
{
    [SerializeField]
    private List<Button> teamCountButtons;

    private CreateGameRoomData roomData;
    // Start is called before the first frame update
    void Start()
    {
        roomData = new CreateGameRoomData(){ teamCount = 1 };
    }
    
    public void ClickBtn(){
        GameObject clickObject = EventSystem.current.currentSelectedGameObject;
        // current button value save
        // print(clickObject.name);
    }

    public void CreateRoom(){
        var manager = NetworkManager.singleton;
        // 방 설정 작업 처리
        //
        //
        manager.StartHost();
    }
}

public class CreateGameRoomData{
    public int teamCount;
}
