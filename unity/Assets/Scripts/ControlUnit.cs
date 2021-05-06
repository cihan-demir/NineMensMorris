using UnityEngine;
using TMPro;
using System.Collections.Generic;
using MuehleStein;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

/// <summary>
/// Example of control application for drag and drop events handle
/// </summary>
public class ControlUnit : MonoBehaviour
{
    public static Game game;

    public GameObject statusText;
    public GameObject inputText;
    private string playerInput;

    private List<Stone> whiteStones;
    private List<Stone> blackStones;
    private List<Field> fields;
    public Button resetButton;

    private static bool DEBUG = true;

    public void Awake()
    {
        if (game == null) {

            initStonesAndFields();
            if (DEBUG) { 
                int[] board = new int[25];
                for(int i = 1; i!= board.Length; i++)
                {
                    if(i < 19)
                        board[i] = i % 2 == 1 ? 1 : -1;
                }
                game = new Game(board, true, false, Game.Phase.Move);
                initBoard(board);                
            }
            else
            {
                game = new Game();
            }
            
            updateText();
        }

        Button btn = resetButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Keypad0))
        {
            playerInput += "0";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
        {
            playerInput += "1";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
        {
            playerInput += "2";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
        {
            playerInput += "3";
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
        {
            playerInput += "4";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5))
        {
            playerInput += "5";
        }
        if (Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.Keypad6))
        {
            playerInput += "6";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7) || Input.GetKeyDown(KeyCode.Keypad7))
        {
            playerInput += "7";
        }
        if (Input.GetKeyDown(KeyCode.Alpha8) || Input.GetKeyDown(KeyCode.Keypad8))
        {
            playerInput += "8";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9) || Input.GetKeyDown(KeyCode.Keypad9))
        {
            playerInput += "9";
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            playerInput = "";
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            playerInput += " ";
        }
        else if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            var input = playerInput.Split(' ');
            if(input.Length == 2)
            {
                int from, to;
                if (int.TryParse(input[0], out from) && int.TryParse(input[1], out to))
                {
                    var possibleMoves = game.GetPossibleMoves();
                    if(possibleMoves.ContainsKey(from) && possibleMoves[from].Contains(to))
                    {
                        initStonesAndFields();
                        game.NextMove(from, to);
                        initBoard(game.GetBoard());
                        updateText();
                    }                    
                }                
            }
            playerInput = "";
        }

        inputText.GetComponent<TextMeshProUGUI>().SetText(playerInput);
    }

    private void initBoard(int[] board)
    {
        for (int i = 1; i != board.Length; i++)
        {
            if (board[i] == 1)
            {
                fields[i - 1].AddStone(whiteStones[0]);
                whiteStones.RemoveAt(0);
            }

            if (board[i] == -1)
            {
                fields[i - 1].AddStone(blackStones[0]);
                blackStones.RemoveAt(0);
            }
        }
    }

    private void initStonesAndFields()
    {

        whiteStones = new List<Stone>();
        blackStones = new List<Stone>();
        fields = new List<Field>();

        GameObject[] ws = GameObject.FindGameObjectsWithTag("WhiteStone");
        foreach(GameObject go in ws) {
            whiteStones.Add(go.GetComponent<Stone>());            
        }

        GameObject[] bs = GameObject.FindGameObjectsWithTag("BlackStone");
        foreach (GameObject go in bs)
        {            
            blackStones.Add(go.GetComponent<Stone>());
        }

        GameObject[] f = GameObject.FindGameObjectsWithTag("Field");
        foreach (GameObject go in f)
        {
            fields.Add(go.GetComponent<Field>());
        }

        whiteStones.OrderBy(s => s.transform.name);
        blackStones.OrderBy(s => s.transform.name);
        fields.OrderBy(s => s.transform.name);
    }

    private void updateText()
    {
        statusText.GetComponent<TextMeshProUGUI>().SetText(game.GetText());
        Debug.Log(game.GetBoardAsString());
    }

    /// <summary>
    /// Operate all drag and drop requests and events from children cells
    /// </summary>
    /// <param name="desc"> request or event descriptor </param>
    void OnSimpleDragAndDropEvent(Field.DropEventDescriptor desc)
    {
        // Get control unit of source cell
        switch (desc.triggerType)                                               // What type event is?
        {
            case Field.TriggerType.DropRequest:                       // Request for item drag (note: do not destroy item on request)
                Debug.Log("DropRequest Request from field " + " " + " to " + desc.destinationField.name + " stone: " + desc.stone);
                int toPosition = int.Parse(desc.destinationField.name);
                int fromPosition = -1;
                if(desc.stone == null)
                {
                    break;
                }

                if(desc.stone.currentField != null)
                {
                    fromPosition = int.Parse(desc.stone.currentField.name);
                }

                if (game.IsLegalMove(fromPosition, toPosition))
                {
                    Stone.DROP_ON_FIELD = true;
                    if (desc.stone.currentField == null)
                    {
                        game.AddStone(toPosition);
                    }
                    else
                    {
                        game.MoveStone(fromPosition, toPosition);

                        if (desc.stone.currentField != null)
                        {
                            desc.stone.currentField.StoneDropOffOnOtherField();
                        }
                    }
                    desc.destinationField.AddStone(desc.stone);

                    if (game.IsMuehle(toPosition))
                    {
                        game.IsInRemoveStoneState = true;
                    }
                }


                break;
            case Field.TriggerType.RemoveStone:
                Debug.Log("RemoveStone Request from field " + " " + " to " + desc.stone.currentField.name + " stone: " + desc.stone);
                game.RemoveStone(int.Parse(desc.stone.currentField.name));
                break;

            case Field.TriggerType.EndTurn:
                Debug.Log("EndTurn");
                if (game.GameEnded)
                {
                    Debug.Log("Game ENDED, " + (game.GetText()));
                }
                break;
            default:
                Debug.Log("Unknown drag and drop event");
                break;
        }

        updateText();
    }

    void TaskOnClick()
    {

        Debug.Log("You have clicked the button!");
        game = null;
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
