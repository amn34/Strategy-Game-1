using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverEffect : MonoBehaviour
{
    //how much the sprite will expand to give the hover effect
    public float hoverAmount;

    //when the mouse is over the sprite make it bigger
    public void OnMouseEnter()
    {
        transform.localScale += Vector3.one * hoverAmount;
    }

    // when the mouse leaves return it to its original size 
    public void OnMouseExit()
    {
        transform.localScale -= Vector3.one * hoverAmount;
    }

}
