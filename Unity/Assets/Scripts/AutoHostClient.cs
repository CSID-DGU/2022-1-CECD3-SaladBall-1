using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace MirrorBasics {
    public class AutoHostClient : MonoBehaviour {

        [SerializeField] NetworkManager networkManager;

        void Start(){
            Debug.Log($"=== {Application.isBatchMode} ===");
            if(!Application.isBatchMode){
                Debug.Log($"=== Client Build ===");
                networkManager.StartClient();
            }else{
                Debug.Log($"=== Server Build ===");
            }
        }

        public void JoinLocal(){
            networkManager.networkAddress = "localhost";
            Debug.Log($"NetworkManager StartClient address:{networkManager.networkAddress}");
            networkManager.StartClient();
        }
    }
}