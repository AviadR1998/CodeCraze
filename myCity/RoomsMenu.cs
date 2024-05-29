using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class RoomsListJSON
{
    public List<string> host;
}

public class RoomsMenu : MonoBehaviour
{
    public GameObject roomsMenu;
    public GameObject roomsList;
    public GameObject waitingRoom;
    public GameObject raceField;
    public GameObject player;
    public TMP_Text player1;
    public TMP_Text player2;

    private int page;
    private string response;
    private List<string> rooms;

    // Start is called before the first frame update
    void Start()
    {
        rooms = new List<string>();
        refresh();
    }

    void OnEnable()
    {
        
    }

    public void close()
    {
        waitingRoom.SetActive(false);
        roomsList.SetActive(true);
        roomsMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        //remove room if exsist
    }

    public void playSolo()
    {
        raceField.SetActive(true);
        player.GetComponent<CharacterController>().enabled = false;
        player.GetComponent<Movement>().enabled = false;
        player.GetComponent<RaceMovment>().enabled = true;
        player.transform.position = new Vector3(-675, 11f, 368.5f);
        player.transform.LookAt(GameObject.Find("EndRace(unseen)").transform);
        close();
    }

    IEnumerator getAllRooms()
    {
        string uri = "http://localhost:5000/api/Rooms/";
        using (UnityWebRequest request = UnityWebRequest.Get(uri))
        {
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
                response = request.error;
            else 
            {
                rooms = JsonUtility.FromJson<RoomsListJSON>(request.downloadHandler.text).host;
                updateRooms();
            }
        }
    }

    IEnumerator createRoomPost()
    {
        string uri = "http://localhost:5000/api/Rooms/Create/nir";
        WWWForm form = new WWWForm();
        //form.AddField("username", "nir");
        using (UnityWebRequest request = UnityWebRequest.Post(uri, form))
        {
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
                response = request.error;
            else
            {

            }
                
        }
    }

    IEnumerator joinRoomPost(string roomName)
    {
        string uri = "http://localhost:5000/api/Rooms/Join/" + roomName;
        WWWForm form = new WWWForm();
        form.AddField("player", "nir2");
        using (UnityWebRequest request = UnityWebRequest.Post(uri, form))
        {
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
                response = request.error;
            else
            {
                roomsList.SetActive(false);
                waitingRoom.SetActive(true);
                player1.text = roomName;
                player2.text = "nir2";
            }
        }
    }

    public void createRoom()
    {
        StartCoroutine(createRoomPost());
        player1.text = "nir";
        player2.text = "--";
    }

    void updateRooms()
    {
        for (int i = 0; i < 6; i++)
        {
            if (i < rooms.Count)
            {
                GameObject.Find("Room-" + (i + 1)).GetComponentInChildren<TMP_Text>().text = rooms[i];
            }
            else
            {
                GameObject.Find("Room-" + (i + 1)).GetComponentInChildren<TMP_Text>().text = "--";
            }
        }
    }

    public void refresh()
    {
        page = 0;
        StartCoroutine(getAllRooms());
    }

    public void prev()
    {
        if (page > 0)
        {
            page -= 6;
            for (int i = 0; i < 6; i++)
            {
                GameObject.Find("Room-" + (i + 1)).GetComponentInChildren<TMP_Text>().text = rooms[page + i];
            }
        }
    }

    public void next()
    {
        if (page + 6 < response.Length)
        {
            page += 6;
            updateRooms();
        }
    }

    public void backWaitingRoom()
    {
        waitingRoom.SetActive(false);
        //remove room
        roomsList.SetActive(true);
    }

    public void endRace()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        GameObject.Find("EndRaceMenu").SetActive(false);
        GameObject.Find("RaceDetails").SetActive(false);
        player.GetComponent<Movement>().enabled = true;
        GameObject.Find("Main Camera").transform.position -= new Vector3(0, 2f, 0);
        player.transform.position = new Vector3(-720, 14, 273);
        raceField.SetActive(false);
        player.GetComponent<CharacterController>().enabled = true;
        player.GetComponent<RaceMovment>().enabled = false;
    }


    public void enterRoom(TMP_Text button)
    {
        if (button.text != "--")
        {
            StartCoroutine(joinRoomPost(button.text));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
