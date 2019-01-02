#region Using declarations
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.Xml.Serialization;
using NinjaTrader.Cbi;
using NinjaTrader.Data;
using NinjaTrader.Gui.Chart;
#endregion

// This namespace holds all indicators and is required. Do not change it.
namespace NinjaTrader.Indicator
{
    /// <summary>
    /// </summary>
    [Description("The Awesome Oscillator shows the difference between a 5-period and a 34-period simple moving average")]
    public class anaAwesomeOscillator : Indicator
    {
        #region Variables
        // Wizard generated variables
            private int fastPeriod 			= 5; 
            private int slowPeriod 			= 34; 
			private int smooth		 		= 5; 
			private int barWidth			= 5;
			private int lineWidth			= 2;
			private double oscillatorValue 	= 0;
			private Color upColor 			= Color.LimeGreen;
			private Color downColor 		= Color.Red;
			private Color signalColor		= Color.LightSteelBlue;
			private bool showLines			= false;
			private SMA fastSMA;
			private SMA slowSMA;
		#endregion

        /// <summary>
        /// This method is used to configure the indicator and is called once before any bar data is loaded.
        /// </summary>
        protected override void Initialize()
        {
			Add(new Plot(new Pen(Color.Gray, 2), "OscillatorLine"));
			Add(new Plot(new Pen(SignalColor, 2), "SignalLine"));
			Add(new Plot(new Pen(Color.Gray, 6), PlotStyle.Bar, "Awesome Oscillator"));
			Add(new Line(Color.DarkGray, 0, "Zero line"));
			
            CalculateOnBarClose	= false;
            Overlay				= false;
			PlotsConfigurable	= false;
        }

		/// <summary>
        /// Called on each bar update event (incoming tick)
        /// </summary>
        protected override void OnStartUp()
        {
			Plots[1].Pen.Color = signalColor;
			Plots[0].Pen.Width = lineWidth;
			Plots[1].Pen.Width = lineWidth;
			Plots[2].Pen.Width = barWidth;
			fastSMA = SMA(Median, fastPeriod);
			slowSMA = SMA(Median, slowPeriod);
		}	

		/// <summary>
        /// Called on each bar update event (incoming tick)
        /// </summary>
        protected override void OnBarUpdate()
        {
            if (CurrentBar < 1)
			{
				OscillatorLine.Set(0);
				SignalLine.Set(0);
				Oscillator.Set(0);	
				return;
			}
			
			oscillatorValue = fastSMA[0]-slowSMA[0];
			if (ShowLines)
			{
				OscillatorLine.Set (oscillatorValue);
				SignalLine.Set(SMA(OscillatorLine,Smooth)[0]);
			}
			else
			{
				OscillatorLine.Reset();
				SignalLine.Reset();
			}
			Oscillator.Set(oscillatorValue);
			if (Rising(Oscillator))
			{
				PlotColors[0][0] = UpColor;
				PlotColors[2][0] = UpColor;
			}
			else
			{
				PlotColors[0][0] = DownColor;
				PlotColors[2][0] = DownColor;
			}
		}

		
        #region Properties
       
		/// <summary>
		/// </summary>
		[Browsable(false)]
		[XmlIgnore()]
		public DataSeries OscillatorLine
		{
			get { return Values[0]; }
		}		/// <summary>
		
		/// <summary>
		/// </summary>
		[Browsable(false)]
		[XmlIgnore()]
		public DataSeries SignalLine
		{
			get { return Values[1]; }
		}
		
		/// <summary>
		/// </summary>
		[Browsable(false)]
		[XmlIgnore()]
		public DataSeries Oscillator
		{
			get { return Values[2]; }
		}
		
		/// <summary>
		/// </summary>
		[Description("Show Oscillator and Signalline")]
		[Gui.Design.DisplayName("Display Lines")]
		[Category("Parameters")]
		public bool ShowLines
		{
			get { return showLines; }
			set { showLines = value; }
		}

		/// <summary>
		/// </summary>
		[Description("Period for fast EMA")]
		[Category("Parameters")]
		public int FastPeriod
		{
			get { return fastPeriod; }
			set { fastPeriod = Math.Max(1, value); }
		}

		/// <summary>
		/// </summary>
		[Description("Period for slow EMA")]
		[Category("Parameters")]
		public int SlowPeriod
		{
			get { return slowPeriod; }
			set { slowPeriod = Math.Max(1, value); }
		}

		/// <summary>
		/// </summary>
		[Description("Period for Smoothing of Signal Line")]
		[Category("Parameters")]
		public int Smooth
		{
			get { return smooth; }
			set { smooth = Math.Max(1, value); }
		}

		/// <summary>
		/// </summary>
		[Description("Width of bars")]
		[Gui.Design.DisplayName("Bar Width")]
		[Category("Plots")]
		public int BarWidth
		{
			get { return barWidth; }
			set { barWidth = Math.Max(1, value); }
		}

		/// <summary>
		/// </summary>
		[Description("Width of plotted lines")]
		[Gui.Design.DisplayName("Line Width")]
		[Category("Plots")]
		public int LineWidth
		{
			get { return lineWidth; }
			set { lineWidth = Math.Max(1, value); }
		}

		/// <summary>
		/// </summary>
        [XmlIgnore()]		
		[Description("Select Color")]
		[Category("Plots")]
		[Gui.Design.DisplayName("Oscillator Rising")]
		public Color UpColor
		{
			get { return upColor; }
			set { upColor = value; }
		}
		
		// Serialize Color object
		[Browsable(false)]
		public string UpColorSerialize
		{
			get { return NinjaTrader.Gui.Design.SerializableColor.ToString(upColor); }
			set { upColor = NinjaTrader.Gui.Design.SerializableColor.FromString(value); }
		}

		/// <summary>
		/// </summary>
        [XmlIgnore()]		
		[Description("Select Color")]
		[Category("Plots")]
		[Gui.Design.DisplayName("Oscillator Falling")]
		public Color DownColor
		{
			get { return downColor; }
			set { downColor = value; }
		}
		
		// Serialize Color object
		[Browsable(false)]
		public string DownColorSerialize
		{
			get { return NinjaTrader.Gui.Design.SerializableColor.ToString(downColor); }
			set { downColor = NinjaTrader.Gui.Design.SerializableColor.FromString(value); }
		}

		/// <summary>
		/// </summary>
		[XmlIgnore()]		
		[Description("Select Color")]
		[Category("Plots")]
		[Gui.Design.DisplayName("Signalline")]
		public Color SignalColor
		{
			get { return signalColor; }
			set { signalColor = value; }
		}
		
		// Serialize Color object
		[Browsable(false)]
		public string SignalColorSerialize
		{
			get { return NinjaTrader.Gui.Design.SerializableColor.ToString(signalColor); }
			set { signalColor = NinjaTrader.Gui.Design.SerializableColor.FromString(value); }
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
        private anaAwesomeOscillator[] cacheanaAwesomeOscillator = null;

        private static anaAwesomeOscillator checkanaAwesomeOscillator = new anaAwesomeOscillator();

        /// <summary>
        /// The Awesome Oscillator shows the difference between a 5-period and a 34-period simple moving average
        /// </summary>
        /// <returns></returns>
        public anaAwesomeOscillator anaAwesomeOscillator(int fastPeriod, bool showLines, int slowPeriod, int smooth)
        {
            return anaAwesomeOscillator(Input, fastPeriod, showLines, slowPeriod, smooth);
        }

        /// <summary>
        /// The Awesome Oscillator shows the difference between a 5-period and a 34-period simple moving average
        /// </summary>
        /// <returns></returns>
        public anaAwesomeOscillator anaAwesomeOscillator(Data.IDataSeries input, int fastPeriod, bool showLines, int slowPeriod, int smooth)
        {
            if (cacheanaAwesomeOscillator != null)
                for (int idx = 0; idx < cacheanaAwesomeOscillator.Length; idx++)
                    if (cacheanaAwesomeOscillator[idx].FastPeriod == fastPeriod && cacheanaAwesomeOscillator[idx].ShowLines == showLines && cacheanaAwesomeOscillator[idx].SlowPeriod == slowPeriod && cacheanaAwesomeOscillator[idx].Smooth == smooth && cacheanaAwesomeOscillator[idx].EqualsInput(input))
                        return cacheanaAwesomeOscillator[idx];

            lock (checkanaAwesomeOscillator)
            {
                checkanaAwesomeOscillator.FastPeriod = fastPeriod;
                fastPeriod = checkanaAwesomeOscillator.FastPeriod;
                checkanaAwesomeOscillator.ShowLines = showLines;
                showLines = checkanaAwesomeOscillator.ShowLines;
                checkanaAwesomeOscillator.SlowPeriod = slowPeriod;
                slowPeriod = checkanaAwesomeOscillator.SlowPeriod;
                checkanaAwesomeOscillator.Smooth = smooth;
                smooth = checkanaAwesomeOscillator.Smooth;

                if (cacheanaAwesomeOscillator != null)
                    for (int idx = 0; idx < cacheanaAwesomeOscillator.Length; idx++)
                        if (cacheanaAwesomeOscillator[idx].FastPeriod == fastPeriod && cacheanaAwesomeOscillator[idx].ShowLines == showLines && cacheanaAwesomeOscillator[idx].SlowPeriod == slowPeriod && cacheanaAwesomeOscillator[idx].Smooth == smooth && cacheanaAwesomeOscillator[idx].EqualsInput(input))
                            return cacheanaAwesomeOscillator[idx];

                anaAwesomeOscillator indicator = new anaAwesomeOscillator();
                indicator.BarsRequired = BarsRequired;
                indicator.CalculateOnBarClose = CalculateOnBarClose;
#if NT7
                indicator.ForceMaximumBarsLookBack256 = ForceMaximumBarsLookBack256;
                indicator.MaximumBarsLookBack = MaximumBarsLookBack;
#endif
                indicator.Input = input;
                indicator.FastPeriod = fastPeriod;
                indicator.ShowLines = showLines;
                indicator.SlowPeriod = slowPeriod;
                indicator.Smooth = smooth;
                Indicators.Add(indicator);
                indicator.SetUp();

                anaAwesomeOscillator[] tmp = new anaAwesomeOscillator[cacheanaAwesomeOscillator == null ? 1 : cacheanaAwesomeOscillator.Length + 1];
                if (cacheanaAwesomeOscillator != null)
                    cacheanaAwesomeOscillator.CopyTo(tmp, 0);
                tmp[tmp.Length - 1] = indicator;
                cacheanaAwesomeOscillator = tmp;
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
        /// The Awesome Oscillator shows the difference between a 5-period and a 34-period simple moving average
        /// </summary>
        /// <returns></returns>
        [Gui.Design.WizardCondition("Indicator")]
        public Indicator.anaAwesomeOscillator anaAwesomeOscillator(int fastPeriod, bool showLines, int slowPeriod, int smooth)
        {
            return _indicator.anaAwesomeOscillator(Input, fastPeriod, showLines, slowPeriod, smooth);
        }

        /// <summary>
        /// The Awesome Oscillator shows the difference between a 5-period and a 34-period simple moving average
        /// </summary>
        /// <returns></returns>
        public Indicator.anaAwesomeOscillator anaAwesomeOscillator(Data.IDataSeries input, int fastPeriod, bool showLines, int slowPeriod, int smooth)
        {
            return _indicator.anaAwesomeOscillator(input, fastPeriod, showLines, slowPeriod, smooth);
        }
    }
}

// This namespace holds all strategies and is required. Do not change it.
namespace NinjaTrader.Strategy
{
    public partial class Strategy : StrategyBase
    {
        /// <summary>
        /// The Awesome Oscillator shows the difference between a 5-period and a 34-period simple moving average
        /// </summary>
        /// <returns></returns>
        [Gui.Design.WizardCondition("Indicator")]
        public Indicator.anaAwesomeOscillator anaAwesomeOscillator(int fastPeriod, bool showLines, int slowPeriod, int smooth)
        {
            return _indicator.anaAwesomeOscillator(Input, fastPeriod, showLines, slowPeriod, smooth);
        }

        /// <summary>
        /// The Awesome Oscillator shows the difference between a 5-period and a 34-period simple moving average
        /// </summary>
        /// <returns></returns>
        public Indicator.anaAwesomeOscillator anaAwesomeOscillator(Data.IDataSeries input, int fastPeriod, bool showLines, int slowPeriod, int smooth)
        {
            if (InInitialize && input == null)
                throw new ArgumentException("You only can access an indicator with the default input/bar series from within the 'Initialize()' method");

            return _indicator.anaAwesomeOscillator(input, fastPeriod, showLines, slowPeriod, smooth);
        }
    }
}
#endregion
