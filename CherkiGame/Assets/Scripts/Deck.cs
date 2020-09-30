using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;
public class Deck : ICards  //A deck means a pile, used for the draw pile and discard pile
{
    private System.Random rng;

    public Deck()
    {
        rng = new System.Random();
        mCards = new List<Card>();
    }

    public Deck(List<Card> cloneList)  
    {
        mCards = cloneList;
    }
    public  void Initialise()    //The function is used for draw deck, to instantiate all the cards needed
    {
        List<Card> allCards = new List<Card>();

        for (int x = 0; x < 6; x++)
        {
            if (x < 3)
            {
                
            allCards.Add(new Card((Suit)x, 1,(MeldType)x));
            allCards.Add(new Card((Suit)x, 1, (MeldType)x));
            allCards.Add(new Card((Suit)x, 1, (MeldType)x));
            allCards.Add(new Card((Suit)x, 1, (MeldType)x));
                
            }
            else
            {
                for (int y = 0; y < 9; y++)
                {
                    allCards.Add(new Card((Suit)x, y+1, (MeldType)(y+3)));
                    allCards.Add(new Card((Suit)x, y+1, (MeldType)(y + 3)));
                    allCards.Add(new Card((Suit)x, y+1, (MeldType)(y + 3)));
                    allCards.Add(new Card((Suit)x, y+1, (MeldType)(y + 3)));
                }
            }
        }

        mCards = allCards;
    }

    public virtual Card Pop()  //get and remove the top card
    {
        Card temp = mCards.Last();
        mCards.Remove(mCards.Last());
        
        return temp;
    }
    public Card GetTop()   //peek but not remove
    {
        if (mCards.Count > 0)
        {
            return mCards.Last();
        }
        return null;
    }

    public void Shuffle()   //To randomize the deck
    {
        int n = mCards.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            Card value = mCards[k];
            mCards[k] = mCards[n];
            mCards[n] = value;
        }
    }
}
