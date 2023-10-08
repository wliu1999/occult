using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cardControllerB : cardController
{
    public override GameObject SwipeRight()
    {
        Debug.Log("Card B swiped right");
        return null;
    }

    public override GameObject SwipeLeft()
    {
        Debug.Log("Card B swiped left");
        return null;
    }
}
