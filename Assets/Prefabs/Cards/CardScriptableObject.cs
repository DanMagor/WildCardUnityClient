using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//TODO: Rework, Download Cards From server
[CreateAssetMenu(fileName = "Card", menuName = "Cards/AttackCard", order = 1)]
public class CardScriptableObject : ScriptableObject {

    public int ID;
    public Sprite image;
    public string bulletLabel;
    public string damageLabel;
}
