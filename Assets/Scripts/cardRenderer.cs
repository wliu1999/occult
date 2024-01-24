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

    // Holder for card image
    public GameObject cardImage;

    private void Awake()
    {
        cardImage = this.gameObject.transform.GetChild(0).gameObject;
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

    }

    public void renderCard(string titleString, string descriptionString, string left, string right)
    {
        title.SetText(titleString);
        description.SetText(descriptionString);
        rightCard = right;
        leftCard = left;
    }

    //public void updateImagePosition(Vector2 position, Vector3 rotation, float cardSpeed)
    //{
    //    cardImage.transform.position = Vector2.MoveTowards(cardImage.transform.position, position, cardSpeed);
    //    cardImage.transform.rotation = Quaternion.Euler(rotation);
    //}

    public void updateLeftCard(string newCard)
    {
        leftCard = newCard;
    }

    public void updateRightCard(string newCard)
    {
        rightCard = newCard;
    }
}
