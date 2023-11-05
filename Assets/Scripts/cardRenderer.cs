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
    string leftCard = "0";
    string rightCard = "0";

    // Utility booleans
    public BoxCollider2D thisCard;
    public bool isMouseOver;

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
        Debug.Log("Swiped Right");
        if (Int32.TryParse(rightCard, out int i)){
            Debug.Log(i);
            return i;
        } else 
        {
            Debug.Log("Could not parse int");
            return 0;
        }
    }

    public int SwipeLeft()
    {
        Debug.Log("Swiped Left");
        if (Int32.TryParse(leftCard, out int i)){
            Debug.Log(i);
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
        if (Input.GetMouseButtonDown(0))
        {
            this.transform.localScale += new Vector3(0.2f, 0.2f, 0.2f);
            Debug.Log("Make big");
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
