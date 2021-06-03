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

  public List<Stone> WhiteStones;
  public List<Stone> BlackStones;
  public List<Field> Fields;


  private static bool DEBUG = false;
  private string playerInput;

  public void Awake()
  {
    if (game == null)
    {
      ResetStoneViews();
      ResetBoardView();
      if (DEBUG)
      {
        int[] board = new int[25];
        for (int i = 1; i != board.Length; i++)
        {
          if (i < 19)
            board[i] = i % 2 == 1 ? 1 : -1;
        }
        game = new Game(board, true, false, Game.Phase.Move);
        UpdateBoardView(board);
      }
      else
      {
        ResetGame();
      }

      UpdateText();
    }
  }

  public void ResetGame()
  {
    game = new Game();
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
      if (input.Length == 2)
      {
        int from, to;
        if (int.TryParse(input[0], out from) && int.TryParse(input[1], out to))
        {
          var possibleMoves = game.GetPossibleMoves();
          if (possibleMoves.ContainsKey(from) && possibleMoves[from].Contains(to))
          {
            ResetStoneViews();
            ResetBoardView();
            game.NextMove(from, to);
            UpdateBoardView(game.GetBoard());
            UpdateText();
          }
        }
      }
      playerInput = "";
    }

    inputText.GetComponent<TextMeshProUGUI>().SetText(playerInput);
  }

  private void UpdateBoardView(int[] board)
  {
    for (int i = 1; i != board.Length; i++)
    {
      if (board[i] == 1)
      {
        Fields[i - 1].AddStone(WhiteStones[0]);
        WhiteStones.RemoveAt(0);
      }

      if (board[i] == -1)
      {
        Fields[i - 1].AddStone(BlackStones[0]);
        BlackStones.RemoveAt(0);
      }
    }
  }

  public void ResetBoardView()
  {
    Fields.ForEach(f => f.ResetField());
  }

  public void ResetStoneViews()
  {
    WhiteStones.ForEach(s => s.ResetStone());
    BlackStones.ForEach(s => s.ResetStone());
  }

  private void UpdateText()
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
        if (desc.stone == null)
        {
          break;
        }

        if (desc.stone.CurrentField != null)
        {
          fromPosition = int.Parse(desc.stone.CurrentField.name);
        }

        if (game.IsLegalMove(fromPosition, toPosition))
        {
          Stone.DROP_ON_FIELD = true;
          if (desc.stone.CurrentField == null)
          {
            game.AddStone(toPosition);
          }
          else
          {
            game.MoveStone(fromPosition, toPosition);

            if (desc.stone.CurrentField != null)
            {
              desc.stone.CurrentField.StoneDropOffOnOtherField();
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
        Debug.Log("RemoveStone Request from field " + " " + " to " + desc.stone.CurrentField.name + " stone: " + desc.stone);
        game.RemoveStone(int.Parse(desc.stone.CurrentField.name));
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

    UpdateText();
  }

  public void OnResetBtnDown()
  {
    ResetStoneViews();
    ResetBoardView();
    ResetGame();
    UpdateText();
  }
}