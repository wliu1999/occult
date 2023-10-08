using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cardControllerC : cardController
{
    public override GameObject SwipeRight()
    {
        Debug.Log("Card C swiped right");
        return null;
    }

    public override GameObject SwipeLeft()
    {
        Debug.Log("Card C swiped left");
        return null;
    }
}
