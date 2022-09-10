using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Mirror;

public class CreateRoomUI : MonoBehaviour
{
    [SerializeField]
    private List<RawImage> avatarImgs;

    [SerializeField]
    private List<Button> playerCountButtons;

    private CreateGameRoomData roomData;
    // Start is called before the first frame update
    void Start(){
        roomData = new CreateGameRoomData(){ playerCount = 10 };
        UpdateAvatarImages();
    }

    private void UpdateAvatarImages(){
        for(int i = 0; i < avatarImgs.Count; i++){
            if(i < roomData.playerCount){
                avatarImgs[i].gameObject.SetActive(true);
            }else{
                avatarImgs[i].gameObject.SetActive(false);
            }
        }
    }

    public void UpdatePlayerCount(int count){
        roomData.playerCount = count;
        for(int i = 0; i < playerCountButtons.Count; i++){
            if(i == count - 4){
                playerCountButtons[i].image.color = new Color(1f,1f,1f,1f);
            }else{
                playerCountButtons[i].image.color = new Color(1f,1f,1f,0f);
            }
        }
        UpdateAvatarImages();
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
    public int playerCount;
}
