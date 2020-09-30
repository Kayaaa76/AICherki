using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherkiStateMachine
{
    List<TurnState> mStates;
    TurnState currentState;
    TurnState humanTurnState;
    TurnState computerTurnState;
    public TurnState CurrentState { get { return currentState; } }

    public CherkiStateMachine()
    {
        humanTurnState = new TurnState("Player Turn State", Main.Turn.Human, Main.Turn.AI, Main.Instance.playerCardsInHand, Main.Instance.drawDeck, Main.Instance.discardDeck);
        computerTurnState = new TurnState("Computer Turn State", Main.Turn.AI, Main.Turn.Human, Main.Instance.computerCardsInHand, Main.Instance.drawDeck, Main.Instance.discardDeck);

        currentState = humanTurnState;

        mStates = new List<TurnState>();
        mStates.Add(humanTurnState);
        mStates.Add(computerTurnState);
    }

    public bool Execute(CherkiMachineState.SourceDeck source)  //Execute draw action, source means to which deck to draw
    {
        if (currentState.DrawCard(source))
        {
            currentState.hasDrawn = true;
            return true;
        }
        return false;
    }

    public bool ExecuteAI(CherkiMachineState.SourceDeck source)  //Execute draw action, source means to which deck to draw
    {
        if (currentState.DrawCard(source))
        {
            currentState.hasDrawn = true;
            return true;
        }
        return false;
    }

    public bool Execute(Card card)  //Execute discard action
    {
        if (currentState.DiscardCard(card))
        {
            currentState.hasDrawn = false;

            foreach (TurnState state in mStates)   //After a card discarded, automatically change state
            {
                if (currentState.NextTurn == state.MyTurn)
                {
                    currentState = state;
                    return true;
                }
            }
        }
        return false;
    }

    public bool CheckVictory()  //Basically an interface to be used by Main script
    {
        if (currentState.CheckVictory())
        {
            Debug.Log("game finished");
            return true;
        }
        return false;
    } 
}