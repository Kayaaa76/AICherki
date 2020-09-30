using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnState : CherkiMachineState  //The state represents a player's turn
{
    CardsInHand mCards;
    Deck drawDeck;
    Deck discardDeck;
    Main.Turn mTurn;
    Main.Turn nextTurn;
    public bool hasDrawn = false;

    public TurnState(string name,Main.Turn mTurn, Main.Turn nextTurn,CardsInHand mCards,Deck drawDeck,Deck discardDeck)
    {
        this.name = name;
        this.mTurn = mTurn;
        this.nextTurn = nextTurn;
        this.mCards = mCards;
        this.drawDeck = drawDeck;
        this.discardDeck = discardDeck;
    }

    public override  bool DrawCard(SourceDeck source)         //Draw a card from a deck
    {
        //Debug.Log("Drawing card");
        if (source == SourceDeck.DrawDeck)
        {
            mCards.Add(drawDeck.Pop());
            
           // Debug.Log("Draw from draw deck");
            return true;
        }
        else if (source == SourceDeck.DiscardDeck)
        {
            mCards.Add(discardDeck.Pop());
           
           // Debug.Log("Draw from discard deck");
            return true;
        }
        else
        {
           // Debug.Log("Error, no deck selected");
            return false;
        }
    }

    public override bool DiscardCard(Card card) //Discard a card from cards in hand
    {
        if (card != null)
        {
            mCards.Remove(card);
            discardDeck.Add(card);
            discardDeck.LeftShiftElement();
            mCards.Sort();

            return true;
        }
        else
        {
            Debug.Log("Error, no card selected");
            return false;
        }
    }

    public override bool CheckVictory()
    {
        return CardsInHand.CheckVictory(mCards);
    }

    public Main.Turn MyTurn { get { return mTurn; } }
    public Main.Turn NextTurn { get { return nextTurn; } } 
}
