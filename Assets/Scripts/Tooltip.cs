using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tooltip : MonoBehaviour
{
    private static Tooltip instance;
    [SerializeField]
    private Camera uiCamera;

    public TMP_Text tooltipText;
    private RectTransform backgroundRectTransform;
    private RectTransform textRectTransform;

    private void Awake()
    {
        instance = this;
        backgroundRectTransform = transform.Find("Background").GetComponent<RectTransform>();
        textRectTransform = tooltipText.GetComponent<RectTransform>();
        showTooltip("test");
        gameObject.SetActive(false);
    }

    public void showTooltip(string tooltipString)
    {
        tooltipText.text = tooltipString;
        //float textPaddingSize = 10f;
        //Vector2 backgroundSize = new Vector2(tooltipText.preferredWidth + textPaddingSize, tooltipText.preferredHeight + textPaddingSize);
        //backgroundRectTransform.sizeDelta = backgroundSize;
        //textRectTransform.sizeDelta = backgroundSize;
        gameObject.SetActive(true);
    }

    public void hideTooltip()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), Input.mousePosition, uiCamera, out localPoint);
        transform.localPosition = localPoint;       
    }
}
