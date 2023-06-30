using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseOverEditMenu : MonoBehaviour
{
    public static bool isOverMenu = false;


    private void OnMouseEnter()
    {
        isOverMenu = true;
    }

    private void OnMouseExit()
    {
        isOverMenu = false;
    }
}
