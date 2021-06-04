using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class StoneAgent : Agent
{
  public bool IsWhiteAgent;
  public ControlUnit ControlUnit;

  /// <summary>
  /// At the start of each episode, OnEpisodeBegin() is called to set-up the
  /// environment for a new episode. Typically the scene is initialized in a
  /// random manner to enable the agent to learn to solve the task under a
  /// variety of conditions.
  /// </summary>
  public override void OnEpisodeBegin()
  {
    if (ControlUnit.Game.isWhiteTurn == IsWhiteAgent)
    {
      //Debug.LogError("Reset game");
      ControlUnit.OnResetBtnDown();
    }
  }

  /// <summary>
  /// The Agent sends the information we collect to the Brain, which uses it to
  /// make a decision. When you train the Agent (or use a trained model), the
  /// data is fed into a neural network as a feature vector. For an Agent to
  /// successfully learn a task, we need to provide the correct information. A
  /// good rule of thumb for deciding what information to collect is to consider
  /// what you would need to calculate an analytical solution to the problem.
  /// </summary>
  /// <param name="sensor"></param>
  public override void CollectObservations(VectorSensor sensor)
  {
    //Debug.LogError(gameObject.name + " CollectObservations");
    // TODO: Add those data for observation
    // Total white stones
    // Total black stones

    // Board data
    var myg = ControlUnit.Game.GetBoard();
    //Debug.LogError("board size: " + myg.Length);
    foreach (var val in myg)
    {
      sensor.AddOneHotObservation(val, 1);
    }
  }

  /// <summary>
  /// Receives actions and assigns the reward.
  /// Reinforcement learning requires rewards to signal which decisions are good
  /// and which are bad. The learning algorithm uses the rewards to determine
  /// whether it is giving the Agent the optimal actions. You want to reward an
  /// Agent for completing the assigned task.
  /// </summary>
  /// <param name="actionBuffers"></param>
  public override void OnActionReceived(ActionBuffers actionBuffers)
  {
    if (actionBuffers.DiscreteActions.Length == 0)
      return;
    Vector2 input = Vector3.zero;
    input.x = actionBuffers.DiscreteActions[0];
    input.y = actionBuffers.DiscreteActions[1];

    //Debug.Log(gameObject.name + " OnActionReceived: " + input.x + " " + input.y);
    AddReward(ProcessStep((int)input.x, (int)input.y));
    
    if (ControlUnit.Game.GameEnded)
    {
      if (ControlUnit.Game.isWhiteTurn == IsWhiteAgent)
      {
        // This agent lost the game
        //Debug.Log(gameObject.name + " lost game");
      }
      else
      {
        Debug.Log(gameObject.name + " won the game");
        SetReward(1.0f);
      }
      EndEpisode();
    }
    else
    {

      if (ControlUnit.Game.isWhiteTurn == IsWhiteAgent)
      {
        if (ControlUnit.Game.IsInRemoveStoneState)
        {
          //Debug.Log(gameObject.name + " can remove a stone!");
          AddReward(0.1f);
        }
      }
    }
  }

  public override void Heuristic(in ActionBuffers actionsOut)
  {
    
    // Randomly input values
    if (ControlUnit.Game.isWhiteTurn == IsWhiteAgent && !ControlUnit.Game.GameEnded)
    {
      if (ControlUnit.Process)
      {
        if (ControlUnit.ShouldResetProcess)
        {
          ControlUnit.Process = false;
        }
        //Debug.Log(gameObject.name);
        var act = actionsOut.DiscreteActions;
        if (ControlUnit.UseValidInput)
        {
          var validInput = GetValidRandomStep();
          act[0] = (int)validInput.x;
          act[1] = (int)validInput.y;
        }
        else
        {
          act[0] = UnityEngine.Random.Range(0, 25);
          act[1] = UnityEngine.Random.Range(0, 25);
        }   
      }
    }
  }
  
  private Vector2 GetValidRandomStep()
  {
    var possibleMoves = ControlUnit.Game.GetPossibleMoves();
    int fromKeyIndex = UnityEngine.Random.Range(0, possibleMoves.Count);
    int toKeyIndex = UnityEngine.Random.Range(0, possibleMoves.ElementAt(fromKeyIndex).Value.Count);
    int from = possibleMoves.ElementAt(fromKeyIndex).Key;
    int to = possibleMoves.ElementAt(fromKeyIndex).Value.ElementAt(toKeyIndex);
    //int from = possibleMoves.ElementAt(fromKeyIndex).Key;
    //int to = possibleMoves.ElementAt(fromKeyIndex).Value.ElementAt(toKeyIndex);
    //foreach (var kvp in possibleMoves)
    //{
    //  var data = String.Join(", ", kvp.Value.ToArray());
    //  Debug.Log(kvp.Key + " -> " + data);
    //}
    return new Vector2(from, to);

  }

  private float ProcessStep(int from, int to)
  {
    if (ControlUnit.Game.isWhiteTurn == IsWhiteAgent && !ControlUnit.Game.GameEnded)
    {
      var possibleMoves = ControlUnit.Game.GetPossibleMoves();
      if (possibleMoves.ContainsKey(from) && possibleMoves[from].Contains(to))
      {
        ControlUnit.Move(from, to);
        return 0.1f;
      }
      else
      {
        return -0.01f;
      }
    }
    return 0;
  }
}
