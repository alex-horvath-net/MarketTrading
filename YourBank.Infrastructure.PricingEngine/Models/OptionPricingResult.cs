namespace YourBank.Infrastructure.PricingEngine.Models {
    public class OptionPricingResult {
        public decimal Price { get; set; }

        // Primary Greeks:
        public double Delta { get; set; }   // Sensitivity of option price to changes in underlying price.
        public double Gamma { get; set; }   // Sensitivity of Delta to changes in underlying price.
        public double Vega { get; set; }    // Sensitivity of option price to changes in volatility.
        public double Theta { get; set; }   // Time decay: rate of change of option price as expiration approaches.
        public double Rho { get; set; }     // Sensitivity of option price to changes in the risk-free interest rate.

        // Secondary Greeks:
        public double Charm { get; set; }   // Rate of change of Delta with respect to time (Delta decay).
        public double Vanna { get; set; }   // Sensitivity of Delta with respect to volatility (or Vega to changes in underlying price).
        public double Vomma { get; set; }   // Sensitivity of Vega to changes in volatility.
        public double Speed { get; set; }   // Rate of change of Gamma with respect to the underlying price.
        public double Color { get; set; }   // The time decay of Gamma.
        public double Zomma { get; set; }   // Sensitivity of Gamma to changes in volatility.
        public double Lambda { get; set; }  // Elasticity: percentage change in option price per percentage change in underlying price.
    }
}
