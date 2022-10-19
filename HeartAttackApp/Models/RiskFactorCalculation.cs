namespace HeartAttackApp.Models
{
    public class RiskFactorCalculation
    {
        public decimal BpRiskPercent { get; set; }
        public decimal ColRiskPercent { get; set; }
        public decimal SugRiskPercent { get; set; }
        public decimal AvgRiskPercent { get; set; }
        public string? BpRisk { get; set; }
        public string? ColRisk { get; set; }
        public string? SugRisk { get; set; }
    }
}
