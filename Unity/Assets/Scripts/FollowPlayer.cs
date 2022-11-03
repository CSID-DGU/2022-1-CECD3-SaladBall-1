using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class FollowPlayer : NetworkBehaviour
{
    void Start()
    {
        if (this.isServer)
            return;

        if(this.isLocalPlayer || hasAuthority)
        {
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Cinemachine.CinemachineVirtualCamera>().m_LookAt = gameObject.transform;
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Follow = gameObject.transform;
        }
        
    }

}
