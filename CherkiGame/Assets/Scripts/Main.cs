using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class Main : MonoBehaviour
{
    public GameObject[] displayCards;                       //The game objects in the game scene that represents the player's cards
    public GameObject[] displayCards_AI;                    //The game objects in the game scene that represents the computer's cards
    public GameObject topDiscard;                           //The game object in the game scene that shows the last discard
    public GameObject animationHandlerScript;               //GameObject to hold the animationscript (Used to call coroutines from another script)

    public TextMeshProUGUI drawDeckRemaining;               //The number of cards remained in draw deck
    public TextMeshProUGUI currentStateText;                //Text to show whose turn is this
    public TextMeshProUGUI currentActionText;               //Text to show whether to draw or discard
    public TextMeshProUGUI bubbleSpeechText;
    public TextMeshProUGUI endText;                         //Text of game finish
    public TextMeshProUGUI turnText;                        //Text to show turn count
    public TextMeshProUGUI PlayerScore;                     //Text to show the player score
    public TextMeshProUGUI AiScore;                         //text to show Ai score

    public CherkiMachineState.SourceDeck deckToDraw;        //the deck the player is currently selecting
    public CherkiStateMachine mMachine;
    public EventSystem mEventSystem;                        //Reference to event system, used for select detection
    public GameObject endCanvas;                            //The game finish canvas
    public MCTSAI mAI;                                      //Reference to the AI script

    public Animator DeckAnimator;
    public Card cardToDiscard;                              //refers to the card the player is currently selecting(the card to be discarded)
    
    public static int turnCounter = 1;                      //Counting the number of turn passed

    [HideInInspector]
    public string playerScore;                              //Player score in string
    [HideInInspector]
    public string aiScore;                                  //AI score in string
    [HideInInspector]
    public bool move = false;
    [HideInInspector]
    public bool playerWin;
    [HideInInspector]
    public Deck drawDeck;
    [HideInInspector]
    public Deck discardDeck;
    [HideInInspector]
    public CardsInHand computerCardsInHand;
    [HideInInspector]
    public CardsInHand playerCardsInHand;
    [HideInInspector]
    public static bool isInitialised = false;
    [HideInInspector]
    public static bool isComplete = false;                  //If the game has completed
    [HideInInspector]
    public GameObject currentSelect;                        //Store the game object currently selected by the player
    [HideInInspector]
    public Card lastDiscardCard;                            //record the last discarded card
    [HideInInspector]
    public CherkiMachineState.SourceDeck lastDrawDeck;      //record the last deck drawn by a player

    #region Objects to Spawn
    public GameObject Plate; //declaring the gameobjects to spawn
    public GameObject WhiteFlower;
    public GameObject Count;
    public GameObject One;
    public GameObject Two;
    public GameObject Three;
    public GameObject Four;
    public GameObject Five;
    public GameObject Six;
    public GameObject Seven;
    public GameObject Eight;
    public GameObject Nine;
    #endregion

    #region Objects Spawn Conditions
    int condition = 0; //the condition will prevent the spamming of the text bubble as well as prevent unneccassary toggling of the items
    int condition1 = 0;
    int condition2 = 0;
    int condition3 = 0;
    int condition4 = 0;
    int condition5 = 0;
    int condition6 = 0;
    int condition7 = 0;
    int condition8 = 0;
    int condition9 = 0;
    int condition10 = 0;
    int condition11 = 0;

    #endregion

    public enum Turn
    {
        Human,
        AI,
        Count
    }

    private static Main _instance;

    public static Main Instance { get { return _instance; } }

    private void Awake()
    {    
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        Screen.SetResolution(640, 480, true, 60);                   //Preset resolution to be 640x480, fullscreen, 60hz
        Application.targetFrameRate = 60;                           //Set target framerate to be 60fps
    }

    // Start is called before the first frame update
    void Start()
    {
        currentSelect = null;
        drawDeck = new Deck();
        drawDeck.Initialise();                                      //Generate all the cards

        discardDeck = new Deck();
        playerCardsInHand = new CardsInHand();
        computerCardsInHand = new CardsInHand();
        mMachine = new CherkiStateMachine();

        UpdateCurrentState();  
        ShuffleDrawDeck();                                          //Shuffle the draw deck
        InitialDistribution();                                      //Distribute 8 cards to each player
        playerCardsInHand.Sort();                                   //Rearrange the hand cards
        computerCardsInHand.Sort();
        UpdateCardsInHand(playerCardsInHand, displayCards);         //Display cards of both players
        UpdateCardsInHand(computerCardsInHand, displayCards_AI);
        deckToDraw = CherkiMachineState.SourceDeck.None;
        cardToDiscard = null;
                                             

        mAI.enabled = true;                                         //Enable the AI script
        isInitialised = true;
    }

    #region Common Functions

    public void InitialDistribution()   //Distribute eight cards to each player
    {
        for (int i = 0; i < 8; i++)
        {
            playerCardsInHand.Add(drawDeck.Pop());
            computerCardsInHand.Add(drawDeck.Pop());
        }
    }

    public void ShuffleDrawDeck()       //Randomize the draw deck
    {
        drawDeck.Shuffle();
    }

    public void UpdateCurrentState()    //UI element update
    {
        currentStateText.text = mMachine.CurrentState.GetName;

        if (!mMachine.CurrentState.hasDrawn)
        {
            animationHandlerScript.GetComponent<AnimationsHandler>().Turnchanged();
            currentActionText.text = "Draw";

            if (mMachine.CurrentState.GetName == "Player Turn State")
            {
                StartCoroutine(PlayerDelay());
            }
        }
    }

    void UpdateTurnCounter()
    {
        turnText.text = "Turn: " + turnCounter;
    }

    public void UpdateDrawDeckRemaining() //UI element update
    {
        drawDeckRemaining.text = drawDeck.Count.ToString();
    }

    public void UpdateLastDiscard()  //Update the UI for displaying last discard card
    {
        if (discardDeck.GetTop() != null)
        {
            topDiscard.SetActive(true);
            TextMeshProUGUI valueText = topDiscard.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI suitText = topDiscard.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

            valueText.text = discardDeck.GetTop().Value.ToString();
            suitText.text = discardDeck.GetTop().Suit.ToString();
        }
        else if (discardDeck.GetTop() == null)
        {
            topDiscard.SetActive(false);
        }
    }

    public void Restart()
    {
        isInitialised = false;
        isComplete = false;
        SceneManager.LoadScene("Restart Scene");
    }
    #endregion

    #region Player's Moves
    public void DetectSelection()   //Allow the player to select interactable game objects in the scene
    {
        if (mEventSystem.currentSelectedGameObject != null)
        {
            if (mEventSystem.currentSelectedGameObject.layer != 5)
            {
                currentSelect = mEventSystem.currentSelectedGameObject;
            }

            if (mEventSystem.currentSelectedGameObject.CompareTag("HeldCard"))
            {
                deckToDraw = CherkiMachineState.SourceDeck.None;

                for (int i = 0; i < displayCards.Count(); i++)
                {
                    if (displayCards[i] == mEventSystem.currentSelectedGameObject)
                    {
                        cardToDiscard = playerCardsInHand.GetCards()[i];
                    }
                }
            }

            if (mEventSystem.currentSelectedGameObject.CompareTag("LastDiscard"))
            {
                cardToDiscard = null;
                deckToDraw = CherkiMachineState.SourceDeck.DiscardDeck;
            }

            if (mEventSystem.currentSelectedGameObject.CompareTag("DrawDeck"))
            {
                cardToDiscard = null;
                deckToDraw = CherkiMachineState.SourceDeck.DrawDeck;
            }
        }
    }

    public void UpdateCardsInHand(CardsInHand handCards, GameObject[] cardUIs)  //Update UI for displaying hand cards
    {
        List<Card> mCards = new List<Card>();
        mCards = handCards.GetCards();

        int playerScoreInt = handCards.score;            //Setting the player score int
        playerScore = playerScoreInt.ToString();        //Converting the int to string so we can change the Score in TMPGUI

        if (mCards.Count == 9)
        {
            cardUIs[8].gameObject.SetActive(true);
        }
        else
        {
            cardUIs[8].gameObject.SetActive(false);
        }
        for (int i = 0; i < mCards.Count(); i++)
        {
            TextMeshProUGUI valueText = cardUIs[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI suitText = cardUIs[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>();

            suitText.text = mCards[i].Suit.ToString();
            valueText.text = mCards[i].Value.ToString();
        }
    }

    public void HumanExecute()  //This function is used to draw or discard when the button clicked
    {
        if (!CardsInHand.CheckVictory(playerCardsInHand) && mMachine.CurrentState.GetName == "Player Turn State" && move == true)
        {
            DetectSelection();
            playerCardsInHand.ResetScore();//to show the potential score in the current hand right now (resets and displays at the start of every turn)
            playerCardsInHand.AnalyseHand(playerCardsInHand);

            if (!mMachine.CurrentState.hasDrawn)    //if has not drawn, then it will execute draw action
            {
                DeckAnimator.SetTrigger("Draw");
                mMachine.Execute(deckToDraw);
                lastDrawDeck = deckToDraw;
                mAI.MatchAndIterate();              //The AI will do its calculation even it's in player's turn, keeping track of the current state is necessary or the AI will be very dumb

                if (CardsInHand.CheckVictory(playerCardsInHand))
                {
                    UpdateEverything();
                    endCanvas.SetActive(true);
                    endText.text = "Player Win!";
                    isComplete = true;
                    playerWin = true;
                    PlayerScore.text = playerScore; //Setting player score
                    for (int i = 0; i < displayCards_AI.Length; i++)
                    {
                        displayCards_AI[i].GetComponentInChildren<CardImage>().enabled = true;
                    }
                }
            }
            else    //if has drawn, then it will execute discard action
            {
                if (mMachine.Execute(cardToDiscard))
                {
                    lastDiscardCard = cardToDiscard;
                    turnCounter += 1;
                    mAI.MatchAndIterate();
                    move = false;
                }
            }
            UpdateEverything();
        }
    }

    public void UpdateEverything()  //A collection of functions that are likely to be called together, mainly for UI update
    {
        ResetExecuteTarget();
        UpdateCurrentState();
        UpdateTurnCounter();
        UpdateDrawDeckRemaining();
        UpdateCardsInHand(playerCardsInHand, displayCards);
        UpdateLastDiscard();
        playerCardsInHand.ResetScore();
        playerCardsInHand.AnalyseHand(playerCardsInHand);
        SpawnObjects();
    }

    IEnumerator PlayerDelay() //A short delay to prevent players from moving before the animation is played
    {
        yield return new WaitForSeconds(2.5f);
        move = true;
    }
    #endregion

    #region Ai's Moves
    public void ResetExecuteTarget()  //Reset the deck to draw and card to discard
    {
        deckToDraw = CherkiMachineState.SourceDeck.None;
        cardToDiscard = null;
    }

    public void AIDraw(CherkiMachineState.SourceDeck sourceDeck) //AI Drawing function but called inside MCTSAI script
    {
        mMachine.Execute(sourceDeck);           //Use FSM to do the action

        if (mMachine.CheckVictory())            //If victory goal met
        {
            UpdateEverything();        
            endCanvas.SetActive(true);          //show end game canvas
            endText.text = "Computer Win!";
            isComplete = true;                  //mark the game as completed
            playerWin = false;
            AiScore.text = aiScore; ;           //Setting AI score
            for (int i = 0; i < displayCards_AI.Length; i++)
            {
                displayCards_AI[i].GetComponentInChildren<CardImage>().enabled = true;
            }
        }

        AIUpdateEverything();
    }

    public void AIDiscard(Card discard)   //AI Discarding function but called inside MCTSAI script
    {
        mMachine.Execute(discard);
        string suit;
        string value;
        value = discard.Value.ToString();
        suit = discard.Suit.ToString();
        bubbleSpeechText.text = "AI discarded: " + suit + value;
        AIUpdateEverything();
    }

    public void UpdateAICardsInHand(CardsInHand handCards, GameObject[] cardUIs)  //Update UI for displaying AI hand cards
    {
        List<Card> mCards = new List<Card>();
        mCards = handCards.GetCards();
        int aiScoreInt = handCards.score;
        aiScore = aiScoreInt.ToString();

        if (mCards.Count == 9)
        {
            cardUIs[8].gameObject.SetActive(true);
        }
        else
        {
            cardUIs[8].gameObject.SetActive(false);
        }

        for (int i = 0; i < mCards.Count(); i++)
        {
            TextMeshProUGUI valueText = cardUIs[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI suitText = cardUIs[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>();

            suitText.text = mCards[i].Suit.ToString();
            valueText.text = mCards[i].Value.ToString();
        }

        handCards.AnalyseHand(computerCardsInHand);
        int.TryParse(aiScore, out handCards.score);
    }

    public void AIUpdateEverything()  //A collection of functions that are likely to be called together, mainly for UI update
    {
        ResetExecuteTarget();
        UpdateCurrentState();
        UpdateTurnCounter();
        UpdateDrawDeckRemaining();
        UpdateAICardsInHand(computerCardsInHand, displayCards_AI);
        UpdateLastDiscard();
    }
    #endregion

    public void SpawnObjects() //Function to spawn gameobjects when conditions are met
    {
        if(playerCardsInHand.bRedFlower == true && condition == 0)
        {
            Plate.SetActive(true);
            Debug.Log("plate spawned");
            bubbleSpeechText.text = "You Can't eat without a plate!";//Changes the text of the speech bubble
            StartCoroutine(animationHandlerScript.GetComponent<AnimationsHandler>().BubbleSpeechText());//Plays animation of  speech bubble
            condition += 1;//changes the condition int so the spawning cannot be done more than once.
        }

        if (playerCardsInHand.bWhiteFlower == true && condition1 == 0)
        {
            WhiteFlower.SetActive(true);
            bubbleSpeechText.text = "Beef Rendang is a popular nyonya cuisine";
            StartCoroutine(animationHandlerScript.GetComponent<AnimationsHandler>().BubbleSpeechText());
            condition1 += 1;
        }

        if(playerCardsInHand.bCount == true && condition2 == 0)
        {
            Count.SetActive(true);
            bubbleSpeechText.text = "Peranakan's like to keep their food in tiffins!";
            StartCoroutine(animationHandlerScript.GetComponent<AnimationsHandler>().BubbleSpeechText());
            condition2 += 1;
        }

        if (playerCardsInHand.bOne == true && condition3 ==0)
        {
            One.SetActive(true);
            bubbleSpeechText.text = "Kuih Dedar is a delicous desert!";
            StartCoroutine(animationHandlerScript.GetComponent<AnimationsHandler>().BubbleSpeechText());
            condition3 += 1;
        }

        if (playerCardsInHand.bTwo == true && condition4 == 0)
        {
            Two.SetActive(true);
            bubbleSpeechText.text = "Kuih Salat is a delicous desert!";
            StartCoroutine(animationHandlerScript.GetComponent<AnimationsHandler>().BubbleSpeechText());
            condition4 += 1;
        }

        if (playerCardsInHand.bThree == true && condition5 == 0)
        {
            Three.SetActive(true);
            bubbleSpeechText.text = "Kuih Lapis is a multi layered, multi flavoured Kuih";
            StartCoroutine(animationHandlerScript.GetComponent<AnimationsHandler>().BubbleSpeechText());
            condition5 += 1;
        }

        if (playerCardsInHand.bFour == true && condition6 == 0)
        {
            Four.SetActive(true);
            bubbleSpeechText.text = "Ondeh Ondeh will make your mouth water!";
            StartCoroutine(animationHandlerScript.GetComponent<AnimationsHandler>().BubbleSpeechText());
            condition6 += 1;
        }

        if (playerCardsInHand.bFive == true && condition7 == 0)
        {
            Five.SetActive(true);
            bubbleSpeechText.text = "Peranakan Teapots are bright, vibrant & very pretty UwU";
            StartCoroutine(animationHandlerScript.GetComponent<AnimationsHandler>().BubbleSpeechText());
            condition7 += 1;
        }

        if (playerCardsInHand.bSix == true && condition8 ==0)
        {
            Six.SetActive(true);
            bubbleSpeechText.text = "Cymbals are commonly used in traditional Nyonya Music!";
            StartCoroutine(animationHandlerScript.GetComponent<AnimationsHandler>().BubbleSpeechText());
            condition8 += 1;
        }

        if (playerCardsInHand.bSeven == true && condition9 == 0)
        {
            Seven.SetActive(true);
            bubbleSpeechText.text = "A chinese flute can always be found in a Peranakan band!";
            StartCoroutine(animationHandlerScript.GetComponent<AnimationsHandler>().BubbleSpeechText());
            condition9 += 1;
        }

        if (playerCardsInHand.bEight == true && condition10 == 0)
        {
            Eight.SetActive(true);
            bubbleSpeechText.text = "Chinese Drums are the most common percussion instrument in nyonya music.";
            StartCoroutine(animationHandlerScript.GetComponent<AnimationsHandler>().BubbleSpeechText());
            condition10 += 1;
        }

        if (playerCardsInHand.bNine == true && condition11 == 0)
        {
            Nine.SetActive(true);
            //No item for Nine ;
            condition11 += 1;
        }
    }
}
