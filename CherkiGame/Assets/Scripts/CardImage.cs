using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class CardImage : MonoBehaviour
{
    public TextMeshProUGUI cardValue;
    public TextMeshProUGUI cardSuit;

    public Image img;

    Sprite Coin1, Coin2, Coin3, Coin4, Coin5, Coin6, Coin7, Coin8, Coin9;
    Sprite Myriad1, Myriad2, Myriad3, Myriad4, Myriad5, Myriad6, Myriad7, Myriad8, Myriad9;
    Sprite String1, String2, String3, String4, String5, String6, String7, String8, String9;
    Sprite OldThousand, RedFlower, WhiteFlower;

    string value;

    // Start is called before the first frame update
    void Start()
    {
        CoinReferences();
        MyriadReferences();
        StringReferences();
        HonourReferences();
    }

    // Update is called once per frame
    void Update()
    {
        CoinCards();
        MyriadCards();
        StringCards();
        HonourCards();
    }

    void CoinCards()
    {
        //Check with the Suit and Value of the cards
        if (cardValue.text == "1" && cardSuit.text == "Coin")
        {
            img.sprite = Coin1;
            //Debug.Log("HI-1");
        }
        if (cardValue.text == "2" && cardSuit.text == "Coin")
        {
            img.sprite = Coin2;
            //Debug.Log("HI-2");
        }
        if (cardValue.text == "3" && cardSuit.text == "Coin")
        {
            img.sprite = Coin3;
            //Debug.Log("HI-3");
        }
        if (cardValue.text == "4" && cardSuit.text == "Coin")
        {
            img.sprite = Coin4;
           // Debug.Log("HI-4");
        }
        if (cardValue.text == "5" && cardSuit.text == "Coin")
        {
            img.sprite = Coin5;
            //Debug.Log("HI-5");
        }
        if (cardValue.text == "6" && cardSuit.text == "Coin")
        {
            img.sprite = Coin6;
            //Debug.Log("HI-6");
        }
        if (cardValue.text == "7" && cardSuit.text == "Coin")
        {
            img.sprite = Coin7;
           // Debug.Log("HI-7");
        }
        if (cardValue.text == "8" && cardSuit.text == "Coin")
        {
            img.sprite = Coin8;
            //Debug.Log("HI-8");
        }
        if (cardValue.text == "9" && cardSuit.text == "Coin")
        {
            img.sprite = Coin9;
            //Debug.Log("HI-9");
        }
    }

    void MyriadCards()
    {
        //Check with the Suit and Value of the cards
        if (cardValue.text == "1" && cardSuit.text == "Myriad")
        {
            img.sprite = Myriad1;
        }
        if (cardValue.text == "2" && cardSuit.text == "Myriad")
        {
            img.sprite = Myriad2;
        }
        if (cardValue.text == "3" && cardSuit.text == "Myriad")
        {
            img.sprite = Myriad3;
        }
        if (cardValue.text == "4" && cardSuit.text == "Myriad")
        {
            img.sprite = Myriad4;
        }
        if (cardValue.text == "5" && cardSuit.text == "Myriad")
        {
            img.sprite = Myriad5;
        }
        if (cardValue.text == "6" && cardSuit.text == "Myriad")
        {
            img.sprite = Myriad6;
        }
        if (cardValue.text == "7" && cardSuit.text == "Myriad")
        {
            img.sprite = Myriad7;
        }
        if (cardValue.text == "8" && cardSuit.text == "Myriad")
        {
            img.sprite = Myriad8;
        }
        if (cardValue.text == "9" && cardSuit.text == "Myriad")
        {
            img.sprite = Myriad9;
        }
    }

    void StringCards()
    {
        //Check with the Suit and Value of the cards
        if (cardValue.text == "1" && cardSuit.text == "String")
        {
            img.sprite = String1;
        }
        if (cardValue.text == "2" && cardSuit.text == "String")
        {
            img.sprite = String2;
        }
        if (cardValue.text == "3" && cardSuit.text == "String")
        {
            img.sprite = String3;
        }
        if (cardValue.text == "4" && cardSuit.text == "String")
        {
            img.sprite = String4;
        }
        if (cardValue.text == "5" && cardSuit.text == "String")
        {
            img.sprite = String5;
        }
        if (cardValue.text == "6" && cardSuit.text == "String")
        {
            img.sprite = String6;
        }
        if (cardValue.text == "7" && cardSuit.text == "String")
        {
            img.sprite = String7;
        }
        if (cardValue.text == "8" && cardSuit.text == "String")
        {
            img.sprite = String8;
        }
        if (cardValue.text == "9" && cardSuit.text == "String")
        {
            img.sprite = String9;
        }
    }

    void HonourCards()
    {
        //Check with the Suit and Value of the cards
        if (cardValue.text == "1" && cardSuit.text == "OldThousand")
        {
            img.sprite = OldThousand;
        }
        //Check with the Suit and Value of the cards
        if (cardValue.text == "1" && cardSuit.text == "RedFlower")
        {
            img.sprite = RedFlower;
        }
        //Check with the Suit and Value of the cards
        if (cardValue.text == "1" && cardSuit.text == "WhiteFlower")
        {
            img.sprite = WhiteFlower;
        }
    }

    void CoinReferences()
    {
        //Make a Reference for the gmae to load the images
        Coin1 = Resources.Load<Sprite>("MyAssets/Cards/Coin/Coin 1") as Sprite;
        Coin2 = Resources.Load<Sprite>("MyAssets/Cards/Coin/Coin 2") as Sprite;
        Coin3 = Resources.Load<Sprite>("MyAssets/Cards/Coin/Coin 3") as Sprite;
        Coin4 = Resources.Load<Sprite>("MyAssets/Cards/Coin/Coin 4") as Sprite;
        Coin5 = Resources.Load<Sprite>("MyAssets/Cards/Coin/Coin 5") as Sprite;
        Coin6 = Resources.Load<Sprite>("MyAssets/Cards/Coin/Coin 6") as Sprite;
        Coin7 = Resources.Load<Sprite>("MyAssets/Cards/Coin/Coin 7") as Sprite;
        Coin8 = Resources.Load<Sprite>("MyAssets/Cards/Coin/Coin 8") as Sprite;
        Coin9 = Resources.Load<Sprite>("MyAssets/Cards/Coin/Coin 9") as Sprite;
    }

    void MyriadReferences()
    {
        Myriad1 = Resources.Load<Sprite>("MyAssets/Cards/Myriad/Myriad 1") as Sprite;
        Myriad2 = Resources.Load<Sprite>("MyAssets/Cards/Myriad/Myriad 2") as Sprite;
        Myriad3 = Resources.Load<Sprite>("MyAssets/Cards/Myriad/Myriad 3") as Sprite;
        Myriad4 = Resources.Load<Sprite>("MyAssets/Cards/Myriad/Myriad 4") as Sprite;
        Myriad5 = Resources.Load<Sprite>("MyAssets/Cards/Myriad/Myriad 5") as Sprite;
        Myriad6 = Resources.Load<Sprite>("MyAssets/Cards/Myriad/Myriad 6") as Sprite;
        Myriad7 = Resources.Load<Sprite>("MyAssets/Cards/Myriad/Myriad 7") as Sprite;
        Myriad8 = Resources.Load<Sprite>("MyAssets/Cards/Myriad/Myriad 8") as Sprite;
        Myriad9 = Resources.Load<Sprite>("MyAssets/Cards/Myriad/Myriad 9") as Sprite;
    }

    void StringReferences()
    {
        String1 = Resources.Load<Sprite>("MyAssets/Cards/String/String 1") as Sprite;
        String2 = Resources.Load<Sprite>("MyAssets/Cards/String/String 2") as Sprite;
        String3 = Resources.Load<Sprite>("MyAssets/Cards/String/String 3") as Sprite;
        String4 = Resources.Load<Sprite>("MyAssets/Cards/String/String 4") as Sprite;
        String5 = Resources.Load<Sprite>("MyAssets/Cards/String/String 5") as Sprite;
        String6 = Resources.Load<Sprite>("MyAssets/Cards/String/String 6") as Sprite;
        String7 = Resources.Load<Sprite>("MyAssets/Cards/String/String 7") as Sprite;
        String8 = Resources.Load<Sprite>("MyAssets/Cards/String/String 8") as Sprite;
        String9 = Resources.Load<Sprite>("MyAssets/Cards/String/String 9") as Sprite;
    }

    void HonourReferences()
    {
        OldThousand = Resources.Load<Sprite>("MyAssets/Cards/Honour/Old Thousand") as Sprite;
        RedFlower = Resources.Load<Sprite>("MyAssets/Cards/Honour/Red Flower") as Sprite;
        WhiteFlower = Resources.Load<Sprite>("MyAssets/Cards/Honour/White Flower") as Sprite;
    }
}
