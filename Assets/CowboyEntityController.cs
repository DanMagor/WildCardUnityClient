using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Prefabs.DataSaving;
using UnityEngine;
using UnityEngine.UI;

public class CowboyEntityController : MonoBehaviour
{

    private const int maxHealth = 100;

    public string userName;
    public bool amIShot;
    public int HP;
    public int Armor;

    public CowboyParticleSystem DamageParticles;
    public CowboyParticleSystem HealParticles;
    public CowboyParticleSystem ArmorIncreaseParticles;
    public CowboyParticleSystem ArmorDecreaseParticles;


    public Animator AnimatorController;


    //UI
    public Text playerHPLabel;
    public Text playerArmorLabel;

    void Awake()
    {
        AnimatorController = GetComponent<Animator>();
        HP = 100;
        Armor = 0;
    }

    public void Shot()
    {
        AnimatorController.Play("Shot");
        
    }

    public void Die()
    {
        AnimatorController.Play("Die");
    }
    private void Update()
    {

    }

    public void UpdateUI()
    {
        
        playerHPLabel.text = HP.ToString();
        playerArmorLabel.text = Armor.ToString();

        
    }

    public void HitByCard(CardInstanceSerializable card)
    {

        Debug.LogFormat("Card {0} hitted {1}", card.ID, this.userName);
        switch (card.Type)
        {
            case "Attack":
                GetDamage(card.Value);
                break;
            case "Heal":
                GetHeal(card.Value);
                break;
            case "Armor":
                GetArmor(card.Value);
                break;
            case "Item":
                GetDamage(card.Value);
                break;
            default:
                Debug.Log("Something Wrong. No type on card");
                break;
        }

        UpdateUI();
    }
    

    ///Important! Logic should be the same as on server for correct feedback animation
    public void GetDamage(int value)
    {
        if (Armor <= 0)
        {
            HP -= value;
            DamageParticles.textMesh.text = value.ToString();
            DamageParticles.enabled = true;
            AnimatorController.Play("GetDamage");

        }
        else
        {
            Armor = Math.Max(0, Armor - value);
            if (Armor <= 0)
            {
                AnimatorController.Play("DestroyArmor");
            }
            else
            {
                AnimatorController.Play("DecreaseArmor");
            }
            ArmorDecreaseParticles.textMesh.text = value.ToString();
            ArmorDecreaseParticles.enabled = true;

        }

    }

    public void GetHeal(int value)
    {
        if (HP < maxHealth)
        {
            HealParticles.textMesh.text = value.ToString();
            HealParticles.enabled = true;
            AnimatorController.Play("GetHeal");
        }
        HP = Math.Min(maxHealth, HP + value);
        
    }

    public void GetArmor(int value)
    {
        Armor += value;
        AnimatorController.Play("IncreaseArmor");
        ArmorIncreaseParticles.textMesh.text = value.ToString();
        ArmorIncreaseParticles.enabled = true;
    }

    

}
