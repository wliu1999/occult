using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ImageRenderer : MonoBehaviour
{
    // Utility Variables
    public bool isMouseOver;
    public bool isBigger = false;
    public SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && isMouseOver && !isBigger)
        {
            this.transform.localScale += new Vector3(0.05f, 0.05f, 0.05f);
            isBigger = true;
        }

        if (isBigger && (!Input.GetMouseButton(0) || !isMouseOver))
        {
            this.transform.localScale -= new Vector3(0.05f, 0.05f, 0.05f);
            isBigger = false;
        }
    }

    private void OnMouseEnter()
    {
        isMouseOver = true;
    }

    private void OnMouseExit()
    {
        isMouseOver = false;
    }

    public void updateImagePosition(Vector2 position, Vector3 rotation, float cardSpeed)
    {
        this.transform.position = Vector2.MoveTowards(this.transform.position, position, cardSpeed);
        this.transform.rotation = Quaternion.Euler(rotation);
    }

    public void updateImage(string spritePath)
    {
        spritePath = spritePath.Trim();
        if (String.Equals(spritePath, "None"))
        {
            spritePath = "Card Art/Letter";
        }
        Texture2D newTexture = Resources.Load(spritePath) as Texture2D;
        Sprite newSprite = Sprite.Create(newTexture, new Rect(0, 0, newTexture.width, newTexture.height), new Vector2(0.5f, 0.5f), (float)(Math.Max(newTexture.height/20.48, newTexture.width/20.48)));
        spriteRenderer.sprite = newSprite;
    }
}
