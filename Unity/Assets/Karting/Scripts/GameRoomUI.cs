using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameRoomUI : MonoBehaviour
{
    public void ExitGameRoom(){
        var manager = NetworkManager.singleton;
        if(manager.mode == Mirror.NetworkManagerMode.Host){
            manager.StopHost();
        }else if(manager.mode == Mirror.NetworkManagerMode.ClientOnly){
            manager.StopClient();
        }
    }
}
