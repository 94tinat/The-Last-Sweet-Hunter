using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

namespace Prototype.NetworkLobby
{
    //Main menu, mainly only a bunch of callback called by the UI (setup throught the Inspector)
    public class LobbyMainMenu : MonoBehaviour 
    {
        public LobbyManager lobbyManager;

        public RectTransform lobbyServerList;
        public RectTransform lobbyPanel;
		//public GameObject selectionPanel;

        public InputField ipInput;

        public void OnEnable()
        {
            lobbyManager.topPanel.ToggleVisibility(true);
			//selectionPanel.SetActive (false);
            ipInput.onEndEdit.RemoveAllListeners();
            ipInput.onEndEdit.AddListener(onEndEditIP);
        }

        public void OnClickHost()
        {
            lobbyManager.StartHost();
			//selectionPanel.SetActive (true);
        }

        public void OnClickJoin()
        {
            lobbyManager.ChangeTo(lobbyPanel);
			//selectionPanel.SetActive (true);
            lobbyManager.networkAddress = ipInput.text;
            lobbyManager.StartClient();

            lobbyManager.backDelegate = lobbyManager.StopClientClbk;
            lobbyManager.DisplayIsConnecting();
            
            lobbyManager.SetServerInfo("Connecting...", lobbyManager.networkAddress);

            // aggiunta da eriberto
            lobbyManager.GetComponent<NetworkDiscovery>().isClient = true;

        }

        public void OnClickDedicated()
        {
            lobbyManager.ChangeTo(null);
            lobbyManager.StartServer();

            lobbyManager.backDelegate = lobbyManager.StopServerClbk;

            lobbyManager.SetServerInfo("Dedicated Server", lobbyManager.networkAddress);
        }

        public void OnClickCreateMatchmakingGame()
        {
            lobbyManager.StartMatchMaker();
            lobbyManager.matchMaker.CreateMatch(
                "The Last Sweet Hunter",
                (uint)lobbyManager.maxPlayers,
                true,
				"", "", "", 0, 0,
				lobbyManager.OnMatchCreate);

            lobbyManager.backDelegate = lobbyManager.StopHost;
            lobbyManager._isMatchmaking = true;
            lobbyManager.DisplayIsConnecting();

            lobbyManager.SetServerInfo("Matchmaker Host", lobbyManager.matchHost);
        }

        public void OnClickOpenServerList()
        {
            lobbyManager.StartMatchMaker();
            lobbyManager.backDelegate = lobbyManager.SimpleBackClbk;
            lobbyManager.ChangeTo(lobbyServerList);
        }

        void onEndEditIP(string text)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                OnClickJoin();
            }
        }

        void onEndEditGameName(string text)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                OnClickCreateMatchmakingGame();
            }
        }

    }
}
