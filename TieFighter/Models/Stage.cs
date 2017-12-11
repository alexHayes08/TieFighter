namespace TieFighter.Models
{
    public enum StageTypes
    {
        Cinimatic,  // A scene where the player has no control
        Action,     // Player has control
        EndStage    // Game is over, mission ends
    };

    public class Stage : IDatastoreEntityAndJsonBinding
    {
        public string Id { get; set; }
        public StageTypes StageType { get; set; }
        [AncestorPath(typeof(Mission))]
        public Mission ParentMission { get; set; }
    }
}
