using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace MirrorBasics {

    public class UILobby : MonoBehaviour {

        public static UILobby instance;

        [Header ("Host Join")]
        [SerializeField] InputField joinMatchInput;
        [SerializeField] InputField nicknameInput;
        [SerializeField] List<Selectable> lobbySelectables = new List<Selectable> ();
        [SerializeField] Canvas nicknameCanvas;
        [SerializeField] Canvas connectCanvas;
        [SerializeField] Canvas lobbyCanvas;
        [SerializeField] Canvas searchCanvas;
        bool searching = false;

        [Header ("Lobby")]
        [SerializeField] Transform UIPlayerParent;
        [SerializeField] GameObject UIPlayerPrefab;
        [SerializeField] Text matchIDText;
        [SerializeField] Dropdown modeMenu;
        [SerializeField] GameObject modeBack;
        [SerializeField] GameObject beginGameButton;
        [SerializeField] List<Sprite> lobbySprite = new List<Sprite> ();
        [SerializeField] Image kartImage;
        int kartNum;

        GameObject localPlayerLobbyUI;

        void Start () {
            instance = this;
            kartNum = 0;
            kartImage.sprite = lobbySprite[kartNum];
            Debug.Log ($"start : sprite array ({lobbySprite.Count}), kartNum : ({kartNum})");
        }

        public void SetStartButtonActive (bool active) {
            beginGameButton.SetActive (active);
            modeBack.SetActive(active);
        }

        public static string Name{ get; private set; }

        public void Nickname(){
            Name = nicknameInput.text;
            nicknameCanvas.enabled = false;
            connectCanvas.enabled = true;
        }

        public void leftAvatar(){
            kartNum = (kartNum - 1 + lobbySprite.Count) % lobbySprite.Count;
            Debug.Log ($"left : sprite array ({lobbySprite.Count}), kartNum : ({kartNum})");
            kartImage.sprite = lobbySprite[kartNum];
            Player.localPlayer.GetKart(kartNum);
        }

        public void rightAvatar(){
            kartNum = (kartNum + 1 + lobbySprite.Count) % lobbySprite.Count;
            Debug.Log ($"right : sprite array ({lobbySprite.Count}), kartNum : ({kartNum})");
            kartImage.sprite = lobbySprite[kartNum];
            Player.localPlayer.GetKart(kartNum);
        }

        public void HostPublic () {
            lobbySelectables.ForEach (x => x.interactable = false);

            Player.localPlayer.HostGame (true, Name);
        }

        public void HostPrivate () {
            lobbySelectables.ForEach (x => x.interactable = false);

            Player.localPlayer.HostGame (false, Name);
        }

        public void HostSuccess (bool success, string matchID) {
            if (success) {
                lobbyCanvas.enabled = true;

                if (localPlayerLobbyUI != null) Destroy (localPlayerLobbyUI);
                localPlayerLobbyUI = SpawnPlayerUIPrefab (Player.localPlayer);
                matchIDText.text = matchID;
            } else {
                lobbySelectables.ForEach (x => x.interactable = true);
            }
        }

        public void Join () {
            lobbySelectables.ForEach (x => x.interactable = false);

            Player.localPlayer.JoinGame (joinMatchInput.text.ToUpper (), Name);
        }

        public void JoinSuccess (bool success, string matchID) {
            if (success) {
                lobbyCanvas.enabled = true;

                if (localPlayerLobbyUI != null) Destroy (localPlayerLobbyUI);
                localPlayerLobbyUI = SpawnPlayerUIPrefab (Player.localPlayer);
                matchIDText.text = matchID;
            } else {
                lobbySelectables.ForEach (x => x.interactable = true);
            }
        }

        public void DisconnectGame () {
            if (localPlayerLobbyUI != null) Destroy (localPlayerLobbyUI);
            Player.localPlayer.DisconnectGame ();

            lobbyCanvas.enabled = false;
            lobbySelectables.ForEach (x => x.interactable = true);
        }

        public GameObject SpawnPlayerUIPrefab (Player player) {
            GameObject newUIPlayer = Instantiate (UIPlayerPrefab, UIPlayerParent);
            newUIPlayer.GetComponent<UIPlayer> ().SetPlayer (player);
            newUIPlayer.transform.SetSiblingIndex (player.playerIndex - 1);

            return newUIPlayer;
        }

        public void BeginGame () {
            Player.localPlayer.BeginGame(modeMenu.value);
        }

        public void SearchGame () {
            StartCoroutine (Searching ());
        }

        public void CancelSearchGame () {
            searching = false;
        }

        public void SearchGameSuccess (bool success, string matchID) {
            if (success) {
                searchCanvas.enabled = false;
                searching = false;
                JoinSuccess (success, matchID);
            }
        }

        IEnumerator Searching () {
            searchCanvas.enabled = true;
            searching = true;

            float searchInterval = 1;
            float currentTime = 1;

            while (searching) {
                if (currentTime > 0) {
                    currentTime -= Time.deltaTime;
                } else {
                    currentTime = searchInterval;
                    Player.localPlayer.SearchGame (Name);
                }
                yield return null;
            }
            searchCanvas.enabled = false;
        }

    }
}