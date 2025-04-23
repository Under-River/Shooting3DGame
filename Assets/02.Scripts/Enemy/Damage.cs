using UnityEngine;

public struct Damage
{
    public int Value;
    public GameObject Attacker;
    public Damage(int value, GameObject attacker)
    {
        Value = value;
        Attacker = attacker;
    }
}