using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public class TreeNode             //Orignial code= https://github.com/aldidoanta/TicTacToeMCTS
{
    static System.Random r = new System.Random();
    static double epsilon = 1e-6;

    public List<TreeNode> children {get; set;} = new List<TreeNode>();
    double nVisits, totValue = 0;
    public double uctValue = 0;

    public MCTSState state;

    public TreeNode(MCTSState state)
    {
        this.state = state;
    }

    public void iterateMCTS()
    {
        LinkedList<TreeNode> visited = new LinkedList<TreeNode>();
        TreeNode cur = this;
        visited.AddLast(this);
        while (!cur.isLeaf()) //1. SELECTION
        {
            cur = cur.select();
            visited.AddLast(cur);
        }
        if (cur.state.stateResult == MCTSState.Result.None)
        {
            cur.expand(); //2. EXPANSION
            TreeNode newNode = cur.select();
            visited.AddLast(newNode);
            double value = newNode.simulate(); //3. SIMULATION

            foreach (TreeNode node in visited)
            {
                node.updateStats(value); //4. BACKPROPAGATION
            }
        }
    }

    public TreeNode select()
    {
        TreeNode selected = null;
        double bestValue = Double.MinValue;
        foreach (TreeNode c in children)
        {
            //UCT value calculation
            double uctValue = c.totValue / (c.nVisits + epsilon) + 
                              Math.Sqrt(Math.Log(nVisits + 1) / (c.nVisits + epsilon)) + 
                              r.NextDouble() * epsilon; // small random number to break ties randomly in unexpanded nodes
            c.uctValue = uctValue;
            if (uctValue > bestValue)
            {
                selected = c;
                bestValue = uctValue;
            }    
        }
        return selected;
    }

    public void expand()                     //This function takes into account of every possibility of the game and make them children of current treeNode
    {                                         //In draw phase, there are 2 possibilities, draw from drawDeck or discardDeck
                                              //In discard phase, there are 9 possibilities, because you can discard any of the 9 cards
        if (state.hasDrawn)
        {
            #region DiscardPhase   //Discard phase expand, 9 possibilities

            #region HumanTurnDiscardExpand
            if (state.currentTurn == Main.Turn.Human)
            {
                state.humanCards.LeftShiftElement();
                foreach (Card cardToDiscard in state.humanCards.GetCards())
                {
                    TreeNode childNode = new TreeNode(new MCTSState(state.currentTurn, state.drawDeck, state.discardDeck, state.humanCards, state.AICards, state.hasDrawn,state.lastDiscard,state.lastDrawDeck));

                    if (cardToDiscard != null)
                    {
                        childNode.state.discardDeck.Add(cardToDiscard);
                        childNode.state.humanCards.Remove(cardToDiscard);
                        childNode.state.discardDeck.LeftShiftElement();
                        childNode.state.humanCards.LeftShiftElement();
                        childNode.state.lastDiscard = cardToDiscard;
                        childNode.state.stateResult = MCTSState.Result.None;
                        childNode.state.hasDrawn = false;
                        childNode.state.currentTurn = Main.Turn.AI;
                        children.Add(childNode);
                    }
                }
            }
            #endregion

            #region AITurnDiscardExpand
            else if (state.currentTurn == Main.Turn.AI)
            {
                // Debug.Log("Expand AI turn, discard");
                state.humanCards.LeftShiftElement();
                foreach (Card cardToDiscard in state.AICards.GetCards())
                {
                    TreeNode childNode = new TreeNode(new MCTSState(state.currentTurn, state.drawDeck, state.discardDeck, state.humanCards, state.AICards, state.hasDrawn, state.lastDiscard, state.lastDrawDeck));

                   // Card cardToDiscard = childNode.state.AICards.GetCardByMeldType((MeldType)i);
                    if (cardToDiscard != null)
                    {
                        childNode.state.discardDeck.Add(cardToDiscard);
                        childNode.state.AICards.Remove(cardToDiscard);
                        childNode.state.discardDeck.LeftShiftElement();
                        childNode.state.AICards.LeftShiftElement();
                        childNode.state.lastDiscard = cardToDiscard;

                        childNode.state.currentTurn = Main.Turn.Human;
                        childNode.state.stateResult = MCTSState.Result.None;
                        childNode.state.hasDrawn = false;
                        children.Add(childNode);
                    }
                }
            }
            #endregion

            #endregion

        }
        else
        {
            #region DrawPhase //Draw phase expand, 2 possibilities (1 possibility when discard deck is empty)

            #region HumanTurnDrawExpand
            if (state.currentTurn == Main.Turn.Human)
            {
                //Debug.Log("Expand human turn, draw");
                for (int i = 0; i < 2; i++)
                {
                    TreeNode childNode = new TreeNode(new MCTSState(state.currentTurn, state.drawDeck, state.discardDeck, state.humanCards, state.AICards, state.hasDrawn, state.lastDiscard, state.lastDrawDeck));
                    if (i == 0)
                    {
                        if (childNode.state.discardDeck.Count > 0)
                        {
                            childNode.state.humanCards.Add(childNode.state.discardDeck.Pop());
                            childNode.state.discardDeck.LeftShiftElement();
                            childNode.state.lastDrawDeck = CherkiMachineState.SourceDeck.DiscardDeck;
                        }
                    }
                    else
                    {
                        childNode.state.humanCards.Add(childNode.state.drawDeck.Pop());
                        childNode.state.lastDrawDeck = CherkiMachineState.SourceDeck.DrawDeck;
                    }
                    
                    childNode.state.humanCards.LeftShiftElement();
                    childNode.state.hasDrawn = true;
                    if (CardsInHand.CheckVictory(childNode.state.humanCards))
                    {
                        childNode.state.stateResult = MCTSState.Result.HumanWin;
                    }
                    children.Add(childNode);
                }
            }
            #endregion

            #region AITurnDrawExpand
            else if (state.currentTurn == Main.Turn.AI)
            {
                //Debug.Log("Expand AI turn, draw");
                for (int i = 0; i < 2; i++)
                {
                    TreeNode childNode = new TreeNode(new MCTSState(state.currentTurn, state.drawDeck, state.discardDeck, state.humanCards, state.AICards, state.hasDrawn, state.lastDiscard, state.lastDrawDeck));
                    if (i == 0)
                    {
                        if (childNode.state.discardDeck.Count > 0)
                        {
                            childNode.state.AICards.Add(childNode.state.discardDeck.Pop());
                            childNode.state.discardDeck.LeftShiftElement();
                            childNode.state.lastDrawDeck = CherkiMachineState.SourceDeck.DiscardDeck;
                        }
                    }
                    else
                    {
                        childNode.state.AICards.Add(childNode.state.drawDeck.Pop());
                        childNode.state.lastDrawDeck = CherkiMachineState.SourceDeck.DrawDeck;

                    }
                    childNode.state.AICards.LeftShiftElement();
                    childNode.state.hasDrawn = true;
                    if (CardsInHand.CheckVictory(childNode.state.AICards))
                    {
                        childNode.state.stateResult = MCTSState.Result.AIWin;
                    }
                    children.Add(childNode);
                }
            }
            #endregion

            #endregion
        }
    }

    public double simulate()   //It simulates all the way to the end of the game, from currentNode
    {                        //Based on the result of the simulation(Win/lose), it return different sim value, used to judge whether its a good node or not
        MCTSState simState = new MCTSState(state.currentTurn, state.drawDeck, state.discardDeck, state.humanCards, state.AICards, state.hasDrawn, state.lastDiscard, state.lastDrawDeck);
        simState.stateResult = state.stateResult;
        int simValue = int.MinValue;

        while(simState.stateResult== MCTSState.Result.None &&simState.drawDeck.Count>0)
        {
            if (!simState.hasDrawn)//In Draw phase
            {
                simState.hasDrawn = true;
               
                if (simState.currentTurn == Main.Turn.AI) //AI's turn
                {
                    if (simState.discardDeck.Count > 0)
                    {
                        if (simState.AICards.NumOfCardsOfType(simState.discardDeck.GetTop().MeldType) == 2)  //if drawing from discard deck can form a meld
                        {
                            simState.AICards.Add(simState.discardDeck.Pop());   //Draw from discard deck 
                        }

                        else
                        {
                            simState.AICards.Add(simState.drawDeck.Pop());   //Other wise just take from draw deck
                        }  
                    }
                    else     //If no card in discard deck, draw from draw deck
                    {
                        simState.AICards.Add(simState.drawDeck.Pop());
                    }
                    
                    if (simState.drawDeck.Count <= 0)  //After draw finish, if the draw deck is empty, mark the game result as draw
                    {
                        simState.stateResult = MCTSState.Result.Draw;
                        break;
                    }

                    if (CardsInHand.CheckVictory(simState.AICards))       //If victory goal met, mark the game result as AIWIN
                    {
                        simState.stateResult = MCTSState.Result.AIWin;
                        break;
                    }
                }
                else if(simState.currentTurn == Main.Turn.Human)  //Human's turn，everything same as above, basically duplicated code
                { 
                    if (simState.discardDeck.Count > 0)
                    {
                        if (simState.humanCards.NumOfCardsOfType(simState.discardDeck.GetTop().MeldType) == 2)  //if drawing from discard deck can form a meld
                        {
                            simState.humanCards.Add(simState.discardDeck.Pop());
                        }
                        else
                        {
                            simState.humanCards.Add(simState.drawDeck.Pop());
                        }
                    }
                    else
                    {
                        simState.humanCards.Add(simState.drawDeck.Pop());
                    }
                  
                    if (simState.drawDeck.Count <= 0)
                    {
                        simState.stateResult = MCTSState.Result.Draw;
                        break;
                    }

                    if (CardsInHand.CheckVictory(simState.humanCards))
                    {
                        simState.stateResult = MCTSState.Result.HumanWin;
                        break;
                    } 
                }              
            }

            else if(simState.hasDrawn)//In discard phase
            {
                simState.hasDrawn = false;
                if (simState.currentTurn == Main.Turn.AI)  //AI's turn
                {                   
                    MeldType typeToDiscard = MostUselessType(simState.AICards);   //Get the least valuable meld type among the cards in hand
                    Card cardToDiscard = simState.AICards.GetCardByMeldType(typeToDiscard);  //get a card of that meld type, then discard it
                    simState.AICards.Remove(cardToDiscard);
                    simState.AICards.LeftShiftElement();
                    simState.discardDeck.Add(cardToDiscard);
                    simState.discardDeck.LeftShiftElement();
                    simState.currentTurn = Main.Turn.Human;                   
                }

                else   //duplicated code
                {                  
                    MeldType typeToDiscard = MostUselessType(simState.humanCards);
                    Card cardToDiscard = simState.humanCards.GetCardByMeldType(typeToDiscard);
                    simState.humanCards.Remove(cardToDiscard);
                    simState.humanCards.LeftShiftElement();
                    simState.discardDeck.Add(cardToDiscard);
                    simState.discardDeck.LeftShiftElement();
                    simState.currentTurn = Main.Turn.AI;                   
                }
            }
        }

        switch (simState.stateResult)
        {
            case MCTSState.Result.Draw:
                {
                   // Debug.Log("Draw result simed");
                    simValue = 0;
                    break;
                }
            case MCTSState.Result.HumanWin:
                {
                   // Debug.Log("HumanWin result simed");
                    simValue = -1; //1 means victory, -1 means defeat
                    break;
                }
            case MCTSState.Result.AIWin:
                {
                    //Debug.Log("AIWin result simed");
                    simValue = 1;
                    break;
                }
            default:
                {
                   // Debug.LogError("illegal simStateResult value");
                    break;
                }
        }
        return simValue;
    }

    public bool isLeaf()
    {
        return children.Count == 0;
    }

    public MeldType MostUselessType(CardsInHand handCards)  //This function is used to find the most useless meld type from a group of cards(hand cards) 
    {
        for (int i = 0; i < (int)MeldType.Count; i++)
        {
            int numOfCurrentTypeInHand = handCards.NumOfCardsOfType((MeldType)i);
            int numOfCurrentTypeInDiscard = state.discardDeck.NumOfCardsOfType((MeldType)i);
            int numOfCardsRevealed = numOfCurrentTypeInHand + numOfCurrentTypeInDiscard;
            int numOfCardsUnRevealed = 0;
            if (numOfCurrentTypeInHand % 3 != 0)  //If cards of current type is not times of 3(cannot form a meld)
            {
                if (i < 3)              //For the three special card type(each type only has 4 cards in total)
                {
                    numOfCardsUnRevealed = 4 - numOfCardsRevealed;

                    if (numOfCurrentTypeInDiscard > 1)  //If you can never form a meld with the remainning card
                    {
                        return (MeldType)i;
                    }
                }
                else if (i > 3)         //For other value card types(12 cards for each type in total)
                {
                    numOfCardsUnRevealed = 12 - numOfCardsRevealed;
                    if (numOfCurrentTypeInDiscard > 9)  //If it can never form a meld with the remainning card
                    {

                        return (MeldType)i;
                    }
                }
            }
        }

        //If none of the cards fit the above condition
        for (int i = 0; i < (int)MeldType.Count; i++)    //find the type which u have only 1 card of, loop from low value to high value
        {
            int numOfCurrentTypeInHand = handCards.NumOfCardsOfType((MeldType)i);
            if (numOfCurrentTypeInHand % 3 == 1 )
            {
                return (MeldType)i;
            }
        }

        //If none of the cards fit the above condition
        for (int i = 0; i < (int)MeldType.Count; i++)    //find any type that has not formed a meld yet, loop from low value to high value
        {
            int numOfCurrentTypeInHand = handCards.NumOfCardsOfType((MeldType)i);
            if (numOfCurrentTypeInHand % 3 >0 )
            {
                return (MeldType)i;
            }
        }
        Debug.Log("Error, No useless type found");
        return MeldType.WhiteFlower;
    }
        
    public void updateStats(double value)
    {
        nVisits++;
        totValue += value;
    }
}
