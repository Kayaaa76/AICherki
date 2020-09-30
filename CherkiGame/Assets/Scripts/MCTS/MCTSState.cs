using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Orignial code= https://github.com/aldidoanta/TicTacToeMCTS
public class MCTSState          //It's a class to record all the information that describes the state of the game at a certain point
{
    public Main.Turn currentTurn;
    public Deck drawDeck;
    public Deck discardDeck;
    public CardsInHand humanCards;
    public CardsInHand AICards;
    public Result stateResult;
    public bool hasDrawn;
    public Card lastDiscard;
    public CherkiMachineState.SourceDeck lastDrawDeck;

    public MCTSState(Main.Turn currentTurn, Deck drawDeck,Deck discardDeck,CardsInHand humanCards, CardsInHand AICards, bool hasDrawn,Card lastDiscard,CherkiMachineState.SourceDeck lastDrawDeck)
    {
        this.drawDeck = new Deck(drawDeck.ShallowClone());
        this.discardDeck = new Deck(discardDeck.ShallowClone());
        this.humanCards = new CardsInHand(humanCards.ShallowClone());
        this.AICards = new CardsInHand(AICards.ShallowClone());

        if (lastDiscard == null)
        {
            this.lastDiscard = null;
        }
        else
        {
            this.lastDiscard = lastDiscard;
        }

        this.currentTurn = currentTurn;
        this.hasDrawn = hasDrawn;
        this.stateResult = Result.None;
        this.lastDrawDeck = lastDrawDeck;
    }

    public enum Result
    {
        HumanWin,
        AIWin,
        Draw,
        None,
        Count
    }
}
