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
    public float maxPosition = 2;
    public float minPosition = -2;
    public float maxRotation = 30;
    public float minRotation = -30;

    public GameObject cardPrefab;
    string[][] cardDatabase;

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
        string result = string.Join(", ", cardDatabase[1]);

        card = spawnCard(1);
        Debug.Log(result);
    }

    // Update is called once per frame
    void Update()
    {
        // only track mouse horizontal position
        // make card's position and rotation depend on horizontal position

        if (Input.GetMouseButton(0) && card.GetComponent<cardRenderer>().isMouseOver)
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
        }
    }

    // Method to spawn a new card based on the ID given.
    public GameObject spawnCard(int ID){
		GameObject newCard = Instantiate(cardPrefab, GameObject.FindGameObjectWithTag("Canvas").transform);
        cardRenderer script = newCard.GetComponent<cardRenderer>();
        script.renderCard(cardDatabase[ID][1], cardDatabase[ID][2], cardDatabase[ID][3], cardDatabase[ID][4]);
		return newCard;
	}
}
