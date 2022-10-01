using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class FollowPlayer : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(this.isLocalPlayer){
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Cinemachine.CinemachineVirtualCamera>().m_LookAt = gameObject.transform;
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Follow = gameObject.transform;
        }
        
    }

}
