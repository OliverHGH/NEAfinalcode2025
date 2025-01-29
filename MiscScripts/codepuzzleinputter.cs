using UnityEngine;
using UnityEngine.UI;
using GameManage;
public class codepuzzleinputter : MonoBehaviour
{
    MathsMethods mathsfordistance = new MathsMethods();
    public InputField inputter;
    public GameObject inputfield, gamemanager,endmessage;
    public Transform player;
    Transform enemytokill;
    bool GameWon, CloseToDesk;
    GameManagerScript manager;
    void Start()
    {
        manager= gamemanager.GetComponent<GameManagerScript>();
        GameWon = false;
        CloseToDesk = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(mathsfordistance.findDif(transform.position, player.position) < 2&& !GameWon)
        {
            inputfield.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            CloseToDesk = true;//allows player to input message when close
        }
        else
        {
            if (CloseToDesk)//makes player unable to interact when not close
            {
                inputfield.SetActive(false);
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                CloseToDesk = false;//ensures if statement isnt continuously called
            }
        }
        if (inputter.text == "lookbehindyou" )//if correct message entered, game is won
        {
            Debug.Log("GameWon");
            GameWon = true;
            enemytokill = manager.enemybodylist[0].transform;
            enemytokill.position = player.position-player.forward*2f;
            endmessage.SetActive(true);
        }
    }
}
