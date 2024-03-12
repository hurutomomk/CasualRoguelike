using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName ="New Shrine", menuName = "Shrine/Create New Shrine")]
public class Shrine : ScriptableObject
{
    /// <summary>
    /// ShrineID
    /// </summary>
    public int shrineID;
    /// <summary>
    /// ShrineSprite
    /// </summary>
    public Sprite shrineSprite;
    /// <summary>
    /// ShrineName
    /// </summary>
    public string shrineName;
    /// <summary>
    /// ShrineDescription
    /// </summary>
    [TextArea(10,10)]
    public string shrineDescription;
    /// <summary>
    /// StatusBonus
    /// </summary>
    public int hp;
    public int maxHp;
    public int attack;
    public int critical;
    public int defence;
    public int agility;
}