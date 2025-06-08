using Combat.Characters;
using Unity.Mathematics;
using UnityEngine;

public class CharacterInfo : IPowered
{
    public CharacterType characterType; // 角色类型

    private int health;
    public int Health { get => health; set => health = math.clamp(value, 0, maxHealth); }

    private int maxHealth;
    public int MaxHealth
    {
        get => maxHealth; set
        {
            maxHealth = math.max(1, value);
            if (health > maxHealth)
            {
                health = maxHealth; // 确保当前生命值不超过最大生命值
            }
        }
    }

    public int AttackPower { get; set; } // 攻击力
    public int HealPower { get; set; } // 治疗力
    public int DefensePower { get; set; } // 防御力
    public bool IsDead;

    public CharacterInfo(CharacterType type, int health, int maxHealth, int attackPower, int healPower, int defensePower)
    {
        characterType = type;
        this.health = math.clamp(health, 0, maxHealth);
        this.maxHealth = math.max(1, maxHealth);
        IsDead = this.health <= 0; // 如果初始生命值为0或负数，则视为死亡
        AttackPower = attackPower;
        HealPower = healPower;
        DefensePower = defensePower;
    }

    public CharacterInfo Clone()
    {
        return new CharacterInfo(characterType, health, maxHealth, AttackPower, HealPower, DefensePower);
    }

    public static CharacterInfo Create(CharacterType type)
    {
        return type switch
        {
            CharacterType.Warrior => new CharacterInfo(type, 40, 40, 2, 0, 1),
            CharacterType.Mage => new CharacterInfo(type, 30, 30, 1, 3, 0),
            CharacterType.Knight => new CharacterInfo(type, 50, 50, 1, 0, 3),
            _ => throw new System.ArgumentException("Unknown character type", nameof(type)),
        };
    }

    public void SetDead()
    {
        IsDead = true;
        health = 0;
    }

    public void Respawn()
    {
        IsDead = false;
        health = maxHealth / 2; // 复活时恢复一半生命值
    }

    public void Reset()
    {
        this.IsDead = false;
        Set(Create(characterType));
    }

    public void Set(CharacterInfo info)
    {
        if (info == null || info.characterType != characterType)
        {
            Debug.LogError($"Invalid character info to set: {info?.characterType} for {characterType}");
            throw new System.ArgumentException("Invalid character info to set", nameof(info));
        }

        if (IsDead && !info.IsDead)
        {
            Debug.LogError($"暂时无法通过Set方法复活角色，请使用Respawn方法。当前状态：IsDead={IsDead}, Info.IsDead={info.IsDead}");
            return;
        }

        if (info.IsDead && !IsDead)
        {
            SetDead();
            return;
        }

        if (IsDead)
        {
            return;
        }

        health = info.health;
        maxHealth = info.maxHealth;
        AttackPower = info.AttackPower;
        HealPower = info.HealPower;
        DefensePower = info.DefensePower;
    }
}