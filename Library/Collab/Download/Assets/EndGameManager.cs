using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class EndGameManager : NetworkBehaviour
{

    public GameObject endMatchCanvas;
    private bool alreadySent;
    private bool[] readyList;

    void Start()
    {
        //GameObject lm = GameObject.FindGameObjectWithTag("LobbyManager");
        //Debug.Log("lobbyslots trovati: " );
        //foreach (GameObject player in lobbyPlayers)
        //{
        //    if (!player.GetComponent<NetworkIdentity>().isLocalPlayer)
        //        Debug.Log("ciao vez");
        //}
        // For the host to do: use the timer and control the time.
        if (isServer) {
            //Debug.Log("EndGameManager sono il server");
            alreadySent = false;
            readyList = new bool[gameObject.GetComponent<NetworkGameTimer>().minPlayers];
            for (int i = 0; i<readyList.Length; i++)
            {
                readyList[i] = false;
            }
}
        else
        {
            alreadySent = false;
            //Debug.Log("EndGameManager sono il client");
        }
    }



    private void Update()
    {
        if (endMatchCanvas.active && !alreadySent)
        {
            GameObject toggle = endMatchCanvas.transform.Find("Toggle").gameObject;
            if ( toggle.GetComponent<Toggle>().isOn )
            {
                alreadySent = true;
                if (isServer)
                {
                    readyList[0] = true;
                }
                else
                {
                    CmdImReady();
                }
            }
        }
    }

    public void ReadyForNextMatch()
    {
        CmdImReady();
    }

    [Command]
    public void CmdImReady()
    {
        Debug.Log("Sono dentro alla cmd");
    }

}