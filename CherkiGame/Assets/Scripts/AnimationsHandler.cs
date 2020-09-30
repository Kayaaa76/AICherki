using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnimationsHandler : MonoBehaviour  //Class is in charge on handling the animations in the game.
{
    public Button drawButton;
    public GameObject card9;
    public Animator card9Anim;
    public Animator DeckAnimator;
    public Animator BubbleSpeech;
    public Animator TurnBorder;
    public TextMeshProUGUI actionText;
    public TextMeshProUGUI stateText;
    public TextMeshProUGUI Turntext;

    private void Start()
    {
        Button btn = drawButton.GetComponent<Button>();
        btn.onClick.AddListener(Card9DrawAnim);
    }

    void Update()
    {
        if (stateText.text == "Computer Turn State" && actionText.text == "Discard")
        {
            StartCoroutine(BubbleSpeechText());
        }
    }

    public void Drawcard() //Starts the animation for drawing card
    {
        //Peacock DrawAnimation
        if(actionText.text == "Draw")
        {
            DeckAnimator.SetTrigger("Draw");
        }
    }

    public void Card9DrawAnim()
    {
        card9Anim.enabled = true;
        StartCoroutine(Card9AnimFalse());
    }

    public void Turnchanged()
    {
        StartCoroutine(TurnChanged());
    }

    public IEnumerator BubbleSpeechText() //starts the animation for the speech bubble
    {
        yield return new WaitForSeconds(1);
        //BubbleSpeech.SetTrigger("AiMoved");
        BubbleSpeech.SetBool("AiMoved", true);//set the animation
        yield return new WaitForSeconds(2);
        BubbleSpeech.SetBool("AiMoved", false);//make it back to false so it can repeat
    }

    IEnumerator TurnChanged()
    {
        yield return new WaitForSeconds(1);

        if(actionText.text == "Draw" && stateText.text == "Player Turn State")
        {
            //card9Anim.enabled = true;
            TurnBorder.SetTrigger("TurnChanged");
            Turntext.text = "Your Turn, Draw!";
        }

        else if(actionText.text == "Draw" && stateText.text == "Computer Turn State")
        {
            TurnBorder.SetTrigger("TurnChanged");
            Turntext.text = "Computer's Turn";
        }
    }

    IEnumerator Card9AnimFalse()
    {
        yield return new WaitForSeconds(1f);
        card9Anim.enabled = false;
    }
}
