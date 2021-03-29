using Binance.Net;
using Binance.Net.Enums;
using Binance.Net.Objects.Futures.FuturesData;
using Binance.Net.Objects.Spot.SpotData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BNAAutoTrader
{

    enum TYPE
    {
        SPOT,
        MARGIN,
        FUTURES
    };

    enum CONNECTION
    {
        CONNECTING,
        CONNECTED,
        DISCONNECTED
    }

    enum WALLET_TYPE
    {
        TESTNET,
        REAL
    }
    enum TRADE_METHOD
    {
        AUTO,
        MANUAL
    }
    class CandleData
    {
        public double open_value { get; set; }
        public double close_value { get; set; }
        public double high_value { get; set; }
        public double low_value { get; set; }
        public DateTime open_datetime { get; set; }
        public double volume_value { get; set; }
    }
    class Rate
    {
        public double ask_value { get; set; }
        public double bid_value { get; set; }
        public double ask_quantity_value { get; set; }
        public double bid_quantity_value { get; set; }
        public double interset_rate_value { get; set; }
        public double founding_rate_value { get; set; }
        public DateTime next_foundig_time { get; set; }
        public DateTime updatetime { get; set; }
        public TimeSpan timespan_tonext{ get; set; }

    }
    static class Global
    {
        public static BinanceClient client = null;

        public static BinanceSocketClient socketClient = null;

        public static TYPE type_status = TYPE.SPOT;

        public static CONNECTION connection_status = CONNECTION.DISCONNECTED;

        public static WALLET_TYPE wallet_status = WALLET_TYPE.TESTNET;

        public static string SPOT_TEST_BASEADDRESS = "https://testnet.binance.vision/";
        public static string FUTURE_TEST_USDTADDRESS = "https://testnet.binancefuture.com/";

        public static string SPOTWSS_TEST_BASEADDRESS = "wss://testnet.binance.vision/";
        public static string FUTUREWSS_TEST_USDTADDRESS = "wss://stream.binancefuture.com/";
        public static string FUTUREWSS_TEST_COINADDRESS = "wss://dstream.binancefuture.com/";

        public static string apiKey = "";
        public static string securityKey = "";

        public static string symbol = "";

        public static Rate current_rate = new Rate();

        public static CandleData candle_data = new CandleData();

        public static BinanceBalance spot_balance1, spot_balance2;

        public static string future_asset = "";
        public static decimal future_balance = 0;

        public static BinancePositionInfoUsdt future_position = new BinancePositionInfoUsdt();

        public static Log log;
        public static KlineInterval timeInterval;
        public static TRADE_METHOD trade_mth = TRADE_METHOD.MANUAL;

        public static Queue<CandleData> candle_array = new Queue<CandleData>();

        public static bool allow_auto_trade = false;

        public static bool order_condition = false;

        public static double sell_limit_price = 0;
        public static double buy_limit_price = 0;
        public static TradeAlgo tAl;

        public static int sell_limit_min_th = 10;
        public static int buy_limit_min_th = 60;
        public static int buy_market_min_th = 60;

        public static double max_quantity_buy = 0;
        public static double max_quantity_sell = 0;

        public static int future_leverage = 0;
    }
}
