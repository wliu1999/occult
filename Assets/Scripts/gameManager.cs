using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class gameManager : MonoBehaviour
{
    // Utility variables
    public GameObject card;
    public float fMovingSpeed;
    public GameObject cardA;
    GameObject nextCard;

    // Card management variables
    Vector2 position;
    Vector3 rotation;
    public float maxPosition = 3;
    public float minPosition = -3;
    public float maxRotation = 30;
    public float minRotation = -30;

    // Card queue system
    

    // Start is called before the first frame update
    void Start()
    {
        position = new Vector2(0, 0);
        rotation = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        // only track mouse horizontal position
        // make card's position and rotation depend on horizontal position

        if (Input.GetMouseButton(0) && card.GetComponent<cardController>().isMouseOver)
        {
            // Set position based on mouse position, and rotation based on position
            position.x = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
            rotation.z = -position.x * 15;

            // Clamp the position of the card
            position.x = Mathf.Clamp(position.x, minPosition, maxPosition);
            rotation.z = Mathf.Clamp(rotation.z, minRotation, maxRotation);

            // Visually move the card to the set position and rotation
            card.transform.position = Vector2.MoveTowards(card.transform.position, position, fMovingSpeed);
            card.transform.rotation = Quaternion.Euler(rotation);
        } else {
            // When the user releases the mouse button, run the swipe right or left commands on the card if the position is far enough
            if (Input.GetMouseButtonUp(0))
            {
                if (position.x >= 2)
                {
                    nextCard = card.GetComponent<cardController>().SwipeRight();
                }
                else if (position.x <= -2)
                {
                    nextCard = card.GetComponent<cardController>().SwipeLeft();
                }

                if (nextCard != null)
                {
                    nextCard = Instantiate(nextCard, new Vector3(0, 0, 0), Quaternion.identity);
                    Destroy(card);
                    card = nextCard;
                    nextCard = null;
                } else
                {
                    nextCard = Instantiate(cardA, new Vector3(0, 0, 0), Quaternion.identity);
                    Destroy(card);
                    card = nextCard;
                    nextCard = null;
                }


            }
            card.transform.position = Vector2.MoveTowards(card.transform.position, new Vector2(0, 0), fMovingSpeed);
            card.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
    }
}
