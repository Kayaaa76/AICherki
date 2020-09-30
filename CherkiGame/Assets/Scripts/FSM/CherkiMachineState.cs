using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherkiMachineState : IState  //Based on IState, this class is more designated to the game
{                                         //However since there is only one state class, this class is also unnecessary

    public virtual bool DrawCard(SourceDeck sourceDeck) { return false; }

    public virtual bool DiscardCard(Card card)
    {
        return false;
    }

    public virtual bool CheckVictory()
    {
        return false;
    }

    public enum SourceDeck
    {
        DrawDeck,
        DiscardDeck,
        None
    }
}
