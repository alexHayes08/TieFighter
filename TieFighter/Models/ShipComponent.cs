namespace TieFighter.Models
{
    public enum ShipComponentType
    {
        LaserCannon,
        IonCannon,
        ProtonTorpedo,
        ConcussionMissile,
        LifeSupport,
        Engine,
        Thruster,
        FuelTank,
        SheildGenerator,
        DroidSocket,
        Cockpit,
        Sensors,
        CloakingDevice,
        HyperDrive
    };

    public class ShipComponent
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public object Value { get; set; }
    }
}
