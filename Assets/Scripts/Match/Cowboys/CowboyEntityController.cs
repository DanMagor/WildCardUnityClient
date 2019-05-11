using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Prefabs.DataSaving;
using UnityEngine;
using UnityEngine.UI;

public class CowboyEntityController : MonoBehaviour
{

    private const int maxHealth = 100;

    public string Username;
    public bool AmIShot;
    public int HP;
    public int Armor;

    [SerializeField]
    private CowboyParticleSystem DamageParticles;
    [SerializeField]
    private CowboyParticleSystem HealParticles;
    [SerializeField]
    private CowboyParticleSystem ArmorIncreaseParticles;
    [SerializeField]
    private CowboyParticleSystem ArmorDecreaseParticles;

    [SerializeField]
    private Animator AnimatorController;


    //UI
    [SerializeField]
    private Text playerHPLabel;
    [SerializeField]
    private Text playerArmorLabel;

    public void Awake()
    {
        AnimatorController = GetComponent<Animator>();
        HP = 100;
        Armor = 0;
    }

    #region Actions
    public void HitByCard(CardEntity card)
    {
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
                Debug.LogError("Something Wrong. No type on card");
                break;
        }
        UpdateUI();
    }
    public void Shot()
    {
        AnimatorController.Play("Shot"); 
    }
    public void Die()
    {
       // AnimatorController.Play("Die");
    }
    #endregion

    #region Stats Changing
    ///Important! Logic should be the same as on server for correct feedback animation
    private void GetDamage(int value)
    {
        if (Armor <= 0)
        {
            HP = Mathf.Max(HP-value, 0);
            DamageParticles.textMesh.text = value.ToString();
            DamageParticles.enabled = true;
            
            if (HP<=0){
                AnimatorController.Play("Die");
            }
            else
            {
                AnimatorController.Play("GetDamage");
            }

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
    private void GetHeal(int value)
    {
        if (HP < maxHealth)
        {
            HealParticles.textMesh.text = value.ToString();
            HealParticles.enabled = true;
            AnimatorController.Play("GetHeal");
        }
        HP = Math.Min(maxHealth, HP + value);
        
    }
    private void GetArmor(int value)
    {
        Armor += value;
        AnimatorController.Play("IncreaseArmor");
        ArmorIncreaseParticles.textMesh.text = value.ToString();
        ArmorIncreaseParticles.enabled = true;
    }
    #endregion

    public void UpdateUI()
    {
        playerHPLabel.text = HP.ToString();
        playerArmorLabel.text = Armor.ToString();
    }

}
