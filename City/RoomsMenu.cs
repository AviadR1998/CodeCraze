using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class RoomsListJSON
{
    public List<string> host;
}

[Serializable]
public class obstJSON
{
    public string obs;
}

public class RoomsMenu : MonoBehaviour
{
    public GameObject roomsMenu;
    public GameObject roomsList;
    public GameObject waitingRoom;
    public GameObject raceField;
    public GameObject player;
    public GameObject raceCar;
    public GameObject questionsAndAnswersPanel;
    public TMP_Text errorText;
    public TMP_Text player1;
    public TMP_Text player2;
    public static bool multiplayerStart;
    public static string obsList, opponent = "RedCar";
    public static SocketIOUnity socket;
    public static bool activated = false;

    private int page;
    private string response;
    private List<string> rooms;
    private string p2Name = "";
    private bool meHost, obsBool = false, joinBool = false, disJoinBool = false, removeRoomBool = false;

    const int ROOM_SLOTS = 6, QUESTION_NUMBER = 10, POSITION_X = -675, POSITION_Y_HOST = 11, POSITION_Z_JOIN = 362;
    const float POSITION_Z_HOST = 368.5f, POSITION_Y_JOIN = 11.2f;
    // Start is called before the first frame update
    void Start()
    {
        socket = new SocketIOUnity("http://" + MainMenu.serverIp + ":3000");
        socket.Connect();
        socket.On("obs", data =>
        {
            obsList = data.GetValue<string>();
            multiplayerStart = obsBool = true;
        });
        socket.On("join", data =>
        {
            p2Name = data.GetValue<string>();
            joinBool = true;
        });
        socket.On("disjoin", data =>
        {
            disJoinBool = true;
        });
        socket.On("removeRoom", data =>
        {
            removeRoomBool = true;
        });
        multiplayerStart = false;
        rooms = new List<string>();
        obsList = "";
    }

    void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Confined;
        activated = Cursor.visible = true;
        meHost = joinBool = disJoinBool = removeRoomBool = obsBool = false;
        errorText.text = "";
        opponent = "RedCar";
        refresh();
    }

    public void close(bool close)
    {
        waitingRoom.SetActive(false);
        roomsList.SetActive(true);
        roomsMenu.SetActive(false);
        PauseMenu.canPause = true;
        Movement.raceOn = false;
        meHost = false;
        errorText.text = "";
        if (close)
        {
            SceneManager.LoadSceneAsync(0);
        }
    }

    void play(float x, float y, float z)
    {
        if (AdminMission.questions.Count < QUESTION_NUMBER)
        {
            errorText.text = "Please wait a few seconds for loading the questions.";
            return;
        }
        errorText.text = "";
        raceField.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        player.GetComponent<CharacterController>().enabled = false;
        player.GetComponent<RaceMovment>().enabled = true;
        player.transform.position = new Vector3(x, y, z);
        player.transform.LookAt(GameObject.Find("EndRace(unseen)").transform);
        close(false);
    }

    public void playSolo()
    {
        if (AdminMission.questions.Count < QUESTION_NUMBER)
        {
            errorText.text = "Please wait a few seconds for loading the questions.";
            return;
        }
        play(POSITION_X, POSITION_Y_HOST, POSITION_Z_HOST);
        raceCar.GetComponent<RaceCar>().enabled = true;
    }

    IEnumerator getAllRooms()
    {
        string uri = "http://" + MainMenu.serverIp + ":5000/api/Rooms/" + MainMenu.topicListSaved;
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
        string uri = "http://" + MainMenu.serverIp + ":5000/api/Rooms/Create/" + Login.usernameConnected;
        WWWForm form = new WWWForm();
        form.AddField("topics", MainMenu.topicListSaved);
        using (UnityWebRequest request = UnityWebRequest.Post(uri, form))
        {
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
                response = request.error;
            else
            {
                socket.Emit("username", Login.usernameConnected);
                meHost = true;
                roomsList.SetActive(false);
                waitingRoom.SetActive(true);
            }
                
        }
    }

    IEnumerator DeleteRoom()
    {
        string uri = "http://" + MainMenu.serverIp + ":5000/api/Rooms/Delete/" + player1.text;
        WWWForm form = new WWWForm();
        using (UnityWebRequest request = UnityWebRequest.Delete(uri))
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
        string uri = "http://" + MainMenu.serverIp + ":5000/api/Rooms/Join/" + roomName;
        WWWForm form = new WWWForm();
        form.AddField("player", Login.usernameConnected);
        form.AddField("topics", MainMenu.topicListSaved);
        using (UnityWebRequest request = UnityWebRequest.Post(uri, form))
        {
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                response = request.error;
                errorText.text = "The room is full.\nPlease try another room.";
            }
            else
            {
                opponent = roomName;
                socket.Emit("username", Login.usernameConnected);
                roomsList.SetActive(false);
                waitingRoom.SetActive(true);
                raceCar.GetComponent<RaceCar>().enabled = false;
                player1.text = roomName;
                player2.text = Login.usernameConnected;
            }
        }
    }

    IEnumerator startMulti(string roomName)
    {
        string uri = "http://" + MainMenu.serverIp + ":5000/api/Rooms/Start/" + roomName;
        WWWForm form = new WWWForm();
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

    public void createRoom()
    {
        if (AdminMission.questions.Count < QUESTION_NUMBER)
        {
            errorText.text = "Please wait a few seconds for loading the questions.";
            return;
        }
        StartCoroutine(createRoomPost());
        errorText.text = "";
        player1.text = Login.usernameConnected;
        player2.text = "--";
    }

    void updateRooms()
    {
        for (int i = 0; i < ROOM_SLOTS; i++)
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
            page -= ROOM_SLOTS;
            for (int i = 0; i < ROOM_SLOTS; i++)
            {
                GameObject.Find("Room-" + (i + 1)).GetComponentInChildren<TMP_Text>().text = rooms[page + i];
            }
        }
    }

    public void next()
    {
        if (page + ROOM_SLOTS < response.Length)
        {
            page += ROOM_SLOTS;
            updateRooms();
        }
    }

    public void backWaitingRoom()
    {
        waitingRoom.SetActive(false);
        errorText.text = "";
        if (meHost)
        {
            StartCoroutine(DeleteRoom());
        }
        else
        {
            socket.Emit("disjoin", player1.text);
        }
        roomsList.SetActive(true);
        meHost = false;
    }

    public void endRace()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        PauseMenu.canPause = true;
        questionsAndAnswersPanel.SetActive(true);
        GameObject.Find("EndRaceMenu").SetActive(false);
        GameObject.Find("RaceDetails").SetActive(false);
        GameObject.Find("Main Camera").transform.position -= new Vector3(0, 2f, 0);
        player.transform.position = new Vector3(POSITION_X, POSITION_Y_HOST, POSITION_Z_HOST);
        raceField.SetActive(false);
        player.GetComponent<CharacterController>().enabled = true;
        player.GetComponent<RaceMovment>().enabled = false;
        SceneManager.LoadSceneAsync(0);
    }


    public void enterRoom(TMP_Text button)
    {
        if (AdminMission.questions.Count < QUESTION_NUMBER)
        {
            errorText.text = "Please wait a few seconds for loading the questions.";
            return;
        }
        errorText.text = "";
        if (button.text != "--")
        {
            StartCoroutine(joinRoomPost(button.text));
        }
    }

    public void playMulti()
    {
        if (!meHost)
        {
            errorText.text = "Only the host can start the game.";
            return;
        }
        if (player2.text == "--")
        {
            errorText.text = "Not enough players for starting the game.";
            return;
        }
        raceCar.GetComponent<RaceCar>().enabled = false;
        StartCoroutine(startMulti(Login.usernameConnected));
        while (obsList != "") { }
        multiplayerStart = true;
        if (meHost)
        {
            play(POSITION_X, POSITION_Y_HOST, POSITION_Z_HOST);
            raceCar.transform.position = new Vector3(POSITION_X, POSITION_Y_JOIN, POSITION_Z_JOIN);
        }
        else
        {
            play(POSITION_X, 11.2f, 362f);
            raceCar.transform.position = new Vector3(POSITION_X, POSITION_Y_HOST, POSITION_Z_HOST);
        }
        meHost = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (obsBool)
        {
            obsBool = false;
            if (!meHost)
            {
                play(POSITION_X, POSITION_Y_JOIN, POSITION_Z_JOIN);
                raceCar.transform.position = new Vector3(POSITION_X, POSITION_Y_HOST, POSITION_Z_HOST);
            }
        }
        if (joinBool)
        {
            joinBool = false;
            opponent = player2.text = p2Name;
        }
        if (disJoinBool)
        {
            disJoinBool = false;
            player2.text = "--";
            opponent = "RedCar";
        }
        if (removeRoomBool)
        {
            removeRoomBool = false;
            player2.text = "--";
            player1.text = "--";
            opponent = "RedCar";
            waitingRoom.SetActive(false);
            roomsList.SetActive(true);
        }
    }
}
