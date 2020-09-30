using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading;

public class MCTSAI : MonoBehaviour        //Original code= https://github.com/aldidoanta/TicTacToeMCTS
{
    public static Main.Turn myTurn = Main.Turn.AI;  //Refers to what turn the AI belongs to
    public int iterationNumber;                     //The number decides how many times the AI will iterate

    [HideInInspector]
    public TreeNode treeNode;

    bool flag = false;
    bool draw = true;

    public Animator AiAnim;

    void Start()
    {
        initAI();
    }

    void Update()
    {
        if (Main.isInitialised)
        {
            if (Main.Instance.mMachine.CurrentState.MyTurn == myTurn && !Main.isComplete)
            {
                if (!Main.Instance.mMachine.CurrentState.hasDrawn)                   //if has not drawn
                {
                    StartCoroutine(DrawingDelay());

                    if (draw == false)
                    {
                        AiAnim.SetTrigger("Drawing");
                        MCTSIterate();                                               //Get state that matches the current game state, then expand and simulate
                        treeNode = treeNode.select();                                //Based on calculation, select the best node to proceed
                        Main.Instance.AIDraw(treeNode.state.lastDrawDeck);           //Draw from a deck according to the node we just selected
                        Main.Instance.lastDrawDeck = treeNode.state.lastDrawDeck;    //Update game/board information 

                        if (CardsInHand.CheckVictory(Main.Instance.computerCardsInHand))
                        {
                            treeNode.state.stateResult = MCTSState.Result.AIWin;
                        }
                    }
                }
                else if (Main.Instance.mMachine.CurrentState.hasDrawn)              //If has drawn
                {
                    //AiAnim.SetBool("Thinking", true);
                    StartCoroutine(DiscardingDelay());
                    if (draw == true)
                    {
                        AiAnim.SetTrigger("Discarded");
                        //AiAnim.SetTrigger("Discarding");
                        //AiAnim.SetBool("Thinking", false);
                        //AiAnim.SetTrigger("AIDiscard");
                        MCTSIterate();                                              //Get state that matches the current game state, then expand and simulate
                        treeNode = treeNode.select();                               //Based on calculation, select the best node to proceed
                        Main.turnCounter += 1;
                        Main.Instance.AIDiscard(treeNode.state.lastDiscard);        //Draw a card according to the node selected  
                        Main.Instance.lastDiscardCard = treeNode.state.lastDiscard; //Update game/board information
                        MCTSIterate();                                              //Now the children number has became zero, we need to iterate to get children
                    }
                }
            }
        }
    }

    public void initAI()   //Initiating the treeNode
    {
        treeNode = new TreeNode(new MCTSState(Main.Instance.mMachine.CurrentState.MyTurn, Main.Instance.drawDeck, Main.Instance.discardDeck, Main.Instance.playerCardsInHand, Main.Instance.computerCardsInHand, Main.Instance.mMachine.CurrentState.hasDrawn, null, CherkiMachineState.SourceDeck.None));
    }

    public void MatchAndIterate()
    {
        FindMatchedNode();
        MCTSIterate();
    }

    public void MCTSIterate()
    {
        //Debug.Log("Iteration: ");
        for (int i = 0; i < iterationNumber; i++)
        {
            treeNode.iterateMCTS();
        }
    }

    public void FindMatchedNode()   //Among all the children nodes, find the one whose state is exactly the same as the game's current state
    {                               //("state" is not FSM state, it is the MCTSState that contain all the information of the game at a certain point)
        flag = false;               //Replace current treeNode with the matched child node
        if (treeNode.children.Count > 0)   //If the current treeNode does not have any children(Happen the first time it runs), create a new node
        {
            //Debug.Log("treeNode children >0");
            if (Main.Instance.lastDiscardCard != null)
            {
                foreach (TreeNode child in treeNode.children)   //Loop through all the children
                {
                    if (child.state.lastDiscard == Main.Instance.lastDiscardCard       //find the child that match the current state of the game
                        && child.state.lastDrawDeck == Main.Instance.lastDrawDeck)
                    {
                        // Debug.Log("Found the target node");
                        treeNode = child;
                        flag = true;
                        break;
                    }
                }
            }
            if (!flag) Debug.Log("unreachable code");
        }
        else
        {
            //Debug.Log("new node created");
            treeNode = new TreeNode(new MCTSState(Main.Instance.mMachine.CurrentState.MyTurn, Main.Instance.drawDeck, Main.Instance.discardDeck, Main.Instance.playerCardsInHand, Main.Instance.computerCardsInHand, Main.Instance.mMachine.CurrentState.hasDrawn, Main.Instance.lastDiscardCard, Main.Instance.lastDrawDeck));
        }
    }

    IEnumerator DrawingDelay()
    {
        yield return new WaitForSeconds(3);
        draw = false;      
    }

    IEnumerator DiscardingDelay()
    {
        yield return new WaitForSeconds(3);
        draw = true;
    }
}
