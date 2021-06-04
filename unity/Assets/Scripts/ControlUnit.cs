using UnityEngine;
using TMPro;
using System.Collections.Generic;
using MuehleStein;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using Unity.MLAgents;

/// <summary>
/// Example of control application for drag and drop events handle
/// </summary>
public class ControlUnit : MonoBehaviour
{
  public static Game Game;

  public TextMeshProUGUI StatusDesc;
  public TextMeshProUGUI BoardDesc;
  public TextMeshProUGUI CommandInputText;

  public List<Stone> WhiteStones;
  public List<Stone> BlackStones;
  public List<Field> Fields;
  public Agent WhiteStone;
  public Agent BlackStone;
  public bool ShowBoardInfo = true;
  public bool StartWithDebugBoardOn = false;
  public bool Process = false;
  public bool ShouldResetProcess = false;
  public bool UseValidInput = true;
  private string playerInput;

  private void Start()
  {
    if (Game == null)
    {
      ResetStoneViews();
      ResetBoardView();
      if (StartWithDebugBoardOn)
      {
        int[] board = new int[25];
        for (int i = 1; i != board.Length; i++)
        {
          if (i < 19)
            board[i] = i % 2 == 1 ? 1 : -1;
        }
        Game = new Game(board, true, false, Game.Phase.Move);
        UpdateBoardView(board);
      }
      else
      {
        ResetGame();
      }

      UpdateText();
    }
    //WhiteStone.RequestDecision();
  }

  public void ResetGame()
  {
    Game = new Game();
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
    else if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
    {
      var input = playerInput.Split(' ');
      if (input.Length == 2)
      {
        int from, to;
        if (int.TryParse(input[0], out from) && int.TryParse(input[1], out to))
        {
          var possibleMoves = Game.GetPossibleMoves();
          if (possibleMoves.ContainsKey(from) && possibleMoves[from].Contains(to))
          {
            Move(from, to);
          }
          else
          {
            Debug.LogError("Cannot move from: " + from + " to: " + to);
          }
        }
      }
      playerInput = "";
    }

    CommandInputText.SetText(playerInput);
  }

  public void Move(int from, int to)
  {
    UpdateStoneView(from, to);
    ResetBoardView();
    Game.NextMove(from, to);
    UpdateBoardView(Game.GetBoard());
    UpdateText();
    Game.PrintBoard();
    
  }

  private void UpdateBoardView(int[] board)
  {
    for (int i = 1; i != board.Length; i++)
    {
      if (board[i] == 1)
      {
        var stone = WhiteStones.First(s => !s.IsOnBoard && s.gameObject.activeSelf);
        Fields[i - 1].SetStone(stone);
      }

      if (board[i] == -1)
      {
        var stone = BlackStones.First(s => !s.IsOnBoard && s.gameObject.activeSelf);
        Fields[i - 1].SetStone(stone);
      }
    }
  }

  public void ResetBoardView()
  {
    Fields.ForEach(f => f.ResetField());
  }

  public void UpdateStoneView(int from, int to)
  {
    //Debug.Log("from " + from + " to: " + to);
    //Debug.LogWarning(Fields.Single(f => f.gameObject.name == from.ToString()).gameObject.ToString());
    //Debug.LogWarning(Fields.Single(f => f.gameObject.name == to.ToString()).gameObject.ToString());
    //int fromIndex = int.Parse(Fields.Single(f => f.gameObject.name == from.ToString()).gameObject.ToString());
    //int toIndex = int.Parse(Fields.Single(f => f.gameObject.name == to.ToString()).gameObject.ToString());
    WhiteStones.ForEach(s => s.UpdateStone(from, to));
    BlackStones.ForEach(s => s.UpdateStone(from, to));
  }

  public void ResetStoneViews()
  {
    WhiteStones.ForEach(s => s.ResetStone());
    BlackStones.ForEach(s => s.ResetStone());
  }

  private void UpdateText()
  {
    StatusDesc.SetText(Game.GetText());
    if (ShowBoardInfo)
      BoardDesc.SetText(Game.GetBoardAsString2());
    else
      BoardDesc.SetText("");
    //Debug.Log(Game.GetText());
    //Debug.Log(Game.GetBoardAsString());
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

        if (Game.IsLegalMove(fromPosition, toPosition))
        {
          Stone.DropOnField = true;
          if (desc.stone.CurrentField == null)
          {
            Game.AddStone(toPosition);
          }
          else
          {
            Game.MoveStone(fromPosition, toPosition);

            if (desc.stone.CurrentField != null)
            {
              desc.stone.CurrentField.StoneDropOffOnOtherField();
            }
          }
          desc.destinationField.SetStone(desc.stone);

          if (Game.IsMuehle(toPosition))
          {
            Game.IsInRemoveStoneState = true;
          }
        }

        break;
      case Field.TriggerType.RemoveStone:
        Debug.Log("RemoveStone Request from field " + " " + " to " + desc.stone.CurrentField.name + " stone: " + desc.stone);
        Game.RemoveStone(int.Parse(desc.stone.CurrentField.name));
        break;

      case Field.TriggerType.EndTurn:
        Debug.Log("EndTurn");
        if (Game.GameEnded)
        {
          Debug.Log("Game ENDED, " + (Game.GetText()));
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