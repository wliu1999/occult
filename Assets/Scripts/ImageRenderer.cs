using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageRenderer : MonoBehaviour
{
    // Utility Variables
    public bool isMouseOver;
    public bool isBigger = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && isMouseOver && !isBigger)
        {
            this.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
            isBigger = true;
        }

        if (isBigger && (!Input.GetMouseButton(0) || !isMouseOver))
        {
            this.transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f);
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
}
