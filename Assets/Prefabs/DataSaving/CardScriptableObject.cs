using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//TODO: Rework, Download Cards From server
[CreateAssetMenu(fileName = "Card", menuName = "Cards/Card", order = 1)]
public class CardScriptableObject : ScriptableObject
{

    public int ID;
    public int Position;
    public bool Selected;
    public Animation CardAnimation;
    public Sprite Image;
    public string InfoText;
    public Sprite InfoIcon;

    public bool RightDirection;
    //public bool Attribute ??



}
