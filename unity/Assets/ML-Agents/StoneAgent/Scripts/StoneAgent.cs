using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

/*
public enum State
{
  Invalid = -1,
  PlaceStone = 0,
  RemoveStone = 1,
  JumpStone = 2,
  MoveStone = 3,
  WaitForMove = 4,
}
*/

//public enum Player
//{
//  White = 0,
//  Black = 1
//}

public class StoneAgent : Agent
{
  //public Player Player;
  //public State PlayState = State.WaitForMove;
  public bool IsWhiteAgent;
  public ControlUnit ControlUnit;

  void Start()
  {
        
  }

  /// <summary>
  /// At the start of each episode, OnEpisodeBegin() is called to set-up the
  /// environment for a new episode. Typically the scene is initialized in a
  /// random manner to enable the agent to learn to solve the task under a
  /// variety of conditions.
  /// </summary>
  public override void OnEpisodeBegin()
  {
    //Debug.Log(gameObject.name + " OnEpisodeBegin");
    /*
    // If the Agent fell, zero its momentum
    if (this.transform.localPosition.y < 0)
    {
      this.rBody.angularVelocity = Vector3.zero;
      this.rBody.velocity = Vector3.zero;
      this.transform.localPosition = new Vector3(0, 0.5f, 0);
    }

    // Move the target to a new spot
    Target.localPosition = new Vector3(Random.value * 8 - 4,
                                       0.5f,
                                       Random.value * 8 - 4);
    */
    // For turn based game, we manually request the agent to make decision
    //RequestDecision();
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
    var myg = ControlUnit.game.GetBoard();
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

    if (actionBuffers.ContinuousActions.Length == 0)
      return;
    //Debug.LogError("OnActionReceived");
    //Debug.LogError(actionBuffers.ContinuousActions);
    //foreach(var d in actionBuffers.ContinuousActions)
    //{
    //  Debug.LogError(d);
    //}
    Vector3 input = Vector3.zero;
    input.x = actionBuffers.ContinuousActions[0];
    input.y = actionBuffers.ContinuousActions[1];
    Debug.LogError(gameObject.name + ": " + input.ToString());
    //Debug.LogError(gameObject.name + " OnActionReceived");

    //GenerateInput(actionBuffers.DiscreteActions);
    /*
    if (ControlUnit.game.addStone == true)
    {
      AddReward(0.1f);
      ControlUnit.game.addStone = false;
    }
    if (game.removeStone == true)
    {
      AddReward(0.3f);
      game.removeStone = false;
    }
    if (game.el)
    {
      AddReward(-0.1f);
      game.el = false;
    }
    */
    /*
    // Actions, size = 2
    Vector3 controlSignal = Vector3.zero;
    controlSignal.x = actionBuffers.ContinuousActions[0];
    controlSignal.z = actionBuffers.ContinuousActions[1];
    rBody.AddForce(controlSignal * forceMultiplier);

    // Rewards
    float distanceToTarget = Vector3.Distance(this.transform.localPosition, Target.localPosition);

    // Reached target
    if (distanceToTarget < 1.42f)
    {
      SetReward(1.0f);
      EndEpisode();
    }

    // Fell off platform
    else if (this.transform.localPosition.y < 0)
    {
      EndEpisode();
    }
    */
  }

  public override void Heuristic(in ActionBuffers actionsOut)
  {
    //Debug.LogError(ControlUnit.game.isWhiteTurn);
    if (ControlUnit.game.isWhiteTurn == IsWhiteAgent)
    {
      //Debug.LogError(gameObject.name + " Heuristic");
      var act = actionsOut.ContinuousActions;
      int from = Random.Range(0, 9);
      int to = Random.Range(0, 9);
      act[0] = from;
      act[1] = to;
      ControlUnit.Move(from, to);
    }
  }

}
