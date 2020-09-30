using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public abstract class  ICards   //An abstract class for classes that represent a group of cards. Eg: A deck, or the cards a player is holding
{
    protected List<Card> mCards;

    public ICards()
    {
        mCards = new List<Card>();
    }

    public ICards(List<Card> mList)
    {
        mCards = mList;
    } 

    public virtual void Remove(Card card)   //Remove a card from the list
    {
         if (!mCards.Remove(card))
         {
             //Debug.Log("Unable to remove card");
             Debug.Log(card.MeldType);
         }
    }

    public virtual void Add(Card card)   //Add a card to the end of the list
    {
        mCards.Add(card);
    }

    public virtual int NumOfCardsOfType(MeldType cardType)   //get the number of cards that belong to a meld type
    {
        int count = 0;
        if (mCards.Count!=0)
        {
            foreach (Card card in mCards)
            {
                if (card.MeldType == cardType)
                {
                    count++;
                }
            }
        }
        return count;
    }

    public virtual List<Card> GetCards() //An interface to get the card list
    {
        return mCards;
    }

    public virtual void LeftShiftElement()  //Used to shift all the elements to fill the gap/empty element
    {                                           //Before: Card1,Card2,Null,Card4
        List<Card> newList = new List<Card>();   //After: Card1,Card2,Card4

        foreach(Card card in mCards)
        {
            if (card != null)
            {
                newList.Add(card);
            }
        }
        mCards = newList;
    }

    public virtual int Count { get { return mCards.Count; } }

    public virtual List<Card> DeepClone()   //Clone every card from a list and create a new list to store them, not used
    {
        List<Card> newList = new List<Card>();

        foreach(Card card in mCards)
        {
            Card newCard = card.Clone();
            newList.Add(newCard);
        }
        return newList;
    }

    public virtual List<Card> ShallowClone()    //Create a new list and stores all the cards stored in the original list without creating clones of them
    {
        List<Card> newList = new List<Card>();

        foreach (Card card in mCards)
        {
            newList.Add(card);
            
        }
        return newList;
    }
}
