namespace TieFighter.Models
{
    public class MatchResults
    {
        [AncestorPath(typeof(Match))]
        public Match ParentMatch { get; set; }
        public User Player { get; set; }
        public double LevelIncreased { get; set; }
        public Medal[] Medals { get; set; }
    }
}
