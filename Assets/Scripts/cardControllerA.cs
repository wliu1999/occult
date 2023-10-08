using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cardControllerA : cardController
{
    public override GameObject SwipeRight()
    {
        Debug.Log("Card A swiped right");
        return rightCard;
    }

    public override GameObject SwipeLeft()
    {
        Debug.Log("Card A swiped left");
        return leftCard;
    }
}
