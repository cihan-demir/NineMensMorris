using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

enum State
{
  Invalid = -1,
  PlaceStone = 0,
  RemoveStone = 1,
  JumpStone = 2,
  MoveStone = 3,
  WaitForMove = 4,
}

public class StoneAgent : Agent
{
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
    // TODO: Add those data for observation
    // Board info
    // Total white stones
    // Total black stones

    // Target and Agent positions
    //sensor.AddObservation(Target.localPosition);
    //sensor.AddObservation(this.transform.localPosition);

    // Agent velocity
    //sensor.AddObservation(rBody.velocity.x);
    //sensor.AddObservation(rBody.velocity.z);
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
}
