using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MirrorBasics {

    [RequireComponent (typeof (NetworkMatch))]
    public class Player : NetworkRoomPlayer {

        public static Player localPlayer;
        [SyncVar] public string matchID;
        [SyncVar] public int playerIndex;
        [SyncVar] public string playerName;

        NetworkMatch networkMatch;

        [SyncVar] public Match currentMatch;

        [SerializeField] GameObject playerLobbyUI;

        Guid netIDGuid;

        void Awake () {
            networkMatch = GetComponent<NetworkMatch> ();
        }

        public override void OnStartServer () {
            netIDGuid = netId.ToString ().ToGuid ();
            networkMatch.matchId = netIDGuid;
        }

        public override void OnStartClient () {
            if (isLocalPlayer) {
                localPlayer = this;
            } else {
                Debug.Log ($"Spawning other player UI Prefab");

                if(UILobby.instance)
                {
                    playerLobbyUI = UILobby.instance.SpawnPlayerUIPrefab (this);
                }
            }
        }

        public override void OnStopClient () {
            Debug.Log ($"Client Stopped ({NetworkClient.isLoadingScene})");

            ClientDisconnect();
        }

        public override void OnStopServer () 
        {
            Debug.Log($"Current Connections : {NetworkServer.connections.Count}");

            if (NetworkServer.connections.Count <= 0)
            {
                NetworkManager.singleton.ServerChangeScene("Lobby");
            }

            Debug.Log ($"Client Stopped on Server ({NetworkServer.isLoadingScene})");

            ServerDisconnect();
        }

        /* 
            HOST MATCH
        */

        public void HostGame (bool publicMatch, string nickname) {
            string matchID = MatchMaker.GetRandomMatchID ();
            CmdHostGame (matchID, publicMatch, nickname);
        }

        [Command]
        void CmdHostGame (string _matchID, bool publicMatch, string _nickname) {
            matchID = _matchID;
            playerName = _nickname;
            if (MatchMaker.instance.HostGame (_matchID, this, publicMatch, out playerIndex)) {
                Debug.Log ($"Game hosted successfully, {playerName}");
                networkMatch.matchId = _matchID.ToGuid ();
                TargetHostGame (true, _matchID, playerIndex, _nickname);
            } else {
                Debug.Log ($"Game hosted failed");
                TargetHostGame (false, _matchID, playerIndex, _nickname);
            }
        }

        [TargetRpc]
        void TargetHostGame (bool success, string _matchID, int _playerIndex, string _playerName) {
            playerIndex = _playerIndex;
            matchID = _matchID;
            playerName = _playerName;
            Debug.Log ($"MatchID: {matchID} == {_matchID}, {playerName}");

            if (UILobby.instance)
            {
                UILobby.instance.HostSuccess (success, _matchID);
            }
        }

        /* 
            JOIN MATCH
        */

        public void JoinGame (string _inputID, string nickname) {
            CmdJoinGame (_inputID, nickname);
        }

        [Command]
        void CmdJoinGame (string _matchID, string _nickname) {
            matchID = _matchID;
            playerName = _nickname;
            if (MatchMaker.instance.JoinGame (_matchID, this, out playerIndex)) {
                Debug.Log ($"Game Joined successfully");
                networkMatch.matchId = _matchID.ToGuid ();
                TargetJoinGame (true, _matchID, playerIndex, _nickname);

                //Host
                if (isServer && playerLobbyUI != null) {
                    playerLobbyUI.SetActive (true);
                }
            } else {
                Debug.Log ($"Game Joined failed");
                TargetJoinGame (false, _matchID, playerIndex, _nickname);
            }
        }

        [TargetRpc]
        void TargetJoinGame (bool success, string _matchID, int _playerIndex, string _playerName) {
            playerIndex = _playerIndex;
            matchID = _matchID;
            playerName = _playerName;
            Debug.Log ($"MatchID: {matchID} == {_matchID}");

            if(UILobby.instance)
            {
                UILobby.instance.JoinSuccess (success, _matchID);
            }
        }

        /* 
            DISCONNECT
        */

        public void DisconnectGame () {
            CmdDisconnectGame ();
        }

        [Command]
        void CmdDisconnectGame () {
            ServerDisconnect ();
        }

        void ServerDisconnect () {
            MatchMaker.instance.PlayerDisconnected (this, matchID);
            RpcDisconnectGame ();
            networkMatch.matchId = netIDGuid;
        }

        [ClientRpc]
        void RpcDisconnectGame () {
            ClientDisconnect ();
        }

        void ClientDisconnect () {
            if (playerLobbyUI != null) {
                if (!isServer && !IsGameScene())
                {
                    Destroy(playerLobbyUI);
                } else {
                    playerLobbyUI.SetActive (false);
                }
            }
        }

        /* 
            SEARCH MATCH
        */

        public void SearchGame (string nickname) {
            CmdSearchGame (nickname);
        }

        [Command]
        void CmdSearchGame (string _nickname) {
            if (MatchMaker.instance.SearchGame (this, out playerIndex, out matchID)) {
                Debug.Log ($"Game Found Successfully");
                networkMatch.matchId = matchID.ToGuid ();
                playerName = _nickname;
                TargetSearchGame (true, matchID, playerIndex, _nickname);

                //Host
                if (isServer && playerLobbyUI != null) {
                    playerLobbyUI.SetActive (true);
                }
            } else {
                Debug.Log ($"Game Search Failed");
                TargetSearchGame (false, matchID, playerIndex, _nickname);
            }
        }

        [TargetRpc]
        void TargetSearchGame (bool success, string _matchID, int _playerIndex, string _playerName) {
            playerIndex = _playerIndex;
            matchID = _matchID;
            playerName = _playerName;
            Debug.Log ($"MatchID: {matchID} == {_matchID} | {success}");

            if(UILobby.instance)
            {
                UILobby.instance.SearchGameSuccess (success, _matchID);
            }
        }

        /* 
            MATCH PLAYERS
        */

        [Server]
        public void PlayerCountUpdated (int playerCount) {
            TargetPlayerCountUpdated (playerCount);
        }

        [TargetRpc]
        void TargetPlayerCountUpdated (int playerCount) {
            if (!UILobby.instance)
                return;

            if (playerCount > 1) {
                UILobby.instance.SetStartButtonActive(true);
            } else {
                UILobby.instance.SetStartButtonActive(false);
            }
        }

        /* 
            BEGIN MATCH
        */

        public void BeginGame (int mode) {
            Debug.Log($"BeginGame : {mode}");
            CmdBeginGame(mode);
        }

        [Command]
        void CmdBeginGame (int _mode) {
            MatchMaker.instance.BeginGame (matchID, _mode);
            Debug.Log ($"Game Beginning");
        }

        public void StartGame (int _mode) { //Server
            Debug.Log($"StartGame : {_mode}");
            TargetBeginGame (_mode);
        }

        [TargetRpc]
        void TargetBeginGame (int _mode) {
            Debug.Log ($"MatchID: {matchID} | Beginning");
            CmdSceneChange(_mode);
        }

        [Command]
        void CmdSceneChange(int _mode){
            Debug.Log($"SceneChange : {_mode}");
            SceneChange(_mode);
        }

        [Server]
        void SceneChange(int _mode){
            string gameSceneName = "Game" + _mode.ToString();
            Debug.Log($"GameScene : {gameSceneName}");
            NetworkManager.singleton.ServerChangeScene(gameSceneName);
        }

        public bool IsGameScene(){
            return (SceneManager.GetActiveScene().name == NetworkManager.singleton.onlineScene);
        }
    }
}