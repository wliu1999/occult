using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class gameManager : MonoBehaviour
{
    // Utility variables
    public GameObject card;
    public float fMovingSpeed;
    public GameObject cardA;
    public GameObject tempCard;
    int nextCard;

    // Card physics management variables
    Vector2 position;
    Vector3 rotation;
    public float maxPosition = 3;
    public float minPosition = -3;
    public float maxRotation = 30;
    public float minRotation = -30;

    // Card management variables
    public GameObject cardPrefab;
    string[][] cardDatabase;

    // Hover text variables
    public TMP_Text leftHoverText;
    public TMP_Text rightHoverText;

    // Start is called before the first frame update
    void Start()
    {
        // Set position and rotation for new card
        position = new Vector2(0, 0);
        rotation = new Vector3(0, 0, 0);

        // Initialize card database to read. Now reads from TSV!
        // Note that we're reading from TSV, not CSV. This is because I was too lazy to find a workaround for all the commas in the scripts, 
        // and we should never have tab values in cards anyways. Therefore, TSV superior hahaha.
        var filePath = "Assets/Cards/Test Scenario.tsv";
        string[] lines = File.ReadAllLines(filePath);
        cardDatabase = new string[lines.Length][];

        // This loop reads the TSV and translates each line into an array that represents a card.
        // Line 53 has the i value offset by 1 to ignore the header.
        for (int i = 1; i < lines.Length; i++)
        {
            string[] newCard = lines[i].Split('\t');
            cardDatabase[i-1] = newCard;
        }

        card = spawnCard(1);
    }

    // Update is called once per frame
    void Update()
    {
        // only track mouse horizontal position
        // make card's position and rotation depend on horizontal position

        if (Input.GetMouseButton(0))
        //&& card.GetComponent<cardRenderer>().isMouseOver
        {
            // Set position based on mouse position, and rotation based on position
            // Note: position is currently based on the user's mouse position
            // To make things visually make more sense, maybe change some of the below appareance functions to change based on the card's position instead
            position.x = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
            rotation.z = -position.x * 15;

            // Clamp the position of the card
            position.x = Mathf.Clamp(position.x, minPosition, maxPosition);
            rotation.z = Mathf.Clamp(rotation.z, minRotation, maxRotation);

            // Visually move the card to the set position and rotation
            card.transform.position = Vector2.MoveTowards(card.transform.position, position, fMovingSpeed);
            card.transform.rotation = Quaternion.Euler(rotation);

            // If the user is moving the card to the right, gradually phase in the right hover text.
            if (position.x < 0 && position.x > -2)
            {
                leftHoverText.faceColor = new Color(0,0,0,-position.x/2);
            }

            // If the user is moving the card to the left, gradually phase in the left hover text.
            if (position.x > 0 && position.x < 2)
            {
                rightHoverText.faceColor = new Color(0,0,0,position.x/2);
            }

            // These should change the color when the user is hovering the card past the threshold to swipe. 
            // For some reason, they're not showing the right color for now. Will have to bugfix later.
            if (position.x <= -2)
            {
                leftHoverText.faceColor = new Color(1,1,1,1);
            }

            if (position.x >= 2)
            {
                rightHoverText.faceColor = new Color(1,1,1,1);
            }


        } else {
            // When the user releases the mouse button, run the swipe right or left commands on the card if the position is far enough
            if (Input.GetMouseButtonUp(0))
            {
                if (position.x >= 2)
                {
                    nextCard = card.GetComponent<cardRenderer>().SwipeRight();
                    tempCard = spawnCard(nextCard);
                    Destroy(card);
                    card = tempCard;
                    nextCard = 0;
                }
                else if (position.x <= -2)
                {
                    nextCard = card.GetComponent<cardRenderer>().SwipeLeft();
                    tempCard = spawnCard(nextCard);
                    Destroy(card);
                    card = tempCard;
                    nextCard = 0;
                }
            }
            card.transform.position = Vector2.MoveTowards(card.transform.position, new Vector2(0, 0), fMovingSpeed);
            card.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            leftHoverText.faceColor = new Color(0,0,0,0);
            rightHoverText.faceColor = new Color(0,0,0,0);
        }
    }

    // Method to spawn a new card based on the ID given.
    public GameObject spawnCard(int ID){
		GameObject newCard = Instantiate(cardPrefab, GameObject.FindGameObjectWithTag("Canvas").transform);
        cardRenderer script = newCard.GetComponent<cardRenderer>();
        script.renderCard(cardDatabase[ID][1], cardDatabase[ID][2], cardDatabase[ID][3], cardDatabase[ID][4]);
        leftHoverText.text = cardDatabase[ID][5];
        leftHoverText.faceColor = new Color(0,0,0,0);
        rightHoverText.text = cardDatabase[ID][6];
        rightHoverText.faceColor = new Color(0,0,0,0);
		return newCard;
	}
}
