using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class cardRenderer : MonoBehaviour
{
    //Title and Description
    public TMP_Text title;
    public TMP_Text description;
    public string leftCard = "0";
    public string rightCard = "0";

    // Utility booleans
    public BoxCollider2D thisCard;
    public bool isMouseOver;
    public bool isBigger = false;

    private void Start()
    {
        thisCard = gameObject.GetComponent<BoxCollider2D>();
    }

    private void OnMouseOver()
    {
        isMouseOver = true;
    }

    private void OnMouseExit()
    {
        isMouseOver = false;
    }

    public int SwipeRight()
    {
        if (Int32.TryParse(rightCard, out int i)){
            return i;
        } else 
        {
            Debug.Log("Could not parse int");
            return 0;
        }
    }

    public int SwipeLeft()
    {
        if (Int32.TryParse(leftCard, out int i)){
            return i;
        } else 
        {
            Debug.Log("Could not parse int");
            return 0;
        }
    }

    // Make the card bigger when the player clicks on it.
    void Update()
    {
        if (Input.GetMouseButton(0) && isMouseOver && !isBigger)
        {
            this.transform.localScale += new Vector3(0.2f, 0.2f, 0.2f);
            isBigger = true;
        }
        
        if (isBigger && (!Input.GetMouseButton(0) || !isMouseOver))
        {
            this.transform.localScale -= new Vector3(0.2f, 0.2f, 0.2f);
            isBigger = false;
        }
    }

    public void renderCard(string titleString, string descriptionString, string left, string right)
    {
        title.SetText(titleString);
        description.SetText(descriptionString);
        rightCard = right;
        leftCard = left;
    }
}
