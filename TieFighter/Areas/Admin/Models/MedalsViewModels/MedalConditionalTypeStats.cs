namespace TieFighter.Areas.Admin.Models.MedalsViewModels
{
    public enum MedalConditionalTypeStatsOperators
    {
        GreaterThan,
        GreaterThanOrEqual,
        LessThan,
        LessThanOrEqual,
        Equal,
        NotEqual
    }

    public class MedalConditionalTypeStats
    {
        public string Name { get; set; }
        public double Value { get; set; }
        public MedalConditionalTypeStatsOperators Operator { get; set; }
        public bool ValueAsPercent { get; set; }
    }
}
