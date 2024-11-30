using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
public enum EquipmentType
{
    Weapon,// ����
    Armor,// ����
    Amulet,// ����
    Flask// ҩˮ
}
[CreateAssetMenu(fileName = "New Equipment Item", menuName = "Data/Equipment Data")]
public class ItemData_Equipment : ItemData
{

    [TextArea(3, 10)]
    public string Effectdescription;
    public EquipmentType equipmentType;
    public float cooldown;
    [Header("Major Stats")]

    public float strength;//����������һ���˺���1%�ı�����
    public float agility;//���ݣ�����һ���ƶ��ٶȣ�ֻ�ܼ��ٺ����ļ���Ч������1%��������
    public float intelligence;//����������3��ħ�����Ժ�1���ħ��������
    public float vitality;//����������3��5���������ֵ
    [Header("Attack Stats")]
 
    public float damage;
    public float criticalChance;
    public float criticalPower;//
    [Header("Defensive Stats")]
    public float maxHealth;
    public float armor;//����
    public float evasion;//����
    [Header("Magic Stats")]
    public float magicResist;
    public float fireDamage;
    public float iceDamage;
    public float lightningDamage;
    [Header("Materilas")]
    public List<InventoryItem> materials;
    [Header("Item Effects")]
    public ItemEffect[] itemEffects;
    
    private int descriptionLength;
    
    public void AddModifer()
    {
        Player player = PlayerManager.instance.player;
        if (player == null) return;
        player.stats.strength.AddModifier(strength);
        player.stats.agility.AddModifier(agility);
        player.stats.intelligence.AddModifier(intelligence);
        player.stats.vitality.AddModifier(vitality);
        player.stats.damage.AddModifier(damage);
        player.stats.criticalChance.AddModifier(criticalChance);
        player.stats.criticalPower.AddModifier(criticalPower);  
        player.stats.maxHealth.AddModifier(maxHealth);
        player.stats.currentHealth += maxHealth;
        player.stats.armor.AddModifier(armor);
        player.stats.evasion.AddModifier(evasion);
        player.stats.FireDamage.AddModifier(fireDamage);
        player.stats.IceDamage.AddModifier(iceDamage);
        player.stats.LightningDamage.AddModifier(lightningDamage);
        player.stats.magicResist.AddModifier(magicResist);
    }
    public void RemoveModifer()
    {
        Player player = PlayerManager.instance.player;
        if (player == null) return;
        player.stats.strength.RemoveModifier(strength);
        player.stats.agility.RemoveModifier(agility);
        player.stats.intelligence.RemoveModifier(intelligence);
        player.stats.vitality.RemoveModifier(vitality);
        player.stats.damage.RemoveModifier(damage);         
        player.stats.criticalChance.RemoveModifier(criticalChance);
        player.stats.criticalPower.RemoveModifier(criticalPower);
        player.stats.maxHealth.RemoveModifier(maxHealth);
        player.stats.armor.RemoveModifier(armor);
        player.stats.evasion.RemoveModifier(evasion);
        player.stats.FireDamage.RemoveModifier(fireDamage);
        player.stats.IceDamage.RemoveModifier(iceDamage);
        player.stats.LightningDamage.RemoveModifier(lightningDamage);    
        player.stats.magicResist.RemoveModifier(magicResist);
    }
    public void ExecuteEffects(Transform target)
    {
        foreach (ItemEffect effect in itemEffects)
        {
            effect.ExecuteEffect(target);
        }
    }
    public override string GetDescription()
    {
        descriptionLength = 0;
        sb.Length = 0;
        AddItemDescription("Strength", strength);
        AddItemDescription("Agility", agility);
        AddItemDescription("Intelligence", intelligence);
        AddItemDescription("Vitality", vitality);
        AddItemDescription("Damage", damage);
        AddItemDescription("Critical Chance", criticalChance);
        AddItemDescription("Critical Power", criticalPower);
        AddItemDescription("Max Health", maxHealth);
        AddItemDescription("Armor", armor);
        AddItemDescription("Evasion", evasion);
        AddItemDescription("Fire Damage", fireDamage);
        AddItemDescription("Ice Damage", iceDamage);
        if (descriptionLength < 5)
        {
            for(int i = 0; i < 5 - descriptionLength; i++)
            {
                sb.AppendLine();
                sb.Append(" ");
            }
        }
        if(Effectdescription!= "")
        {
            sb.AppendLine();
            sb.AppendLine("Effect Description:"+Effectdescription);
           
        }
       return sb.ToString();
    }
    private void AddItemDescription(string name, float value)
    {
        if(value!= 0)
        {
            if(sb.Length > 0)
            {
                sb.AppendLine();
            }
            sb.Append("+ " + value + " " + name);
            descriptionLength++;
            
        }
    }
    
   
}
