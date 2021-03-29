using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BNAAutoTrader
{
    class TradeAlgo
    {
        private MainForm frm;
        public TradeAlgo(MainForm frmParam)
        {
            frm = frmParam;
            Thread th = new Thread(CheckSignal);
            th.IsBackground = true;
            th.Start();
        }
        public bool DetectBuyLimitSignal()
        {
            double ma_value = 0.0;
            double sum_val = 0;
            double cu_av_value = (Global.current_rate.ask_value + Global.current_rate.bid_value) / 2;
            int index;

            if (Global.candle_array.Count < Global.buy_limit_min_th)
                return false;
            for (index = Global.candle_array.Count - 1; index >= 0; index--)
            {
                CandleData cd = Global.candle_array.ElementAt(index);
                sum_val += (cd.open_value + cd.close_value) / 2;
            }
            ma_value = sum_val / Global.buy_limit_min_th;
            double diff_val = ma_value - cu_av_value;
            if (diff_val * 100 / cu_av_value > 5)
                return true;

            return false;
        }

        public bool DetectSellLimitSignal()
        {
            double ma_value = 0.0;
            double sum_val = 0;
            double cu_av_value = (Global.current_rate.ask_value + Global.current_rate.bid_value) / 2;
            int index;
            if (Global.candle_array.Count < Global.sell_limit_min_th)
                return false;
            for (index = Global.candle_array.Count - Global.sell_limit_min_th; index < Global.candle_array.Count; index++)
            {
                CandleData cd = Global.candle_array.ElementAt(index);
                sum_val += (cd.open_value + cd.close_value) / 2;
            }
            ma_value = sum_val / Global.sell_limit_min_th;
            double diff_val = cu_av_value - ma_value;
            if (diff_val * 100 / cu_av_value > 1)
                return true;

            return false;
        }

        public bool DetectBuyMarketSignal()
        {
            double ma_value = 0.0, ma_vol_value = 0;
            double sum_val = 0, sum_vol_val = 0;
            double cu_av_value = (Global.current_rate.ask_value + Global.current_rate.bid_value) / 2;
            double cu_vol_value = Global.candle_data.volume_value;
            int index;
            if (Global.candle_array.Count < Global.buy_market_min_th)
                return false;
            for (index = Global.candle_array.Count - 1; index >= 0; index--)
            {
                CandleData cd = Global.candle_array.ElementAt(index);
                sum_val += (cd.open_value + cd.close_value) / 2;
                sum_vol_val += cd.volume_value;
            }
            ma_value = sum_val / Global.buy_market_min_th;
            ma_vol_value = sum_vol_val / Global.buy_market_min_th;
            double diff_val = cu_vol_value - ma_vol_value;
            if ((cu_av_value > ma_value && diff_val * 100 / cu_vol_value > 5) || CalculateRsi() < 35)
                return true;

            return false;
        }

        public bool DetectSellMarketSignal()
        {
            return false;
        }

        public double CalculateRsi()
        {
            var prices = Global.candle_array.ToArray();

            double Tolerance = 2e-10;
            double sumGain = 0;
            double sumLoss = 0;
            for (int i = 1; i < prices.Length; i++)
            {
                var difference = prices[i].close_value - prices[i - 1].close_value;
                if (difference >= 0)
                {
                    sumGain += difference;
                }
                else
                {
                    sumLoss -= difference;
                }
            }

            if (sumGain == 0) return 0;
            if (Math.Abs(sumLoss) < Tolerance) return 100;

            var relativeStrength = sumGain / sumLoss;

            return 100.0 - (100.0 / (1 + relativeStrength));
        }

        private void CheckSignal()
        {
            while(true)
            { 
                if (!Global.allow_auto_trade || !Global.order_condition || Global.connection_status == CONNECTION.DISCONNECTED || Global.trade_mth == TRADE_METHOD.MANUAL)
                {
                    Thread.Sleep(10);
                    continue;
                }
                if (DetectBuyLimitSignal())
                {
                    frm.placeLimitOrder(Binance.Net.Enums.OrderSide.Buy);
                }
                else if (DetectSellLimitSignal())
                {
                    frm.placeLimitOrder(Binance.Net.Enums.OrderSide.Sell);
                }
                else if (DetectBuyMarketSignal())
                {
                    frm.placeMarketOrder(Binance.Net.Enums.OrderSide.Buy);
                }
                else if (DetectSellMarketSignal())
                {
                    frm.placeMarketOrder(Binance.Net.Enums.OrderSide.Sell);
                }
                Thread.Sleep(10);
            }
        }
    }
}
