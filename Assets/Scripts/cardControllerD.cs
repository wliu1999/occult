using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cardControllerD : cardController
{
    public override GameObject SwipeRight()
    {
        Debug.Log("Card D swiped right");
        return null;
    }

    public override GameObject SwipeLeft()
    {
        Debug.Log("Card D swiped left");
        return null;
    }
}
