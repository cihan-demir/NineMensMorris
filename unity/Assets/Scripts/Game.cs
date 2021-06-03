using System;
using System.Collections.Generic;
using System.Linq;

namespace MuehleStein
{
  //https://muehlespieler.de/x_uebungen/index.php?page=begriffe_notation
  public class Game
  {
    public bool isWhiteTurn { get; private set; }

    public enum Phase { Place, Remove, Move, Jump }

    public bool IsInRemoveStoneState { get; set; }

    public bool IsInMoveState
    {
      get
      {
        return isWhiteTurn ? white == Phase.Move : black == Phase.Move;
      }
    }

    public bool IsInPlaceState
    {
      get
      {
        return white == Phase.Place || black == Phase.Place;
      }
    }

    public bool IsInJumpState
    {
      get
      {
        return isWhiteTurn ? white == Phase.Jump : black == Phase.Jump;
      }
    }

    public bool GameEnded { get; private set; }

    private Phase white = Phase.Place;
    private Phase black = Phase.Place;
    private int turn = 0;
    private int numberOfWhiteStones = 9;
    private int numberOfBlackStones = 9;

    int[] board = new int[25];

    int[,] muehleIndex = new int[,]
    {
        //horizontal
        {1, 2, 3 },
        {4, 5, 6 },
        {7, 8, 9 },
        {10, 11, 12},
        {13, 14, 15},
        {16, 17, 18},
        {19, 20, 21},
        {22, 23, 24},

        //vertical
        {1, 10, 22},
        {4, 11, 19},
        {7, 12, 16},
        {2, 5, 8},
        {17, 20, 23},
        {9, 13, 18},
        {6, 14, 21},
        {3, 15, 24},
    };

    private List<List<int>> muehleListIndex = new List<List<int>>()
        {
            //horizontal
            new List<int>{1, 2, 3 },
            new List<int>{4, 5, 6 },
            new List<int>{7, 8, 9 },
            new List<int>{10, 11, 12},
            new List<int>{13, 14, 15},
            new List<int>{16, 17, 18},
            new List<int>{19, 20, 21},
            new List<int>{22, 23, 24},

            //vertical
            new List<int>{1, 10, 22},
            new List<int>{4, 11, 19},
            new List<int>{7, 12, 16},
            new List<int>{2, 5, 8},
            new List<int>{17, 20, 23},
            new List<int>{9, 13, 18},
            new List<int>{6, 14, 21},
            new List<int>{3, 15, 24},
        };

    private IDictionary<int, List<int>> validMoves = new Dictionary<int, List<int>>()
        {
            {1, new List<int>{2, 10 } },
            {2, new List<int>{1, 3, 5 } },
            {3, new List<int>{2, 15 } },
            {4, new List<int>{5, 11 } },
            {5, new List<int>{2, 4, 6, 8 } },
            {6, new List<int>{5, 14 } },
            {7, new List<int>{8, 12 } },
            {8, new List<int>{5, 7, 9 } },
            {9, new List<int>{8, 13} },
            {10, new List<int>{1, 11, 22 } },
            {11, new List<int>{4, 10, 12, 19 } },
            {12, new List<int>{7, 11, 16 } },
            {13, new List<int>{9, 14, 18 } },
            {14, new List<int>{6, 13, 15, 21 } },
            {15, new List<int>{3, 14, 24 } },
            {16, new List<int>{12, 17 } },
            {17, new List<int>{16, 18, 20 } },
            {18, new List<int>{13, 17 } },
            {19, new List<int>{11, 20 } },
            {20, new List<int>{17, 19, 21, 23 } },
            {21, new List<int>{14, 20 } },
            {22, new List<int>{10, 23 } },
            {23, new List<int>{20, 22, 24 } },
            {24, new List<int>{15, 23 } },
        };

    private IDictionary<int, List<List<int>>> possibleMuehle = new Dictionary<int, List<List<int>>>();

    public Game()
    {
      isWhiteTurn = true;
      IsInRemoveStoneState = false;
      Init();
    }


    public Game(int[] board, bool isWhiteTurn, bool isInRemoveStoneState, Phase phase, int numberOfWhiteStones = 9, int numberOfBlackStones = 9)
    {
      this.board = board;
      this.isWhiteTurn = isWhiteTurn;
      this.IsInRemoveStoneState = isInRemoveStoneState;
      Init();
      if (phase == Phase.Move || phase == Phase.Jump)
      {
        turn = 18;
      }
      white = phase;
      black = phase;

      this.numberOfWhiteStones = numberOfWhiteStones;
      this.numberOfBlackStones = numberOfBlackStones;
    }

    private void Init()
    {
      for (int i = 1; i != board.Length; i++)
      {
        possibleMuehle.Add(i, new List<List<int>>());
      }

      for (int i = 1; i != board.Length; i++)
      {

        foreach (List<int> muehle in muehleListIndex)
        {
          if (muehle.Contains(i))
          {
            possibleMuehle[i].Add(muehle);
          }
        }
      }
    }

    public void NextMove(int from, int to)
    {
      if (IsInRemoveStoneState)
      {
        RemoveStone(to);
      }
      else if (IsInPlaceState)
      {
        AddStone(to);
      }
      else if (IsInMoveState || IsInJumpState)
      {
        MoveStone(from, to);
      }
      else
      {
        PrintError("NextMove(" + from + ", " + to + ") invalid state");
      }
    }

    public void AddStone(int toPosition)
    {
      if (IsInRemoveStoneState)
      {
        PrintError("addStone() isInRemoveStoneStat: " + IsInRemoveStoneState);
      }

      if (board[toPosition] != 0)
      {
        PrintError("addStone() board[toPosition] is not 0");
      }

      if (IsInMoveState)
      {
        PrintError("addStone() IsInMoveState, use move instead of add");
      }

      if (GameEnded)
      {
        PrintError("addStone() GameEnded");
      }

      turn++;
      if (turn == 17)
      {
        white = Phase.Move;
      }


      if (turn == 18)
      {
        black = Phase.Move;
      }

      int value = isWhiteTurn ? 1 : -1;
      board[toPosition] = value;

      if (IsMuehle(toPosition))
      {
        IsInRemoveStoneState = true;
      }
      else
      {
        PlayerTurnEnd();
      }
    }


    public void MoveStone(int fromPosition, int toPosition)
    {
      turn++;
      if (turn <= 18)
      {
        PrintError("moveStone(" + fromPosition + " " + toPosition + ") turn: " + turn);
      }
      if (IsInRemoveStoneState)
      {
        PrintError("moveStone(" + fromPosition + " " + toPosition + ") isInRemoveStoneStat: " + IsInRemoveStoneState);
      }
      if (board[toPosition] != 0)
      {
        PrintError("moveStone(" + fromPosition + " " + toPosition + ") is not 0 board[toPosition]: " + board[toPosition]);
      }

      if (board[fromPosition] != (isWhiteTurn ? 1 : -1))
      {
        PrintError("moveStone(" + fromPosition + " " + toPosition + ") is not " + (isWhiteTurn ? 1 : -1) + " board[fromPosition]: " + board[fromPosition]);
      }

      if (!IsLegalMove(fromPosition, toPosition))
      {
        PrintError("moveStone(" + fromPosition + " " + toPosition + ") is not IsLegalMove()");
      }

      if (GameEnded)
      {
        PrintError("moveStone(" + fromPosition + " " + toPosition + ") GameEnded");
      }

      board[toPosition] = board[fromPosition];
      board[fromPosition] = 0;

      if (IsMuehle(toPosition))
      {
        IsInRemoveStoneState = true;
      }
      else
      {
        PlayerTurnEnd();
      }
    }

    public bool IsLegalMove(int fromPosition, int toPosition)
    {

      /* if (turn <= 18 || isInRemoveStoneState || board[toPosition] != 0 || board[fromPosition] != (isWhiteTurn ? 1 : -1) || GameEnded)
       {
           return false;
       }
       */
      //Place
      if (IsInPlaceState && IsValidPlaceLocation(toPosition))
      {
        return true;
      }

      if (fromPosition == -1) //only when adding the from position is empty
      {
        return false;
      }


      //move
      var value = isWhiteTurn ? 1 : -1;


      if (IsInMoveState && board[fromPosition] == value && board[toPosition] == 0 && validMoves[fromPosition].Contains(toPosition))
      {
        return validMoves[fromPosition].Contains(toPosition);
      }

      //Jump
      if (isWhiteTurn && white == Phase.Jump)
      {
        return board[toPosition] == 0;
      }

      if (!isWhiteTurn && black == Phase.Jump)
      {
        return board[toPosition] == 0;
      }
      return false;
    }


    public bool IsValidPlaceLocation(int toPosition)
    {
      return board[toPosition] == 0;
    }

    public void RemoveStone(int position)
    {
      if (!IsInRemoveStoneState)
      {
        PrintError("RemoveStone(" + position + ") isInRemoveStoneStat: " + IsInRemoveStoneState);
      }

      if (!IsRemoveLegalMove(position))
      {
        PrintError("RemoveStone(" + position + ") isRemoveLegalMove: false ");
      }

      if (isWhiteTurn)
      {
        numberOfBlackStones--;
        if (numberOfBlackStones == 3)
        {
          black = Phase.Jump;
        }
      }
      else
      {
        numberOfWhiteStones--;
        if (numberOfWhiteStones == 3)
        {
          white = Phase.Jump;
        }
      }

      IsInRemoveStoneState = false;
      board[position] = 0;
      PlayerTurnEnd();
    }

    public bool IsRemoveLegalMove(int position)
    {
      int stoneToBeRemoved = isWhiteTurn ? -1 : 1;
      if (board[position] != stoneToBeRemoved)
      {
        Console.WriteLine("isRemoveLegalMove not legal, trying to remove wrong color stone");
        return false;
      }
      if (!StoneIsInMuehle(position))
      {
        return true;
      }

      for (int i = 1; i != board.Length; i++)
      {
        if (board[i] == stoneToBeRemoved)
        {
          if (!StoneIsInMuehle(i))
          {
            return false;
          }
        }
      }
      return true;
    }

    private bool StoneIsInMuehle(int position)
    {
      foreach (List<int> muehle in possibleMuehle[position])
      {
        if (IsMuehle(board[muehle[0]], board[muehle[1]], board[muehle[2]]))
        {
          return true;
        }
      }
      return false;
    }

    public bool IsMuehle(int position)
    {
      for (int i = 0; i < muehleIndex.GetLength(0); i++)
      {
        if ((muehleIndex[i, 0] == position || muehleIndex[i, 1] == position || muehleIndex[i, 2] == position) && IsMuehle(board[muehleIndex[i, 0]], board[muehleIndex[i, 1]], board[muehleIndex[i, 2]]))
        {
          return true;
        }
      }
      return false;
    }

    private bool IsMuehle(int pos1, int pos2, int pos3)
    {
      int sum = pos1 + pos2 + pos3;
      return sum == 3 || sum == -3;
    }

    public Dictionary<int, List<int>> GetPossibleMoves()
    {
      if (IsInRemoveStoneState)
      {
        var valueToBeRemoved = isWhiteTurn ? -1 : 1;

        //are stone of that type that are not in mühle?
        var currentPositions = GetBoardPositions(valueToBeRemoved);

        var pos = currentPositions.FindAll(p => !StoneIsInMuehle(p));
        if (pos.Count() != 0)
        {

          pos.Sort();
          return new Dictionary<int, List<int>>() { { 0, pos } }; ;
        }

        currentPositions.Sort();
        return new Dictionary<int, List<int>>() { { 0, currentPositions } }; ;
      }

      //place
      if (IsInPlaceState)
      {
        return new Dictionary<int, List<int>>() { { 0, GetBoardPositions(0) } };
      }

      //move
      if (IsInMoveState)
      {
        var possibleMovesInCurrentState = new Dictionary<int, List<int>>();
        var value = isWhiteTurn ? 1 : -1;
        var currentPositions = GetBoardPositions(value);
        foreach (int position in currentPositions)
        {
          foreach (var possibleMoves in validMoves[position])
          {
            if (board[possibleMoves] == 0)
            {
              if (!possibleMovesInCurrentState.ContainsKey(position))
              {
                possibleMovesInCurrentState.Add(position, new List<int>());
              }
              possibleMovesInCurrentState[position].Add(possibleMoves);
            }
          }
        }
        return possibleMovesInCurrentState;
      }

      if (IsInJumpState)
      {
        var possibleMovesInCurrentState = new Dictionary<int, List<int>>();
        var value = isWhiteTurn ? 1 : -1;
        var currentPositions = GetBoardPositions(value);

        var currentValidJumpPositions = GetBoardPositions(0);
        foreach (int position in currentPositions)
        {
          possibleMovesInCurrentState.Add(position, currentValidJumpPositions);
        }
        return possibleMovesInCurrentState;
      }



      PrintError("GetPossibleMoves() not valid state");
      return null;
    }

    private List<int> GetBoardPositions(int value)
    {
      var r = new List<int>();
      for (int i = 1; i < board.Length; i++)
      {
        if (board[i] == value)
        {
          r.Add(i);
        }
      }
      return r;

    }

    public string GetBoardAsString()
    {
      return "" + board[1].ToSign() + "-----" + board[2].ToSign() + "-----" + board[3].ToSign() + "" + "\n" +
          "| " + board[4].ToSign() + "---" + board[5].ToSign() + "---" + board[6].ToSign() + " |" + "\n" +
          "| | " + board[7].ToSign() + " " + board[8].ToSign() + " " + board[9].ToSign() + " | |" + "\n" +
          "" + board[10].ToSign() + " " + board[11].ToSign() + " " + board[12].ToSign() + "   " + board[13].ToSign() + " " + board[14].ToSign() + " " + board[15].ToSign() + "" + "\n" +
          "| | " + board[16].ToSign() + " " + board[17].ToSign() + " " + board[18].ToSign() + " | |" + "\n" +
          "| " + board[19].ToSign() + "---" + board[20].ToSign() + "---" + board[21].ToSign() + " |" + "\n" +
          "" + board[22].ToSign() + "-----" + board[23].ToSign() + "-----" + board[24].ToSign() + "";
    }

    public void PrintBoard()
    {
      Console.WriteLine(GetBoardAsString());
    }

    public string GetText()
    {
      if (GameEnded)
      {
        return (isWhiteTurn ? "Black Won" : "White Won");
      }
      var playerInfo = isWhiteTurn ? "White Phase:" + GetPhase(white) : "Black Phase:" + GetPhase(black);
      playerInfo += IsInRemoveStoneState ? " remove a stone" : string.Empty;
      return "Player turn: " + playerInfo;
    }

    public int[] GetBoard()
    {
      return board;
    }

    private void PlayerTurnEnd()
    {
      isWhiteTurn = !isWhiteTurn;
      if (turn >= 18 && (numberOfWhiteStones == 2 || numberOfBlackStones == 2 || GetPossibleMoves().Count == 0))
      {
        GameEnded = true;
      }
    }

    private void PrintError(string v)
    {
      Console.WriteLine("Error: " + v);
      Console.WriteLine(GetText());
      Console.WriteLine("------------------------------");
      board.ToList().ForEach(b => Console.Write(b + ","));
      Console.WriteLine(isWhiteTurn + ", " + IsInRemoveStoneState + ", " + (isWhiteTurn ? GetPhase(white) : GetPhase(black)) + ", " + numberOfWhiteStones + ", " + numberOfBlackStones);
      Console.WriteLine("------------------------------");

      PrintBoard();
      throw new IllegalMoveException();
    }

    private string GetPhase(Phase phase)
    {
      switch (phase)
      {
        case Phase.Place:
          return "place";
        case Phase.Remove:
          return "remove";
        case Phase.Move:
          return "move";
        case Phase.Jump:
          return "jump";
        default:
          return "Unknow phase";
      }
    }
  }

  public class IllegalMoveException : Exception
  {

  }

  public static class ExtensionMethods
  {
    public static string ToSign(this int stoneValue)
    {
      if (stoneValue == 0)
      {
        return "0";
      }
      return stoneValue == -1 ? "o" : "x";
    }
  }
}