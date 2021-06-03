using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Policies;
using MuehleStein;
using Unity.MLAgents.Actuators;


/*
public enum Player
{
    white = 0,
    black = 1
}
public class newAgent : Agent
{
    [HideInInspector] public Player type;
    [HideInInspector] public ControlUnit controlUnit;
    public Game game = new Game();
    public override void Initialize()
    {
        controlUnit = FindObjectOfType<ControlUnit>();
    }
    public override void OnEpisodeBegin()
    {
        RequestDecision();
        type = GetComponent<BehaviorParameters>().TeamId == 0 ? Player.white : Player.black;
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        var myg = game.GetBoard();
        foreach(var val in myg)
        {
            sensor.AddOneHotObservation(val, 1);
        }
       
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        GenerateInput(actions.DiscreteActions);

        if(game.addStone==true)
        {
            AddReward(0.1f);
            game.addStone = false;
        }
        if(game.removeStone==true)
        {
            AddReward(0.3f);
            game.removeStone = false;
        }
        if(game.el)
        {
            AddReward(-0.1f);
            game.el = false;
        }
    }

    public void GenerateInput(ActionSegment<int> action)
    {
        int n1 = 0, n2 = 0;
        n1 = action[0];
        n2 = action[1];
        controlUnit.playerInput = n1.ToString() + " " + n2.ToString();
        Debug.Log(controlUnit.playerInput);

        RequestDecision();
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            var act = actionsOut.DiscreteActions;
            act[0] = Random.Range(0, 9);
            act[1] = Random.Range(0, 9);
        }
    }
}
*/