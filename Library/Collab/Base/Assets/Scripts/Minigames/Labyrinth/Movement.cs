using System.Collections;
using Prototype.NetworkLobby;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class Movement : NetworkBehaviour
{
    [SerializeField]
    private GameObject player;
    private static Character character;

    private int x = DijkstraSquare.GetDimensions()[0];
    private int y = DijkstraSquare.GetDimensions()[1];
    private Node[,] matrix;
    private Graph g;

    private float gap;

    private bool flagClient = false;

    public GameObject floatingText;

    private void Start()
    {

        /**
         * ATTENZIONE:
         * Mi devo preoccupare che tutta la scena venga caricata correttamente prima di fare operazioni su di essa.
         * Per fare cio' non posso mettere nello start un ciclo while che aspetti qualcosa o un certo ammontare di tempo
         * perche' Unity si impianta e noi non possiamo passare le giornate a piangere.
         * Quindi facciamo così: facciamo partire una coroutine in cui aspetteremo che la scena si carichi, nel frattempo 
         * Unity sarà contento perchè puo' finire il suo start del ca**o.
         * Vedi proseguo della spiegazione sopra la definizione della coroutine.
         **/
        StartCoroutine("startMovement");

    }

    /**
     * Questa e' la coroutine che ci salva la vita.
     * Aspettiamo che la scena sia pronta in ogni client, o meglio finchè non e' pronta in ogni client perdiamo un frame (return null)
     * Dopodiche' facciamo partire il setup Player e siamo tutti happy :)
     **/
    IEnumerator startMovement()
    {

        if (isServer) {
           
            float time = 0f;
            while (time < 2f)
            {
                time += Time.deltaTime;
                //PANNELLO DI CARICAMENTO
               
            }
        }

        while(!ClientScene.ready)
        {
            yield return null;
        }

        if (isServer) RpcSetupPlayer();

    }

    [ClientRpc]
    public void RpcSetupPlayer()
    {
        
        player.transform.position = new Vector3(-5, -3, 0);
        character = new Character(0, 0, player);

        if (isLocalPlayer)
        {
            Camera.main.GetComponent<LabCameraFollow>().SetPlayer(this.transform);
        }
        matrix = DijkstraSquare.GetMatrix();
        g = DijkstraSquare.GetGraph();
        gap = DijkstraSquare.GetGap();

       
    }

    void Update()
    {
        if (isLocalPlayer && character != null)
        {
            print("sono nell'update");
            int i = character.x;
            int j = character.y;

            if (SwipeDetector.GetSwipeUp())
            {
                if (j + 1 < y && g.areConnected(matrix[i, j], matrix[i, j + 1]))
                {
                    player.transform.position = player.transform.position + transform.up * gap;
                    character.y = character.y + 1;
                }
                SwipeDetector.GestureCompleted();
            }

            if (SwipeDetector.GetSwipeDown())
            {
                if (j - 1 >= 0 && g.areConnected(matrix[i, j], matrix[i, j - 1]))
                {
                    player.transform.position = player.transform.position + transform.up * -gap;
                    character.y = character.y - 1;
                }
                SwipeDetector.GestureCompleted();
            }
            if (SwipeDetector.GetSwipeRight())
            {
                if (i + 1 < x && g.areConnected(matrix[i, j], matrix[i + 1, j]))
                {
                    player.transform.position = player.transform.position + transform.right * gap;
                    character.x = character.x + 1;
                }
                SwipeDetector.GestureCompleted();
            }
            if (SwipeDetector.GetSwipeLeft())
            {
                if (i - 1 >= 0 && g.areConnected(matrix[i, j], matrix[i - 1, j]))
                {
                    player.transform.position = player.transform.position + transform.right * -gap;
                    character.x = character.x - 1;
                }
                SwipeDetector.GestureCompleted();
            }
        }
    }


    void OnTriggerEnter2D(Collider2D col)
    {

        for (int i = 0; i < LobbyManager.s_Singleton.lobbySlots.Length; ++i)
        {
            LobbyPlayer p = LobbyManager.s_Singleton.lobbySlots[i] as LobbyPlayer;

            if (p != null && isLocalPlayer)
            {
                int points = col.gameObject.GetComponent<Candy>().GetCandyPoint();
                p.IncrementScore(points);
                Debug.Log("aggiunto " + points + " punti");

                Destroy(col.gameObject);

                switch (col.gameObject.GetComponent<Candy>().GetCandyName())
                {
                    case "Basic":
                        //Score.numBasic++;
                        Debug.Log("Sono basic");
                        break;
                    case "ChocolateBar":
                        Debug.Log("Sono chocolate");
                        break;

                }

                if (floatingText) ShowFloatingText(points);
               
            }
        }

    }

    void ShowFloatingText(int value)
    {
        var go = Instantiate(floatingText, new Vector3(player.transform.position.x, player.transform.position.y +1, 0), Quaternion.identity, transform);
        go.GetComponent<TextMeshPro>().text = "+ "+value;
    }

}
