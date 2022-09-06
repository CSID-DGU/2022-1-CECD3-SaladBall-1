using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
}

public class CreateGameRoomData{
    public int teamCount;
}
