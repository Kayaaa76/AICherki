using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using System.Runtime.CompilerServices;
using UnityEngine.SocialPlatforms.Impl;
using TMPro;

public class CardsInHand : ICards       //The class represents the cards a player is holding
{
    //bools to check if the cardtypes are detected for the scoring system
    #region bool Conditions for Scoring system
    public bool bRedFlower = false;
    public bool bWhiteFlower = false;
    public bool bOldThousand = false;
    public bool bCount = false;
    public bool bOne = false;
    public bool bTwo = false;
    public bool bThree = false;
    public bool bFour = false;
    public bool bFive = false;
    public bool bSix = false;
    public bool bSeven = false;
    public bool bEight = false;
    public bool bNine = false;
    #endregion

    //Int conditions for detection of meld (if one int == 3 then there is a meld/set of 3s)
    #region int conditions for scoring system
    private int iRedFlower = 0;
    private int iWhiteFlower = 0;
    private int iOldThousand = 0;
    private int iCount = 0;
    private int iOne = 0;
    private int iTwo = 0;
    private int iThree = 0;
    private int iFour = 0;
    private int iFive = 0;
    private int iSix = 0;
    private int iSeven = 0;
    private int iEight = 0;
    private int iNine = 0;
    #endregion 

    public int score = 0;           //The score int to keep track of score.

    TextMeshPro tScore;

    public CardsInHand() //call to update variable mCards
    {
        mCards = new List<Card>();
    }

    public CardsInHand(List<Card> cloneList)
    {
        mCards = cloneList;
    }

    public void Sort()            //Sort the cards based on their meld type
    {                             //substantially based on their value and suit
        Card[] tempArrary = mCards.ToArray();
        for(int i = 7; i >= 0; i--)
        {
            for(int j = 0; j < i; j++)
            {
                if (tempArrary[j].MeldType > tempArrary[j + 1].MeldType)
                {
                    if (tempArrary[j] != null)
                    {
                        Card tempCard = tempArrary[j];
                        tempArrary[j] = tempArrary[j + 1];
                        tempArrary[j + 1] = tempCard;
                    }
                }
            }
        }
        List<Card> sortedCards = tempArrary.ToList();
        mCards = sortedCards;
    }

    public static bool CheckVictory(CardsInHand mCardsInHand)   //Check if there are 3 completed meld
    {
        int numOfCurrentMeldType = 0;
        int completeSetCount = 0;

        for (int i = 0; i < 12; i++)
        {
            numOfCurrentMeldType = mCardsInHand.NumOfCardsOfType((MeldType)i);

            if (numOfCurrentMeldType == 3)
            {
                completeSetCount += 1;
            }
            else if (numOfCurrentMeldType > 3)
            {
                completeSetCount += (numOfCurrentMeldType - (numOfCurrentMeldType % 3)) / 3;
            }
            if (numOfCurrentMeldType == 8 && completeSetCount == 2)  //2 completeSetCount + 2numOfCurrentMeldType = 0
            {
                Debug.Log("CHERKI");
            }
        }

        if (completeSetCount == 3)
        {
            return true;   
        }
        return false;
    }

    public Card GetCardByMeldType(MeldType meldType)   //Get the first card found of the meld type
    { 
        foreach(Card card in mCards)
        {  
            if (card.MeldType == meldType)
            {
                return card;
            }   
        }
        Debug.Log("Card not found, error");
        return null;
    }

    public void ResetScore()//To reset the score so that the it shows to possible score of the players hand everytime.
    {
        score = 0;
        bRedFlower = false;
        bWhiteFlower = false;
        bOldThousand = false;
        bCount = false;
        bOne = false;
        bTwo = false;
        bThree = false;
        bFour = false;
        bFive = false;
        bSix = false;
        bSeven = false;
        bEight = false;
        bNine = false;
    }

    public void AnalyseHand(CardsInHand mCardsInHand)//Analyse what melds are currently in the hand  eg.Ones, Nines              
    {                       
        int numOfCurrentCardsOfType = 0;
        for (int i = 0; i < 12; i++)
        {
            numOfCurrentCardsOfType = mCardsInHand.NumOfCardsOfType((MeldType)i);

            //foreach (Card card in mCards)
            if (numOfCurrentCardsOfType == 3)
            {
                foreach (Card card in mCards)
                {
                    #region meldtype
                    if (card.MeldType == MeldType.RedFlower && bRedFlower == false)//so only if it is false then it executes (to prevent spamming of the update and if statement) 
                    {
                         iRedFlower ++; //this will increase whenever there is a card type 
                        if (iRedFlower == 3)//when there is three of the same card (A Meld) then it will add the score
                        {
                            bRedFlower = true; //the bool is set to true so that the condition only occurs once.
                            score += 3;
                            //Debug.Log("RedFlower detected score is" + score);
                        }
                    }

                    if (card.MeldType == MeldType.WhiteFlower && bWhiteFlower == false)
                    {
                        iWhiteFlower++;
                        if(iWhiteFlower == 3)
                        {
                            bWhiteFlower = true;
                            score += 3;
                            //Debug.Log("WF" + score);
                        }
                    }

                    if (card.MeldType == MeldType.OldThousand && bOldThousand == false)
                    {
                        iOldThousand++;
                        if(iOldThousand == 3)
                        {
                            bOldThousand = true;
                            score += 3;
                            //Debug.Log("OT" + score);
                        }   
                    }

                    if (card.MeldType == MeldType.Count && bCount == false)
                    {
                        iCount++;
                        if(iCount == 3)
                        {
                            bCount = true;
                            score += 3;
                            //Debug.Log("C" + score);
                        }
                    }

                    if (card.MeldType == MeldType.One && bOne == false)
                    {
                        iOne++;
                        if (iOne == 3)
                        {
                            bOne = true;
                            score += 3;
                            //Debug.Log("Ones detected score is" + score);
                        }
                    }

                    if (card.MeldType == MeldType.Two && bTwo == false)
                    {
                        iTwo++;
                        if(iTwo == 3)
                        {
                            bTwo = true;
                            score += 6;
                            //Debug.Log("Twos detected score is" + score);
                        } 
                    }

                    if (card.MeldType == MeldType.Three && bThree == false)
                    {
                        iThree++;
                        if(iThree == 3)
                        {
                            bThree = true;
                            score += 9;
                            //Debug.Log("Threes detected score is" + score);
                        }
                    }

                    if (card.MeldType == MeldType.Four && bFour == false)
                    {
                        iFour++;
                        if (iFour ==3)
                        {
                            bFour = true;
                            score += 12;
                            //Debug.Log("Fours detected " + score);
                        }
                    }

                    if (card.MeldType == MeldType.Five && bFive == false)
                    {
                        iFive++;
                        if (iFive == 3)
                        {
                            bFive = true;
                            score += 15;
                        }
                    }

                    if (card.MeldType == MeldType.Six && bSix == false)
                    {
                        iSix++;
                        if(iSix == 3)
                        {
                            bSix = true;
                            score += 18;
                        }
                    }

                    if (card.MeldType == MeldType.Seven && bSeven == false)
                    {
                        iSeven++;
                        if(iSeven == 3)
                        {
                            bSeven = true;
                            score += 21;
                        }
                    }

                    if (card.MeldType == MeldType.Eight && bEight == false)
                    {
                        iEight++;
                        if(iEight == 3)
                        {
                            bEight = true;
                            score += 24;
                        }    
                    }

                    if (card.MeldType == MeldType.Nine && bNine == false)
                    {
                        iNine++;
                        if(iNine == 3)
                        {
                            bNine = true;
                            score += 27;
                        }      
                    }
                    #endregion meldtype
                }
                iRedFlower = 0;
                iWhiteFlower = 0;
                iOldThousand = 0;
                iCount = 0;
                iOne = 0;
                iTwo = 0;
                iThree = 0;
                iFour = 0;
                iFive = 0;
                iSix = 0;
                iSeven = 0;
                iEight = 0;
                iNine = 0;
            }
        }
    }
}




