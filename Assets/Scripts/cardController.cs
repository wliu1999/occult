using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cardController : MonoBehaviour
{
    public BoxCollider2D thisCard;
    public bool isMouseOver;
    public GameObject rightCard;
    public GameObject leftCard;

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

    public virtual GameObject SwipeRight()
    {
        Debug.Log("Swiped Right");
        return null;
    }

    public virtual GameObject SwipeLeft()
    {
        Debug.Log("Swiped Left");
        return null;
    }

    // Make the card bigger when the player clicks on it.
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            this.transform.localScale += new Vector3(5f, 5f, 5f);
            Debug.Log("Make big");
        }
    }
}
