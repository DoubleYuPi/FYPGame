public enum AnimationName
{
    toolUp,
    toolDown,
    toolLeft,
    toolRight,
    pickupUp,
    pickupDown,
    pickupLeft,
    pickupRight,
    pickupIdleDown,
    pickupIdleUp,
    pickupIdleLeft,
    pickupIdleRight,
    walkDown,
    walkUp,
    walkLeft,
    walkRight,
    idleDown,
    idleUp,
    idleLeft,
    idleRight,
    count
}

public enum Places
{
    Overworld,
    MCHouse,
    Dungeon
}
public enum GridBool
{
    hoeable,
    dropItem
}

public enum PartVariantType
{
    none,
    carry,
    hoe,
    sickle,
    sword,
    wateringCan,
    count
}

public enum PlayerState
{
    idle,
    walk,
    attack,
    spell,
    hoeing,
    choping,
    watering,
    picking,
    interact,
    stagger,
    count
}

public enum EnemyState
{
    idle,
    walk,
    attack,
    stagger
}

public enum InventoryLocation
{
    player,
    chest,
    count
}

public enum ToolEffect
{
    none,
    watering
}

public enum ItemType
{
    Seed,
    Fruit,
    Watering_Tool,
    Hoeing_Tool,
    Choping_Tool,
    Pickup_Tool,
    Slaying_Tool,
    Destroyable_object,
    Door_Key,
    none,
    count
}

public enum DoorType
{
    key,
    button,
    mission
}
