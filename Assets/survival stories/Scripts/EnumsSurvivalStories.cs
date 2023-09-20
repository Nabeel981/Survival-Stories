using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Flags]
public enum ObjectType
{
    None = 0,
    Resource = 1,
    Enemy = 2,
    Tool = 4,
    Building = 8,
    PlayerTrait = 16,
    Self = 32
}


public enum PlayerType
{
    User,
    AiEnemy

}

[System.Flags]
public enum ToolType
{
    None = 0,
    Forage = 1,
    Weapon = 2,
    Consumable = 4,
    Container = 8,

}
[System.Flags]
public enum ResourceBehaviour
{
    None = 0,
    Containable = 1,
    Gatherable = 2,
    Consumable = 4,


}

public enum AnnouncementType
{
    withImage,
    withoutImage

}
[System.Flags]
public enum UseType
{
    None = 0,
    AutoUse = 1,
    ManualUse = 2
}

[System.Flags]
public enum EffectType
{
    None = 0,
    RateChange = 1,
    ValueChange = 2


}


[System.Flags]
public enum PlayerTraits
{
    None = 0,
    Hunger = 1,
    Thirst = 2,
    Health = 4,
    Speed = 8,
    harvestSpeed = 16,
    Stamina = 32

}

[System.Flags]
public enum ResourcesType
{
    none = 0,
    wood = 1,
    stone = 2,
    gold = 4,
    water = 8,
    Bush = 16,
    Mushroom = 32,
    Glitterherb = 64,
    Mintyherb = 128,
    Featherherb = 256,
    Spicyherb = 512,
    Meat = 1024
}

public enum SkillType
{

    Swiftness,
    Healthy,
    Fighter,
    DoubleCraft,
    Durability,
    Harvester,
    Woody,
    Stoner,
    Efficient,
    Builder,
    LongRange




}
[System.Flags]
public enum Currency
{
    none = 0,
    Gold = 1,
    ChronoCrystals = 2

}



public class EnumsSurvivalStories : MonoBehaviour
{

}
