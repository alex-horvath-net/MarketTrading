## Features

- FX Options Trading System  

  Delivered real-time implied volatility data for FX Options Pricing and Risk Engines, based on strike and tenor requests. The Albacore system ingested market data such as ATM volatility, Risk Reversals, and Butterflies from Bloomberg, Refinitiv, and internal sources, and constructed the volatility surface using interpolation and extrapolation. The Pricing Engine consumed this implied volatility to calculate fair market value (premium) and Greeks (Delta, Gamma, Vega, Theta, Rho) using Black-Scholes or Garman-Kohlhagen models. The Risk Engine used the same data to compute portfolio-level risk metrics, including Greek aggregation, Value-at-Risk (VaR), and stress testing. 

 

- Fixed Income Trading System 

  Delivered real-time pricing and risk metrics for Fixed Income Pricing and Risk Engines, based on ISIN, coupon rate, and maturity date. The Merlin system ingested market data such as bond quotes, benchmark rates (SONIA, SOFR), swap curves, and credit spreads from Bloomberg and internal sources. Constructed zero-coupon, forward, and credit-adjusted curves using bootstrapping and interpolation (linear, log-linear, cubic spline). Merlin calculated standard pricing outputs (clean price, accrued interest, yield-to-maturity) and instrument-level risk metrics (DV01, PV01, duration, convexity). The Pricing Engine used these outputs to determine fair market value via discounted cash flow (DCF) models. The Risk Engine consumed the same inputs to run real-time portfolio-level analytics including sensitivities, VaR, scenario analysis, and interest rate stress testing. 


## Development SSL Certificate

This project uses a local HTTPS dev certificate. To set it up:

```powershell
# Generate cert file
dotnet dev-certs https -ep ./certificates/aspnetapp.pfx -p YourPassword123

# Trust it (Windows/macOS)
dotnet dev-certs https --trust
