using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPanel : MonoBehaviour
{
    public GameObject ScorePanel;
    public GameObject DiscardPlaceholder;
    public GameObject DiscardTop;

    public void OpenPanel()
    {
        if (ScorePanel != null)
        {
            Animator anim = ScorePanel.GetComponent<Animator>();
            if (anim != null)
            {
                bool isOpen = anim.GetBool("OpenTab");
                anim.SetBool("OpenTab", !isOpen);
            }
        }
    }

    public void PlaceHolderResetDiscard()
    {
        DiscardTop.transform.position = DiscardPlaceholder.transform.position;
    }
}
