using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Card //class holds the important variables and values of a card, is called onto by other scripts
{
    int value = 0;
    Suit suit;               //Suit can be string, coin, white flower or ect
    MeldType meldType;       //Regardless of its suit, as long as the cards can form a meld, they belong to the same meld type

    public Card(Suit mSuit, int mValue, MeldType mMeldType) //sets value for variables
    {
        this.value = mValue;
        this.suit = mSuit;
        this.meldType = mMeldType;
    }

    public int Value { get { return value; } }
    public Suit Suit { get { return suit; } }
    public MeldType MeldType { get { return meldType; } }
    //public int ID{get{ return cardID; } }

    public Card Clone()          //A deepClone function, not used
    {
        Card cloneCard = (Card)this.MemberwiseClone();
        cloneCard.value = this.value;
        cloneCard.suit = this.suit;
        cloneCard.meldType = this.meldType;
        return cloneCard;
    }
}

public enum MeldType  //The way they are arranged is based on their value
{
    RedFlower,
    WhiteFlower,
    OldThousand,
    One,
    Two,
    Three,
    Four,
    Five,
    Six,
    Seven,
    Eight,
    Nine,
    Count 
}

public enum Suit //Enum to arrange cards and give them their suits
{
    RedFlower,
    WhiteFlower,
    OldThousand,
    Coin,
    String,
    Myriad
}