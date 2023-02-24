#region Using declarations
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Serialization;
using NinjaTrader.Cbi;
using NinjaTrader.Gui;
using NinjaTrader.Gui.Chart;
using NinjaTrader.Gui.SuperDom;
using NinjaTrader.Gui.Tools;
using NinjaTrader.Data;
using NinjaTrader.NinjaScript;
using NinjaTrader.Core.FloatingPoint;
using NinjaTrader.NinjaScript.DrawingTools;
#endregion

//This namespace holds Indicators in this folder and is required. Do not change it. 
namespace NinjaTrader.NinjaScript.Indicators
{
	public class TrendMagic : Indicator
	{
		private double cciVal 					= 0.0;
		private double atrVal 					= 0.0;
		private double upTrend 					= 0.0;
		private double downTrend 				= 0.0;
		private Series<double> lineColor;
		
		protected override void OnStateChange()
		{
			if (State == State.SetDefaults)
			{
				Description						= @"An ATR based trendline regulated by the position of CCI and its zero line.";
				Name							= "TrendMagic";
				IsOverlay						= true; 
				IsSuspendedWhileInactive		= true;
				cciPeriod 						= 20;
				atrPeriod 						= 14;
				atrMult							= 1.0;
				useCciTrendColor				= false;
				BullBrush						= Brushes.Lime;
				BearBrush						= Brushes.Red;
				AddPlot(new Stroke(Brushes.Orange, 2), PlotStyle.Line, Name);
				AddPlot(Brushes.Transparent, "Trend Output");
				AddPlot(Brushes.Transparent, "Buy/Sell Output");
			}
			else if (State == State.DataLoaded)
			{
				Trend[0] = 0;
				lineColor = new Series<double>(this);
			}
		}
		

		protected override void OnBarUpdate()
		{
			if (CurrentBar < cciPeriod || CurrentBar < atrPeriod)
				return;
			
			cciVal = CCI(Close, cciPeriod)[0];
			atrVal = ATR(Close, atrPeriod)[0];
			
			upTrend = Low[0] - atrVal * atrMult;
			downTrend = High[0] + atrVal * atrMult;
							
			if (cciVal >= 0)
				if (upTrend < Trend[1])
					Trend[0] = Trend[1];
				else
					Trend[0] = upTrend;
			else
				if (downTrend > Trend[1])
					Trend[0] = Trend[1];
				else
					Trend[0] = downTrend;
				
			TrendOutput[0] = Trend[0] > Trend[1] ? 1 : Trend[0] < Trend[1] ? -1 : TrendOutput[1];
			BuySellOutput[0] = TrendOutput[0] == 1 && TrendOutput[1] != 1 ? 1 : TrendOutput[0] == -1 && TrendOutput[1] != -1 ? -1 : 0;
				
			if (useCciTrendColor)
				lineColor[0] = cciVal > 0 ? 1 : -1;
			else
				lineColor[0] = TrendOutput[0];
			
			PlotBrushes[0][0] = lineColor[0] == 1 ? BullBrush : BearBrush;	
		}

		#region Properties
		[NinjaScriptProperty]
		[Range(1, int.MaxValue)]
		[Display(Name="CCI Period", Order=1, GroupName="Parameters")]
		public int cciPeriod
		{ get; set; }
		
		[NinjaScriptProperty]
		[Range(1, int.MaxValue)]
		[Display(Name="ATR Period", Order=2, GroupName="Parameters")]
		public int atrPeriod
		{ get; set; }
		
		[NinjaScriptProperty]
		[Range(-20, int.MaxValue)]
		[Display(Name="ATR Multiplier", Order=3, GroupName="Parameters")]
		public double atrMult
		{ get; set; }
		
		[NinjaScriptProperty]
		[Display(Name="Use CCI Trend for Line Color", Order=4, GroupName="Parameters")]
		public bool useCciTrendColor
		{ get; set; }
		
		[XmlIgnore()]
		[Display(Name = "Bull Color", GroupName="Colors", Order=1)]
		public Brush BullBrush
		{ get; set; }

		[Browsable(false)]
		public string BullBrushSerialize
		{
			get { return Serialize.BrushToString(BullBrush); }
   			set { BullBrush = Serialize.StringToBrush(value); }
		}
		
		[XmlIgnore()]
		[Display(Name = "Bear Color", GroupName="Colors", Order=2)]
		public Brush BearBrush
		{ get; set; }

		[Browsable(false)]
		public string BearBrushSerialize
		{
			get { return Serialize.BrushToString(BearBrush); }
   			set { BearBrush = Serialize.StringToBrush(value); }
		}

		[Browsable(false)]
		[XmlIgnore]
		public Series<double> Trend
		{
			get { return Values[0]; }
		}
		
		[Browsable(false)]
		[XmlIgnore]
		public Series<double> TrendOutput
		{
			get { return Values[1]; }
		}
		
		[Browsable(false)]
		[XmlIgnore]
		public Series<double> BuySellOutput
		{
			get { return Values[2]; }
		}
		#endregion

	}
}

#region NinjaScript generated code. Neither change nor remove.

namespace NinjaTrader.NinjaScript.Indicators
{
	public partial class Indicator : NinjaTrader.Gui.NinjaScript.IndicatorRenderBase
	{
		private TrendMagic[] cacheTrendMagic;
		public TrendMagic TrendMagic(int cciPeriod, int atrPeriod, double atrMult, bool useCciTrendColor)
		{
			return TrendMagic(Input, cciPeriod, atrPeriod, atrMult, useCciTrendColor);
		}

		public TrendMagic TrendMagic(ISeries<double> input, int cciPeriod, int atrPeriod, double atrMult, bool useCciTrendColor)
		{
			if (cacheTrendMagic != null)
				for (int idx = 0; idx < cacheTrendMagic.Length; idx++)
					if (cacheTrendMagic[idx] != null && cacheTrendMagic[idx].cciPeriod == cciPeriod && cacheTrendMagic[idx].atrPeriod == atrPeriod && cacheTrendMagic[idx].atrMult == atrMult && cacheTrendMagic[idx].useCciTrendColor == useCciTrendColor && cacheTrendMagic[idx].EqualsInput(input))
						return cacheTrendMagic[idx];
			return CacheIndicator<TrendMagic>(new TrendMagic(){ cciPeriod = cciPeriod, atrPeriod = atrPeriod, atrMult = atrMult, useCciTrendColor = useCciTrendColor }, input, ref cacheTrendMagic);
		}
	}
}

namespace NinjaTrader.NinjaScript.MarketAnalyzerColumns
{
	public partial class MarketAnalyzerColumn : MarketAnalyzerColumnBase
	{
		public Indicators.TrendMagic TrendMagic(int cciPeriod, int atrPeriod, double atrMult, bool useCciTrendColor)
		{
			return indicator.TrendMagic(Input, cciPeriod, atrPeriod, atrMult, useCciTrendColor);
		}

		public Indicators.TrendMagic TrendMagic(ISeries<double> input , int cciPeriod, int atrPeriod, double atrMult, bool useCciTrendColor)
		{
			return indicator.TrendMagic(input, cciPeriod, atrPeriod, atrMult, useCciTrendColor);
		}
	}
}

namespace NinjaTrader.NinjaScript.Strategies
{
	public partial class Strategy : NinjaTrader.Gui.NinjaScript.StrategyRenderBase
	{
		public Indicators.TrendMagic TrendMagic(int cciPeriod, int atrPeriod, double atrMult, bool useCciTrendColor)
		{
			return indicator.TrendMagic(Input, cciPeriod, atrPeriod, atrMult, useCciTrendColor);
		}

		public Indicators.TrendMagic TrendMagic(ISeries<double> input , int cciPeriod, int atrPeriod, double atrMult, bool useCciTrendColor)
		{
			return indicator.TrendMagic(input, cciPeriod, atrPeriod, atrMult, useCciTrendColor);
		}
	}
}

#endregion
