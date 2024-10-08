using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoccerMovment : MonoBehaviour
{
    public GameObject arrow;
    public GameObject boy;
    public GameObject player;
    public GameObject ball;
    public GameObject[] pointGame;
    public GameObject[] ballPos;

    Vector3 enemyKeeperPos;
    int score, rnd;
    float rndf;
    bool kick, turn;
    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        enemyKeeperPos = new Vector3(pointGame[0].transform.position.x, pointGame[0].transform.position.y, pointGame[0].transform.position.z);
        turn = true;
        kick = false;
        boy.SetActive(true);
        initiateTurn(0, 1, 0, new Vector3(0, 2.1f, 0), new Vector3(0, 2f, 0));
    }

    void initiateTurn(int i, int j, int ballI, Vector3 boyStart, Vector3 ballStart)
    {
        boy.transform.position = pointGame[i].transform.position - boyStart;
        player.transform.position = pointGame[j].transform.position;
        arrow.transform.position = pointGame[j].transform.position - new Vector3(6.2f, 0.6f, -0.3f);
        ball.transform.position = ballPos[ballI].transform.position + ballStart;
        player.transform.LookAt(pointGame[i].transform);
        arrow.transform.LookAt(pointGame[i].transform);
        boy.transform.LookAt(pointGame[j].transform);
    }

    // Update is called once per frame
    void Update()
    {
        if (turn)
        {
            playerTurn();
        }
        else
        {
            npcTurn();
        }
    }

    void playerTurn()
    {
        if (!kick && (Input.GetKey("right") || Input.GetKey("d")) && pointGame[0].transform.position.z < -12.6f)
        {
            pointGame[0].transform.position += new Vector3(0, 0, 0.2f);
        }
        if (!kick && (Input.GetKey("left") || Input.GetKey("a")) && pointGame[0].transform.position.z > -22.4f)
        {
            pointGame[0].transform.position -= new Vector3(0, 0, 0.2f);
        }
        if (!kick && Input.GetKey("space"))
        {
            kick = true;
            rnd = Random.Range(0, 4);
            rndf = Random.Range(-21.7f, -13.3f);
        }
        if (kick)
        {
            if (rnd == 0)
            {
                boy.transform.position = Vector3.MoveTowards(boy.transform.position, new Vector3(boy.transform.position.x, boy.transform.position.y, rndf), Time.deltaTime * 5);
                //boy.transform.position = Vector3.MoveTowards(boy.transform.position, new Vector3(boy.transform.position.x, boy.transform.position.y, -13.3f), Time.deltaTime * 5);
            }
            /*if (rnd == 1)
            {
                boy.transform.position = Vector3.MoveTowards(boy.transform.position, new Vector3(boy.transform.position.x, boy.transform.position.y, -21.7f), Time.deltaTime * 5);
            }*/
            if (rnd == 2 || rnd == 1)
            {
                if (pointGame[0].transform.position.z > -13.3f)
                {
                    boy.transform.position = Vector3.MoveTowards(boy.transform.position, new Vector3(boy.transform.position.x, boy.transform.position.y, -13.3f), Time.deltaTime * 5);
                }
                else
                {
                    if (pointGame[0].transform.position.z < -21.7f)
                    {
                        boy.transform.position = Vector3.MoveTowards(boy.transform.position, new Vector3(boy.transform.position.x, boy.transform.position.y, -21.7f), Time.deltaTime * 5);
                    }
                    else
                    {
                        boy.transform.position = Vector3.MoveTowards(boy.transform.position, pointGame[0].transform.position - new Vector3(0, 2f, 0), Time.deltaTime * 5);
                    }
                }
            }
            transform.Rotate(0, 13, 13);
            ball.transform.position = Vector3.MoveTowards(ball.transform.position, pointGame[0].transform.position - new Vector3(4, 1.5f, 0), Time.deltaTime * 10);
        }
        arrow.transform.LookAt(pointGame[0].transform);
    }

    void npcKick()
    {
        kick = true;
        rndf = Random.Range(-12.6f, -22.4f);
    }

    void npcTurn()
    {
        if ((Input.GetKey("right") || Input.GetKey("d")) && player.transform.position.z < -12.6f)
        {
            player.transform.position += new Vector3(0, 0, 0.2f);
        }
        if ((Input.GetKey("left") || Input.GetKey("a")) && player.transform.position.z > -22.4f)
        {
            player.transform.position -= new Vector3(0, 0, 0.2f);
        }
        if (kick)
        {
            transform.Rotate(0, 13, 13);
            ball.transform.position = Vector3.MoveTowards(ball.transform.position, new Vector3(player.transform.position.x + 4, player.transform.position.y, rndf), Time.deltaTime * 15);
        }
    }

    void triggerHit(Collider other, int addScore)
    {
        if (other.tag == "NPC" || other.tag == "Player")
        {
            kick = false;
        }
        if (other.tag == "StopLine")
        {
            score += addScore;
            kick = false;
        }
        if ((turn && score % 10 == 3) || (!turn && score / 10 == 3))
        {
            if (turn)
            {
                //player win
                print("you win");
            }
            else
            {
                //npc win
                print("you lose");
            }
        }
        else
        {
            if (turn)
            {
                arrow.SetActive(false);
                initiateTurn(3, 2, 1, new Vector3(0, 2.1f, 0), new Vector3(0, 1f, -0.1f));
                Invoke("npcKick", 2);
            }
            else
            {
                arrow.SetActive(true);
                pointGame[0].transform.position = enemyKeeperPos;
                initiateTurn(0, 1, 0, new Vector3(0, 2.1f, 0), new Vector3(0, 2f, 0));
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (kick)
        {
            if (turn)
            {
                triggerHit(other, 1);
            }
            else
            {
                triggerHit(other, 10);
            }
            turn = !turn;
        }
    }
}
