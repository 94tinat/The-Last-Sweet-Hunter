﻿using Prototype.NetworkLobby;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerMoveJoystick : NetworkBehaviour
{

    public Vector3 moveVector;
    public float speed;
    public Joystick joystick;
    public GameObject ps;

    public GameObject floatingText;

    private void Start()
    {

        if (isLocalPlayer)
        {
            GameObject j = GameObject.Find("MyJoystick");
            if (j != null)
            {
                joystick = j.GetComponent<Joystick>();

            }

            moveVector = new Vector3(4f, -10.9f, 0);
            gameObject.transform.position = moveVector;

            print(transform.position);
            Camera.main.GetComponent<CameraFollow>().SetPlayer(this.transform);

            //addPlayerStats();
        }



    }
    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer)
        {
            
            moveVector = new Vector3(joystick.Horizontal, joystick.Vertical, 0);
            gameObject.transform.Translate(moveVector * speed * Time.deltaTime);

        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        for (int i = 0; i < LobbyManager.s_Singleton.lobbySlots.Length; ++i)
        {
            LobbyPlayer p = LobbyManager.s_Singleton.lobbySlots[i] as LobbyPlayer;

            if (p != null && isLocalPlayer)
            {
                if (other.gameObject.CompareTag("Candy"))
                {
                    Candy candy = (Candy)other.gameObject.GetComponent<Candy>();
                    Debug.Log("Ho raccolto la caramella: " + candy.GetCandyName());
                    Score.scoreValue += candy.GetCandyPoint();

                    // meglio distruggerla la caramella quando viene raccolta piuttosto che disattivarla ma tenerla nella scena
                    // other.gameObject.SetActive(false);
                    Destroy(other.gameObject);

                    switch (candy.GetCandyName())
                    {
                        case "Basic":
                            Score.numBasic++;
                            break;
                        case "Lollipop":
                            Score.numLollipop++;
                            break;
                        case "Praline":
                            Score.numPraline++;
                            break;
                        case "Licorice":
                            Score.numLicorice++;
                            break;
                        case "GummyBear":
                            Score.numGummyBear++;
                            break;
                        case "SugarCane":
                            Score.numSugarCane++;
                            break;
                        case "Marshmallow":
                            Score.numMarshmallow++;
                            break;
                        case "Macaron":
                            Score.numMacaron++;
                            break;
                        case "Donut":
                            Score.numDonut++;
                            break;
                        case "Cupcake":
                            Score.numCupcake++;
                            break;
                    }

                    if (floatingText) ShowFloatingText(candy.GetCandyPoint());
                }
            }
        }
    }

    void ShowFloatingText(int value)
    {
        var go = Instantiate(floatingText, new Vector3(transform.position.x, transform.position.y + 1, 0), Quaternion.identity, transform);
        go.GetComponent<TextMeshPro>().text = "+ " + value;
        go.GetComponent<TextMeshPro>().fontSize = 12;

    }


    //public void addPlayerStats()
    //{
    //        GameObject psContainer = GameObject.Find("PlayersStats");
    //        int childrenCount = psContainer.transform.childCount;
    //        if (psContainer != null)
    //        {
    //            GameObject go = Instantiate(ps);
    //            go.transform.SetParent(psContainer.transform);
    //            go.GetComponent<RectTransform>().localPosition = new Vector3(0f, 0f - 100 * childrenCount, 0f);
    //            go.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
    //        }
    //}

}