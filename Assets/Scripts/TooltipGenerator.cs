using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject tooltipObject;
    [SerializeField]
    private string tooltipString;

    private Tooltip tooltip;

    // Start is called before the first frame update
    void Start()
    {
        tooltip = tooltipObject.GetComponent<Tooltip>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseEnter()
    {
        tooltip.showTooltip(tooltipString);
    }

    private void OnMouseExit()
    {
        tooltip.hideTooltip();
    }
}
