using UnityEngine;

public interface IPickup
{
    public void getWeaponStats(weaponStats weapon);

    public void getHealthItemStats(healthItems item); // added this line - Sam

    public void getSpeedItemStats(speedItems item); // added - Cade

    public void getJumpItemStats(jumpItems item); // Cade
}
