#region Using declarations
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml.Serialization;
using NinjaTrader.Cbi;
using NinjaTrader.Data;
using NinjaTrader.Gui.Chart;
#endregion

// This namespace holds all indicators and is required. Do not change it.
namespace NinjaTrader.Indicator
{
    /// <summary>
    /// Alerts for anaAwesome Indicator
    /// </summary>
    [Description("Alerts for anaAwesome Indicator")]
    public class AwesomeAlerts : Indicator
    {
        #region Variables
        // Wizard generated variables
        // User defined variables (add any user defined variables below)
		
		private double zeroLine= 0;
		
        #endregion

        /// <summary>
        /// This method is used to configure the indicator and is called once before any bar data is loaded.
        /// </summary>
        protected override void Initialize()
        {
            Add(new Plot(Color.FromKnownColor(KnownColor.Orange), PlotStyle.Bar, "Plot0"));
            Overlay				= false;
        }

        /// <summary>
        /// Called on each bar update event (incoming tick)
        /// </summary>
        protected override void OnBarUpdate()
        {
            // Use this method for calculating your indicator values. Assign a value to each
            // plot below by replacing 'Close[0]' with your own formula.
          if (CrossAbove(anaAwesomeOscillator(5, false, 34, 3).Oscillator, ZeroLine, 1))
            {
                DrawSquare("My square" + CurrentBar, false, 0, 0, Color.Orange);         
                Alert("MyAlert6", NinjaTrader.Cbi.Priority.High, "AnaAwesome CrossAbove0", "Alert1.wav", 10, Color.White, Color.Black);
				Print("CrossAbove"); 
				Print(Instrument.FullName);
				Print(","+Time[0]+" "+ Close[0]);
				Plot0.Set(Close[0]);
		
            }

            // Condition set 2
            if (CrossBelow(anaAwesomeOscillator(5, false, 34, 3).Oscillator, ZeroLine, 1))
            {
                DrawSquare("My square" + CurrentBar, false, 0, 0, Color.Magenta);            
                Alert("MyAlert7", NinjaTrader.Cbi.Priority.High, "AnaAwesome Crossed Below 0","Alert2.wav", 10, Color.White, Color.Black);
				Print("CrossBelow");
				Print(Instrument.FullName);
				Print(","+Time[0]+" "+ Close[0]);
            }
        }

        #region Properties
        [Browsable(false)]	// this line prevents the data series from being displayed in the indicator properties dialog, do not remove
        [XmlIgnore()]		// this line ensures that the indicator can be saved/recovered as part of a chart template, do not remove
        public DataSeries Plot0
        {
           get { return Values[0]; }
        }
		public double ZeroLine
		{
			get { return zeroLine; }
			set { zeroLine = Math.Max(0, value); }
		}

        #endregion
    }
}

#region NinjaScript generated code. Neither change nor remove.
// This namespace holds all indicators and is required. Do not change it.
namespace NinjaTrader.Indicator
{
    public partial class Indicator : IndicatorBase
    {
        private AwesomeAlerts[] cacheAwesomeAlerts = null;

        private static AwesomeAlerts checkAwesomeAlerts = new AwesomeAlerts();

        /// <summary>
        /// Alerts for anaAwesome Indicator
        /// </summary>
        /// <returns></returns>
        public AwesomeAlerts AwesomeAlerts()
        {
            return AwesomeAlerts(Input);
        }

        /// <summary>
        /// Alerts for anaAwesome Indicator
        /// </summary>
        /// <returns></returns>
        public AwesomeAlerts AwesomeAlerts(Data.IDataSeries input)
        {
            if (cacheAwesomeAlerts != null)
                for (int idx = 0; idx < cacheAwesomeAlerts.Length; idx++)
                    if (cacheAwesomeAlerts[idx].EqualsInput(input))
                        return cacheAwesomeAlerts[idx];

            lock (checkAwesomeAlerts)
            {
                if (cacheAwesomeAlerts != null)
                    for (int idx = 0; idx < cacheAwesomeAlerts.Length; idx++)
                        if (cacheAwesomeAlerts[idx].EqualsInput(input))
                            return cacheAwesomeAlerts[idx];

                AwesomeAlerts indicator = new AwesomeAlerts();
                indicator.BarsRequired = BarsRequired;
                indicator.CalculateOnBarClose = CalculateOnBarClose;
#if NT7
                indicator.ForceMaximumBarsLookBack256 = ForceMaximumBarsLookBack256;
                indicator.MaximumBarsLookBack = MaximumBarsLookBack;
#endif
                indicator.Input = input;
                Indicators.Add(indicator);
                indicator.SetUp();

                AwesomeAlerts[] tmp = new AwesomeAlerts[cacheAwesomeAlerts == null ? 1 : cacheAwesomeAlerts.Length + 1];
                if (cacheAwesomeAlerts != null)
                    cacheAwesomeAlerts.CopyTo(tmp, 0);
                tmp[tmp.Length - 1] = indicator;
                cacheAwesomeAlerts = tmp;
                return indicator;
            }
        }
    }
}

// This namespace holds all market analyzer column definitions and is required. Do not change it.
namespace NinjaTrader.MarketAnalyzer
{
    public partial class Column : ColumnBase
    {
        /// <summary>
        /// Alerts for anaAwesome Indicator
        /// </summary>
        /// <returns></returns>
        [Gui.Design.WizardCondition("Indicator")]
        public Indicator.AwesomeAlerts AwesomeAlerts()
        {
            return _indicator.AwesomeAlerts(Input);
        }

        /// <summary>
        /// Alerts for anaAwesome Indicator
        /// </summary>
        /// <returns></returns>
        public Indicator.AwesomeAlerts AwesomeAlerts(Data.IDataSeries input)
        {
            return _indicator.AwesomeAlerts(input);
        }
    }
}

// This namespace holds all strategies and is required. Do not change it.
namespace NinjaTrader.Strategy
{
    public partial class Strategy : StrategyBase
    {
        /// <summary>
        /// Alerts for anaAwesome Indicator
        /// </summary>
        /// <returns></returns>
        [Gui.Design.WizardCondition("Indicator")]
        public Indicator.AwesomeAlerts AwesomeAlerts()
        {
            return _indicator.AwesomeAlerts(Input);
        }

        /// <summary>
        /// Alerts for anaAwesome Indicator
        /// </summary>
        /// <returns></returns>
        public Indicator.AwesomeAlerts AwesomeAlerts(Data.IDataSeries input)
        {
            if (InInitialize && input == null)
                throw new ArgumentException("You only can access an indicator with the default input/bar series from within the 'Initialize()' method");

            return _indicator.AwesomeAlerts(input);
        }
    }
}
#endregion
