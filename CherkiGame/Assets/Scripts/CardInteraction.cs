using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class CardInteraction : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Quaternion originRot;
    Vector3 originPos;
    Vector3 originalScale;

    float dropDist;
    float retrieveDist;

    public bool dragged;
    public static bool retrieved;

    public GameObject DiscardPlace;
    public GameObject RetrievablePlace;
    public GameObject RetrieveableCard;
    public GameObject playerCards;
    public GameObject MainScript;           //Reference to the Main Script
    public TextMeshProUGUI cardValue;
    public TextMeshProUGUI cardSuit;
    public Animator card9Anim;

    // Start is called before the first frame update
    void Start()
    {
        originPos = gameObject.transform.position;
        originRot = gameObject.transform.rotation;
        originalScale = gameObject.transform.localScale;

    }

    // Update is called once per frame
    void Update()
    {
        dropDist = Vector3.Distance(gameObject.transform.position, DiscardPlace.transform.position);            //Discard check placement
        retrieveDist = Vector3.Distance(gameObject.transform.position, RetrievablePlace.transform.position);    //Retrieve check placement

        if (Main.isComplete == true)
        {
            gameObject.GetComponent<CardImage>().enabled = true;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {

    }


    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
        transform.SetAsLastSibling();

        transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.localScale = new Vector3(0.54f, 1.5f, 0f);
    }

    public void OnEndDrag(PointerEventData eventData)
    {

        if (dropDist <= 30f)                 //Card is discarded when on the playmat
        {
            MainScript.GetComponent<Main>().HumanExecute();
            transform.position = new Vector3(originPos.x, originPos.y);
            transform.rotation = originRot;
            transform.localScale = originalScale;
        }

        else if (retrieveDist <= 80f)       //Card is retrieved from the playmat
        {
            MainScript.GetComponent<Main>().HumanExecute();
            transform.position = new Vector3(originPos.x, originPos.y);
            transform.rotation = originRot;
            transform.localScale = originalScale;
        }

        else                                //Card goes back to original position once it's let go and not discarded
        {
            transform.position = new Vector3(originPos.x, originPos.y);
            transform.rotation = originRot;
            transform.localScale = originalScale;
        }
    }
}
