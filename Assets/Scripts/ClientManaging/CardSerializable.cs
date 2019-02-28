using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardSerializable
{
    public int id;
    public string type;
    public string name;
    public string image;

    //Attack Card Info:
    public int damage;
    public int bullets;
    public int accuracy;

    //Heal Card Info:
    public int heal;

    //Item Card Info:
    public int itemDuration;
    public string itemEffectLabel;
    public string itemEffectImage;

    public int initiativeEffect;
    public int initiativeValue;
    public int initiativeDuration;
}
