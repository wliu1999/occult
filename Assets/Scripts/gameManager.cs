using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System;

public class gameManager : MonoBehaviour
{
    // Utility variables
    public GameObject card = null;
    public GameObject cardImage = null;
    float cardSpeed = 3f;
    public GameObject tempCard;
    public GameObject tempImage;
    int nextCard;
    public bool isPaused = false;

    // Card physics management variables
    // NOTE: update these in editor, not in script.
    Vector2 position;
    Vector3 rotation;
    public float maxPosition = 1;
    public float minPosition = -1;
    public float maxRotation = 5;
    public float minRotation = -5;

    // Card management variables
    public GameObject cardPrefab;
    public GameObject cardImagePrefab;
    public GameObject spawnArea;
    string[,] cardDatabase;
    int cardID = 0;

    // Need to convert the TSV into bytes format as that's the only way to register it as a unity object.
    // This is necessary for building, as reading directly from a file isn't possible post build.
    public TextAsset cardTSV;

    // Make sure this number matches the number of data columns per card.
    int cardDataColumns = 20;

    // Hover text variables
    public TMP_Text leftHoverText;
    public TMP_Text rightHoverText;

    // Vision Sheet
    public GameObject visionSheet;
    public GameObject darkenEffect;

    // KPI Variables
    int kpi1 = 0, kpi2 = 0, kpi3 = 0, kpi4 = 0, kpi5 = 0, kpiMax = 10;
    public Image notoriety;
    public Image money;
    public Image weaponry;
    public Image zeal;

    // Path chosen
    string path = "none";

    // Store script as variable
    public ImageRenderer imageRenderer;

    // Start is called before the first frame update
    void Start()
    {
        //darkenEffect.SetActive(false);

        // Set position and rotation for new card
        position = spawnArea.transform.position;
        rotation = new Vector3(0, 0, 0);

        updateCardDatabase();

        card = resetCard();
        cardImage = spawnCardImage();

        //Set KPI default values
        updateKPI("5", "5", "5", "5", "5");
    }

    // Update is called once per frame
    void Update()
    {
        // only track mouse horizontal position
        // make card's position and rotation depend on horizontal position
        if (!isPaused)
        {
            if (Input.GetMouseButton(0) && imageRenderer.isMouseOver)
            {
                // Set position based on mouse position, and rotation based on position
                // Note: position is currently based on the user's mouse position
                // To make things visually make more sense, maybe change some of the below appareance functions to change based on the card's position instead
                position.x = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
                rotation.z = -position.x * 5;

                // Clamp the position of the card
                //position.x = Mathf.Clamp(position.x, minPosition, maxPosition);
                rotation.z = Mathf.Clamp(rotation.z, minRotation, maxRotation);

                // Visually move the card to the set position and rotation
                updateCardPosition(position, rotation);

                // If the user is moving the card to the right, gradually phase in the right hover text.
                if (position.x < 0 && position.x > minPosition)
                {
                    leftHoverText.faceColor = new Color(1, 1, 1, position.x / minPosition);
                    rightHoverText.faceColor = new Color(0, 0, 0, 0);
                }

                // If the user is moving the card to the left, gradually phase in the left hover text.
                if (position.x > 0 && position.x < maxPosition)
                {
                    rightHoverText.faceColor = new Color(1, 1, 1, position.x / maxPosition);
                    leftHoverText.faceColor = new Color(0, 0, 0, 0);
                }

                // These should change the color when the user is hovering the card past the threshold to swipe. 
                // For some reason, they're not showing the right color for now. Will have to bugfix later.
                if (position.x <= -1)
                {
                    leftHoverText.faceColor = new Color(1, 1, 1, 1);
                }

                if (position.x >= 1)
                {
                    rightHoverText.faceColor = new Color(1, 1, 1, 1);
                }


            }
            else
            {
                // When the user releases the mouse button, run the swipe right or left commands on the card if the position is far enough
                if (Input.GetMouseButtonUp(0) && imageRenderer.isMouseOver)
                {
                    // Swipe Left
                    if (position.x <= minPosition)
                    {
                        // Update the KPIs and perform any special operations based on the current card.
                        updateKPI(cardDatabase[cardID, 7], cardDatabase[cardID, 8], cardDatabase[cardID, 9], cardDatabase[cardID, 10], cardDatabase[cardID, 11]);
                        if (cardDatabase[cardID, 17] != "None")
                        {
                            processSpecial(cardDatabase[cardID, 17]);
                        }

                        //Spawn the next card
                        nextCard = card.GetComponent<cardRenderer>().SwipeLeft();
                        tempCard = spawnCard(nextCard);
                        Destroy(card);
                        card = tempCard;
                        tempImage = spawnCardImage();
                        Destroy(cardImage);
                        cardImage = tempImage;
                        nextCard = 0;

                    }
                    // Swipe Right
                    else if (position.x >= maxPosition)
                    {
                        // Update the KPIs and perform any special operations based on the current card.
                        updateKPI(cardDatabase[cardID, 12], cardDatabase[cardID, 13], cardDatabase[cardID, 14], cardDatabase[cardID, 15], cardDatabase[cardID, 16]);
                        if (cardDatabase[cardID, 18] != "None")
                        {
                            processSpecial(cardDatabase[cardID, 18]);
                        }

                        //Spawn the next card
                        nextCard = card.GetComponent<cardRenderer>().SwipeRight();
                        tempCard = spawnCard(nextCard);
                        Destroy(card);
                        card = tempCard;
                        tempImage = spawnCardImage();
                        Destroy(cardImage);
                        cardImage = tempImage;
                        nextCard = 0;
                    }
                } else
                {
                    updateCardPosition(spawnArea.transform.position, new Vector3(0, 0, 0));
                    leftHoverText.faceColor = new Color(0, 0, 0, 0);
                    rightHoverText.faceColor = new Color(0, 0, 0, 0);
                }
            }
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                unpauseGame();
            }
        }
    }

    public void updateCardDatabase()
    {
        // Initialize card database to read. Now reads from the direct byte data of a TSV!
        // Note that we're reading from TSV, not CSV. This is because I was too lazy to find a workaround for all the commas in the scripts, 
        // and we should never have tab values in cards anyways. Therefore, TSV superior hahaha.

        byte[] cardTSVBytes = cardTSV.bytes;

        // Decode from bytes into one big string.
        string cardTSVBigString = System.Text.Encoding.UTF8.GetString(cardTSVBytes, 0, cardTSVBytes.Length);

        // Split the string based on tabs and newlines (since each line is separated by a newline, and each value by a tab)
        string[] splitCards = cardTSVBigString.Split(new char[] { '\t', '\n' });

        // Set our database size equal to the length of our resulting array divided by the number of rows of data in each card
        // Offset it by 1 to account for the header
        int databaseSize = splitCards.Length / cardDataColumns - 1;

        // Set the size of cardDatabase 2d array based on above calculations.
        cardDatabase = new string[databaseSize, cardDataColumns];

        // Grab the (i+1) value of the splitCards array to account for the header.
        // Populate the cardDatabase array.
        for (int i = 0; i < databaseSize; i++)
        {
            for (int j = 0; j < cardDataColumns; j++)
            {
                cardDatabase[i, j] = splitCards[((i + 1) * cardDataColumns) + j];
            }
        }
    }

    // Method to spawn a new card based on the ID given.
    public GameObject spawnCard(int ID){
        if(ID >= (cardDatabase.Length/cardDataColumns))
        {
            ID = 0;
        }
		GameObject newCard = Instantiate(cardPrefab, spawnArea.transform);
        newCard.transform.position += new Vector3(0, 0, -1);
        cardRenderer script = newCard.GetComponent<cardRenderer>();
        script.renderCard(cardDatabase[ID,1], cardDatabase[ID,2], cardDatabase[ID,3], cardDatabase[ID,4]);
        leftHoverText.text = cardDatabase[ID,5];
        leftHoverText.faceColor = new Color(0,0,0,0);
        rightHoverText.text = cardDatabase[ID,6];
        rightHoverText.faceColor = new Color(0,0,0,0);
        cardID = ID;
        return newCard;
	}

    public GameObject spawnCardImage()
    {
        GameObject newCardImage = Instantiate(cardImagePrefab, spawnArea.transform);
        imageRenderer = newCardImage.GetComponent<ImageRenderer>();
        imageRenderer.updateImage(cardDatabase[cardID, 19]);
        return newCardImage;
    }

    public GameObject resetCard()
    {
        GameObject newCard = Instantiate(cardPrefab, spawnArea.transform);
        newCard.transform.position += new Vector3(0, 0, -1);
        cardRenderer script = newCard.GetComponent<cardRenderer>();
        script.renderCard(cardDatabase[1, 1], cardDatabase[1, 2], cardDatabase[1, 3], cardDatabase[1, 4]);
        leftHoverText.text = cardDatabase[1, 5];
        leftHoverText.faceColor = new Color(0, 0, 0, 0);
        rightHoverText.text = cardDatabase[1, 6];
        rightHoverText.faceColor = new Color(0, 0, 0, 0);
        cardID = 1;
        return newCard;
    }

    public void pauseGame()
    {
        isPaused = true;
        darkenEffect.SetActive(true);
        visionSheet.transform.position = new Vector2(0, 0);
    }

    public void unpauseGame()
    {
        isPaused = false;
        darkenEffect.SetActive(false);
        visionSheet.transform.position = new Vector2(0, 600);
    }

    public void updateCardPosition(Vector2 position, Vector3 rotation)
    {
        imageRenderer.updateImagePosition(position,rotation, cardSpeed);
    }

    public void updateKPI(string one, string two, string three, string four, string five)
    {
        float fillAmount;
        // Parse first string into int
        if (Int32.TryParse(one, out int i))
        {
            kpi1 = kpi1 + i;
        }
        else
        {
            Debug.Log("Could not parse one");
        }
        // Parse second string into int
        if (Int32.TryParse(two, out int j))
        {
            kpi2 = kpi2 + j;
        }
        else
        {
            Debug.Log("Could not parse two");
        }

        if (Int32.TryParse(three, out int k))
        {
            kpi3 = kpi3 + k;
        }
        else
        {
            Debug.Log("Could not parse three");
        }

        if (Int32.TryParse(four, out int l))
        {
            kpi4 = kpi4 + l;
        }
        else
        {
            Debug.Log("Could not parse four");
        }

        if (Int32.TryParse(four, out int m))
        {
            kpi5 = kpi5 + m;
        }
        else
        {
            Debug.Log("Could not parse five");
        }

        // Update text to reflect actual KPI value;
        fillAmount = (float)kpi1 / (float)kpiMax;
        notoriety.fillAmount = fillAmount;

        fillAmount = (float)kpi2 / (float)kpiMax;
        money.fillAmount = fillAmount;

        fillAmount = (float)kpi3 / (float)kpiMax;
        weaponry.fillAmount = fillAmount;

        fillAmount = (float)kpi4 / (float)kpiMax;
        zeal.fillAmount = fillAmount;
    }

    public void processSpecial(string special)
    {
        special = special.Trim();
        switch (special)
        {
            case "None":
                break;
            case "Mediocrity": 
            case "Ambition":
                path = special;
                break;
            case "Day 1":
                cardTSV = Resources.Load("Cards/Day 1") as TextAsset;
                Destroy(card);
                updateCardDatabase();
                card = resetCard();
                darkenEffect.SetActive(false);
                break;
            case "Check path":
                if (path == "Mediocrity")
                {
                    card.GetComponent<cardRenderer>().updateRightCard(cardDatabase[cardID, 3]);
                } else if (path == "Ambition")
                {
                    card.GetComponent<cardRenderer>().updateLeftCard(cardDatabase[cardID, 4]);
                }
                break;
            case "Day 1 Scenarios":
                cardTSV = Resources.Load("Cards/Day 1 Scenarios") as TextAsset;
                Destroy(card);
                updateCardDatabase();
                card = resetCard();
                break;
            default:
                Debug.Log("Current special tag: " + special);
                break;
        }
    }
}
