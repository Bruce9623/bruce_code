using Binance.Net;
using Binance.Net.Enums;
using Binance.Net.Interfaces;
using Binance.Net.Objects.Futures.FuturesData;
using Binance.Net.Objects.Futures.MarketData;
using Binance.Net.Objects.Futures.UserStream;
using Binance.Net.Objects.Spot;
using Binance.Net.Objects.Spot.MarketData;
using Binance.Net.Objects.Spot.SpotData;
using Binance.Net.Objects.Spot.UserStream;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Logging;
using CryptoExchange.Net.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BNAAutoTrader
{
    public partial class MainForm : Form
    {

        private string StreamListenKey = "";

        private double QtyValue;

        private double ManualQtyValue;

        private DateTime gdtKeepAliveTime = DateTime.MinValue;
        public MainForm()
        {
            InitializeComponent();

            this.comboType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboSymbol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboRT.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboPeriod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lstLog.SelectionMode = SelectionMode.MultiExtended;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Global.log = new Log(this.lstLog, this);
            Global.tAl = new TradeAlgo(this);
            Global.type_status = TYPE.FUTURES;
            Global.future_leverage = int.Parse(this.txtLeverage.Text);
            SetEnableDisableManualBtn(true);
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (Global.connection_status == CONNECTION.DISCONNECTED)
            { 
                Global.connection_status = CONNECTION.CONNECTING;
                this.btnConnect.Text = Properties.Resources.CONNECTING;
                ConnectWallet();
            }
            else if (Global.connection_status == CONNECTION.CONNECTED)
            {
                DisconnectWallet();
                Global.connection_status = CONNECTION.DISCONNECTED;
                this.btnConnect.Text = Properties.Resources.CONNECT;
                Global.log.addLog("Disconnected Successfully (" + this.comboRT.Text + ")");
                SetEnableDisableCombobox(true);
            }

        }

        private void ConnectWallet()
        {
            if (!SetParamFromUI())
                return;

            this.btnConnect.Enabled = false;

            if (CreateClient())
            {
                if (Global.type_status == TYPE.SPOT)
                    Global.log.addLog(Properties.Resources.SPOT + " Connected Successfully (" + this.comboSymbol.Text + "-" + this.comboRT.Text + ")");
                else if (Global.type_status == TYPE.MARGIN)
                    Global.log.addLog(Properties.Resources.MARGIN + " Connected Successfully (" + this.comboSymbol.Text + "-" + this.comboRT.Text + ")");
                else if (Global.type_status == TYPE.FUTURES)
                    Global.log.addLog(Properties.Resources.FUTURES + " Connected Successfully (" + this.comboSymbol.Text + "-" + this.comboRT.Text + ")");

                Global.connection_status = CONNECTION.CONNECTED;
                this.btnConnect.Text = Properties.Resources.DISCONNECT;

                Thread th = new Thread(CheckKeepAlive);
                th.IsBackground = true;
                th.Start();
                SetEnableDisableCombobox(false);
            }
            else
            {
                string error_msg = "";
                if (Global.type_status == TYPE.SPOT)
                    error_msg = Properties.Resources.SPOT + " Connection Error";
                else if (Global.type_status == TYPE.MARGIN)
                    error_msg = Properties.Resources.MARGIN + " Connection Error";
                else if (Global.type_status == TYPE.FUTURES)
                    error_msg = Properties.Resources.FUTURES + " Connection Error";

                MessageBox.Show(error_msg);
                Global.log.addLog(error_msg); 
                Global.connection_status = CONNECTION.DISCONNECTED;
                this.btnConnect.Text = Properties.Resources.CONNECT;
                SetEnableDisableCombobox(true);
            }
            this.btnConnect.Enabled = true;
        }

        private bool CreateClient()
        {
            BinanceClient.SetDefaultOptions(new BinanceClientOptions()
            {
                ApiCredentials = new ApiCredentials(Global.apiKey, Global.securityKey),
            });
            BinanceSocketClient.SetDefaultOptions(new BinanceSocketClientOptions()
            {
                ApiCredentials = new ApiCredentials(Global.apiKey, Global.securityKey),
            });

            if (Global.wallet_status == WALLET_TYPE.REAL)
            {
                Global.client = new BinanceClient();
                Global.socketClient = new BinanceSocketClient();
            }
            else
            {
                Global.client = new BinanceClient(new BinanceClientOptions(Global.SPOT_TEST_BASEADDRESS, Global.FUTURE_TEST_USDTADDRESS, Global.FUTURE_TEST_USDTADDRESS)
                {
                    ApiCredentials = new ApiCredentials(Global.apiKey, Global.securityKey),
                });
                Global.socketClient = new BinanceSocketClient(new BinanceSocketClientOptions(Global.SPOTWSS_TEST_BASEADDRESS, Global.FUTUREWSS_TEST_USDTADDRESS, Global.FUTUREWSS_TEST_COINADDRESS)
                {
                    ApiCredentials = new ApiCredentials(Global.apiKey, Global.securityKey),
                });
            }

            if (Global.type_status == TYPE.SPOT)
            { 
                var startSpotResult = Global.client.Spot.UserStream.StartUserStream();

                if (!startSpotResult.Success)
                {
                    StreamListenKey = "";
                    Global.log.addLog("Invalid API Key or Security Key");
                    return false;
                }

                StreamListenKey = startSpotResult.Data;
                               
                var result = Global.client.Spot.Order.GetOpenOrders(Global.symbol);
                var BookPrice = Global.client.Spot.Market.GetBookPrice(Global.symbol);
                if (BookPrice.Success)
                {
                    Global.current_rate.ask_value = (double)BookPrice.Data.BestAskPrice;
                    Global.current_rate.bid_value = (double)BookPrice.Data.BestBidPrice;
                    updateCurRateLabel();
                }
                var kline_result = Global.client.Spot.Market.GetKlines(Global.symbol, Global.timeInterval);
                int index;
                if (kline_result.Success)
                {
                    IBinanceKline ibk;
                    for (index = kline_result.Data.Count() - 61; index < kline_result.Data.Count() - 1; index++)
                    {
                        CandleData cd = new CandleData();
                        ibk = kline_result.Data.ToList()[index];
                        cd.close_value = (double)ibk.Close;
                        cd.open_value = (double)ibk.Open;
                        cd.low_value = (double)ibk.Low;
                        cd.high_value = (double)ibk.High;
                        cd.open_datetime = ibk.OpenTime;
                        cd.volume_value = (double)ibk.BaseVolume;
                        Global.candle_array.Enqueue(cd);
                    }
                }

                WebCallResult<BinanceAccountInfo> accountInfo = Global.client.General.GetAccountInfo();
                bool bFirst = true;
                for (int i = 0; i < accountInfo.Data.Balances.Count(); i++)
                {
                    BinanceBalance balance = accountInfo.Data.Balances.ElementAt(i);
                    if (Global.symbol.Contains(balance.Asset))
                    {
                        if (bFirst)
                        {
                            Global.spot_balance1 = balance;
                            bFirst = false;
                        }
                        else
                            Global.spot_balance2 = balance;

                        if (balance.Total > 0)
                        {

                            Global.log.addLog($"Balance {balance.Asset} Free={balance.Free}, Total={balance.Total}, Locked = {balance.Locked}");
                        }
                    }
                }

                Global.socketClient.Spot.SubscribeToKlineUpdates(Global.symbol, Global.timeInterval,
                    message =>
                    {
                        if (Global.candle_data.open_value != 0 && Global.candle_data.open_datetime != message.Data.OpenTime)
                        {
                            CandleData cdTemp = new CandleData();
                            cdTemp.open_value = Global.candle_data.open_value;
                            cdTemp.close_value = Global.candle_data.close_value;
                            cdTemp.high_value = Global.candle_data.high_value;
                            cdTemp.low_value = Global.candle_data.low_value;
                            cdTemp.open_datetime = Global.candle_data.open_datetime;
                            cdTemp.volume_value = Global.candle_data.volume_value;
                            String strCandleData = $"[Open {cdTemp.open_value}, Close {cdTemp.close_value}, High {cdTemp.high_value}, Low {cdTemp.low_value}, OpenDateTime {cdTemp.open_datetime}]";
                            Global.log.addLog(strCandleData);
                            Global.candle_array.Dequeue();
                            Global.candle_array.Enqueue(cdTemp);
                        }
                        if (message.Data.Open > 0)
                            Global.candle_data.open_value = (double)message.Data.Open;
                        if (message.Data.Close > 0)
                            Global.candle_data.close_value = (double)message.Data.Close;
                        if (message.Data.High > 0)
                            Global.candle_data.high_value = (double)message.Data.High;
                        if (message.Data.Low > 0)
                            Global.candle_data.low_value = (double)message.Data.Low;
                        if (message.Data.QuoteVolume > 0)
                            Global.candle_data.volume_value = (double)message.Data.BaseVolume;
                        Global.candle_data.open_datetime = message.Data.OpenTime;
                    });

                Global.socketClient.Spot.SubscribeToBookTickerUpdates(Global.symbol, data =>
                {
                    // Handle data
                    if (data.BestAskPrice > 0)
                        Global.current_rate.ask_value = (double)data.BestAskPrice;
                    if (data.BestAskQuantity > 0)
                        Global.current_rate.ask_quantity_value = (double)data.BestAskQuantity;
                    if (data.BestBidPrice > 0)
                        Global.current_rate.bid_value = (double)data.BestBidPrice;
                    if (data.BestBidQuantity > 0)
                        Global.current_rate.bid_quantity_value = (double)data.BestBidQuantity;

//                    Global.log.addLog(Global.current_rate.ask_value + "-" + Global.current_rate.bid_value);

                    Global.current_rate.updatetime = DateTime.Now;
                    updateCurRateLabel();

                });
                
                WebCallResult<IEnumerable<BinanceOrder>> ordersList = Global.client.Spot.Order.GetAllOrders(Global.symbol);
                Global.socketClient.Spot.SubscribeToUserDataUpdates(StreamListenKey,
                        orderUpdate =>
                        { // Handle order update
                            String strOrderUpdate = $"Spot orderUpdate {orderUpdate.Status}, Price {orderUpdate.Price}, Quantity {orderUpdate.Quantity}, QuantityFilled {orderUpdate.QuantityFilled}, LastPriceFilled {orderUpdate.LastPriceFilled}, Commission {orderUpdate.Commission}";

                            if (orderUpdate.Status == OrderStatus.Filled || orderUpdate.Status == OrderStatus.PartiallyFilled)
                            {
                             //   Global.log.addLog(strOrderUpdate);
                                updateFilledPrice(orderUpdate.LastPriceFilled);
                            }
                            else
                                Console.WriteLine(strOrderUpdate);

                            if (orderUpdate.Status == OrderStatus.Expired)
                                updateFilledPrice(0);
                        },
                        ocoUpdate =>
                        { // Handle oco order update
                            Console.WriteLine($"Spot ocoUpdate {ocoUpdate}");
                        },
                        positionUpdate =>
                        { // Handle account position update
                        for (int i = 0; i < positionUpdate.Balances.Count(); i++)
                            {
                                BinanceStreamBalance balance = positionUpdate.Balances.ElementAt(i);
                                if (Global.spot_balance1.Asset == balance.Asset)
                                {
                                    Global.log.addLog($"{Global.spot_balance1.Asset} Free {Global.spot_balance1.Free}=>{balance.Free}, Locked {Global.spot_balance1.Locked}=>{balance.Locked}");
                                    Global.spot_balance1.Free = balance.Free;
                                    Global.spot_balance1.Locked = balance.Locked;
                                }
                                if (Global.spot_balance2.Asset == balance.Asset)
                                {
                                    Global.log.addLog($"{Global.spot_balance2.Asset} Free {Global.spot_balance2.Free}=>{balance.Free}, Locked {Global.spot_balance2.Locked}=>{balance.Locked}");
                                    Global.spot_balance2.Free = balance.Free;
                                    Global.spot_balance2.Locked = balance.Locked;
                                }
                            }
                            Console.WriteLine($"Spot positionUpdate {positionUpdate}");
                        },
                        balanceUpdate =>
                        { // Handle balance update
                            Console.WriteLine($"Spot balanceUpdate {balanceUpdate}");
                        });
            }
            else if (Global.type_status == TYPE.MARGIN)
            {
                var startMarginResult = Global.client.Margin.UserStream.StartUserStream();
                if (!startMarginResult.Success)
                {
                    StreamListenKey = "";
                    Global.log.addLog("Invalid API Key or Security Key");
                    return false;
                }

                StreamListenKey = startMarginResult.Data;

                var result = Global.client.Margin.Order.GetOpenMarginAccountOrders(Global.symbol);
                var BookPrice = Global.client.Margin.Market.GetMarginPriceIndex(Global.symbol);
                if (BookPrice.Success)
                {
                    Global.current_rate.ask_value = (double)BookPrice.Data.Price;
                    Global.current_rate.bid_value = (double)BookPrice.Data.Price;
                    updateCurRateLabel();
                }

                WebCallResult<BinanceAccountInfo> accountInfo = Global.client.General.GetAccountInfo();
                bool bFirst = true;
                for (int i = 0; i < accountInfo.Data.Balances.Count(); i++)
                {
                    BinanceBalance balance = accountInfo.Data.Balances.ElementAt(i);
                    if (Global.symbol.Contains(balance.Asset))
                    {
                        if (bFirst)
                        {
                            Global.spot_balance1 = balance;
                            bFirst = false;
                        }
                        else
                            Global.spot_balance2 = balance;

                        if (balance.Total > 0)
                        {
                             Global.log.addLog($"Balance {balance.Asset} Free={balance.Free}, Total={balance.Total}, Locked = {balance.Locked}");
                        }
                    }
                }
                Global.socketClient.Spot.SubscribeToBookTickerUpdates(Global.symbol, data =>
                {
                    // Handle data
                    if (data.BestAskPrice > 0)
                        Global.current_rate.ask_value = (double)data.BestAskPrice;
                    if (data.BestAskQuantity > 0)
                        Global.current_rate.ask_quantity_value = (double)data.BestAskQuantity;
                    if (data.BestBidPrice > 0)
                        Global.current_rate.bid_value = (double)data.BestBidPrice;
                    if (data.BestBidQuantity > 0)
                        Global.current_rate.bid_quantity_value = (double)data.BestBidQuantity;

                    Global.current_rate.updatetime = DateTime.Now;
                    updateCurRateLabel();
                });
                var kline_result = Global.client.Spot.Market.GetKlines(Global.symbol, Global.timeInterval);
                int index;
                if (kline_result.Success)
                {
                    IBinanceKline ibk;
                    for (index = kline_result.Data.Count() - 61; index < kline_result.Data.Count() - 1; index++)
                    {
                        CandleData cd = new CandleData();
                        ibk = kline_result.Data.ToList()[index];
                        cd.close_value = (double)ibk.Close;
                        cd.open_value = (double)ibk.Open;
                        cd.low_value = (double)ibk.Low;
                        cd.high_value = (double)ibk.High;
                        cd.open_datetime = ibk.OpenTime;
                        cd.volume_value = (double)ibk.BaseVolume;
                        Global.candle_array.Enqueue(cd);
                    }

                }
                Global.socketClient.Spot.SubscribeToKlineUpdates(Global.symbol, Global.timeInterval,
                    message =>
                    {
                        if (Global.candle_data.open_value != 0 && Global.candle_data.open_datetime != message.Data.OpenTime)
                        {
                            CandleData cdTemp = new CandleData();
                            cdTemp.open_value = Global.candle_data.open_value;
                            cdTemp.close_value = Global.candle_data.close_value;
                            cdTemp.high_value = Global.candle_data.high_value;
                            cdTemp.low_value = Global.candle_data.low_value;
                            cdTemp.open_datetime = Global.candle_data.open_datetime;
                            cdTemp.volume_value = Global.candle_data.volume_value;
                            String strCandleData = $"[Open {cdTemp.open_value}, Close {cdTemp.close_value}, High {cdTemp.high_value}, Low {cdTemp.low_value}, OpenDateTime {cdTemp.open_datetime}]";
                            Global.log.addLog(strCandleData);
                            Global.candle_array.Dequeue();
                            Global.candle_array.Enqueue(cdTemp);
                        }
                        if (message.Data.Open > 0)
                            Global.candle_data.open_value = (double)message.Data.Open;
                        if (message.Data.Close > 0)
                            Global.candle_data.close_value = (double)message.Data.Close;
                        if (message.Data.High > 0)
                            Global.candle_data.high_value = (double)message.Data.High;
                        if (message.Data.Low > 0)
                            Global.candle_data.low_value = (double)message.Data.Low;
                        if (message.Data.QuoteVolume > 0)
                            Global.candle_data.volume_value = (double)message.Data.BaseVolume;
                        Global.candle_data.open_datetime = message.Data.OpenTime;
                    });

                WebCallResult<IEnumerable<BinanceOrder>> ordersList = Global.client.Margin.Order.GetAllMarginAccountOrders(Global.symbol);
                Global.socketClient.Spot.SubscribeToUserDataUpdates(StreamListenKey,
                        orderUpdate =>
                        { // Handle order update
                            String strOrderUpdate = $"Spot orderUpdate {orderUpdate.Status}, Price {orderUpdate.Price}, Quantity {orderUpdate.Quantity}, QuantityFilled {orderUpdate.QuantityFilled}, LastPriceFilled {orderUpdate.LastPriceFilled}, Commission {orderUpdate.Commission}";

                            if (orderUpdate.Status == OrderStatus.Filled || orderUpdate.Status == OrderStatus.PartiallyFilled)
                            {
                                   Global.log.addLog(strOrderUpdate);
                                updateFilledPrice(orderUpdate.LastPriceFilled);
                            }
                            else
                                Console.WriteLine(strOrderUpdate);

                            if (orderUpdate.Status == OrderStatus.Expired)
                                updateFilledPrice(0);
                        },
                        ocoUpdate =>
                        { // Handle oco order update
                        //    Console.WriteLine($"Spot ocoUpdate {ocoUpdate}");
                        },
                        positionUpdate =>
                        { // Handle account position update
                            for (int i = 0; i < positionUpdate.Balances.Count(); i++)
                            {
                                BinanceStreamBalance balance = positionUpdate.Balances.ElementAt(i);
                                if (Global.spot_balance1.Asset == balance.Asset)
                                {
                                    Global.log.addLog($"{Global.spot_balance1.Asset} Free {Global.spot_balance1.Free}=>{balance.Free}, Locked {Global.spot_balance1.Locked}=>{balance.Locked}");
                                    Global.spot_balance1.Free = balance.Free;
                                    Global.spot_balance1.Locked = balance.Locked;
                                }
                                if (Global.spot_balance2.Asset == balance.Asset)
                                {
                                    Global.log.addLog($"{Global.spot_balance2.Asset} Free {Global.spot_balance2.Free}=>{balance.Free}, Locked {Global.spot_balance2.Locked}=>{balance.Locked}");
                                    Global.spot_balance2.Free = balance.Free;
                                    Global.spot_balance2.Locked = balance.Locked;
                                }
                            }
                            Console.WriteLine($"Spot positionUpdate {positionUpdate}");
                        },
                        balanceUpdate =>
                        { // Handle balance update
                            Console.WriteLine($"Spot balanceUpdate {balanceUpdate}");
                        });
            }
            else if (Global.type_status == TYPE.FUTURES)
            {
                var startFuturesResult = Global.client.FuturesUsdt.UserStream.StartUserStream();
                if (!startFuturesResult.Success)
                {
                    StreamListenKey = "";
                    Global.log.addLog("Invalid API Key or Security Key");
                    return false;
                }

                StreamListenKey = startFuturesResult.Data;

                var result = Global.client.FuturesUsdt.Order.GetOpenOrders(Global.symbol);
                var BookPrice = Global.client.FuturesUsdt.Market.GetBookPrices(Global.symbol);
                if (BookPrice.Success && BookPrice.Data.Count() > 0)
                {
                    Global.current_rate.ask_value = (double)BookPrice.Data.ElementAt(0).BestAskPrice;
                    Global.current_rate.bid_value = (double)BookPrice.Data.ElementAt(0).BestBidPrice;
                    updateCurRateLabel();
                }
                else
                {
                    Global.log.addLog($"No Symbol {Global.symbol} at FUTURES");
                    return false;
                }


                WebCallResult<IEnumerable<BinanceFuturesMarkPrice>> interestRate = Global.client.FuturesUsdt.Market.GetMarkPrices(Global.symbol);
                if (interestRate.Data != null && interestRate.Data.Count() != 0)
                { 
//                  Global.current_rate.interset_rate_value = (double)interestRate.Data.ElementAt(0).InterestRate;
                    Global.current_rate.founding_rate_value = (double)interestRate.Data.ElementAt(0).FundingRate;
                    Global.current_rate.next_foundig_time = interestRate.Data.ElementAt(0).NextFundingTime;
                }

                WebCallResult<BinanceFuturesAccountInfo> futureAccountInfo = Global.client.FuturesUsdt.Account.GetAccountInfo();
                for (int i = 0; i < futureAccountInfo.Data.Assets.Count(); i++)
                {
                    BinanceFuturesAccountAsset asset = futureAccountInfo.Data.Assets.ElementAt(i);
                    if (Global.symbol.EndsWith(asset.Asset))
                    {
                        Global.future_asset = asset.Asset;
                        Global.future_balance = asset.CrossWalletBalance;
                    }

                    if (asset.AvailableBalance > 0)
                    {
                        Global.log.addLog($"Future Balance {asset.Asset} Available={asset.AvailableBalance}");
                    }
                }

                for (int i = 0; i < futureAccountInfo.Data.Positions.Count(); i++)
                {
                    BinancePositionInfoUsdt position = futureAccountInfo.Data.Positions.ElementAt(i);
                    if (position.Symbol == Global.symbol)
                    {
                        Global.future_position = position;
                    }
                }
                
                var leverage_result =  Global.client.FuturesUsdt.ChangeInitialLeverage(Global.symbol, Global.future_leverage);
                if (!leverage_result.Success)
                {
                    Global.log.addLog("Set Leverage Failure");
                    return false;
                }
                var kline_result = Global.client.FuturesUsdt.Market.GetKlines(Global.symbol, Global.timeInterval);
                int index;
                if (kline_result.Success)
                {
                    IBinanceKline ibk;
                    for (index = kline_result.Data.Count() - 61; index < kline_result.Data.Count() - 1 ; index++)
                    {
                        CandleData cd = new CandleData();
                        ibk = kline_result.Data.ToList()[index];
                        cd.close_value = (double)ibk.Close;
                        cd.open_value = (double)ibk.Open;
                        cd.low_value = (double)ibk.Low;
                        cd.high_value = (double)ibk.High;
                        cd.open_datetime = ibk.OpenTime;
                        cd.volume_value = (double)ibk.BaseVolume;
                        Global.candle_array.Enqueue(cd);
                    }

                }
                Global.socketClient.FuturesUsdt.SubscribeToBookTickerUpdates(Global.symbol, data =>
                {
                    if (data.BestAskPrice > 0)
                        Global.current_rate.ask_value = (double)data.BestAskPrice;
                    if (data.BestAskQuantity > 0)
                        Global.current_rate.ask_quantity_value = (double)data.BestAskQuantity;
                    if (data.BestBidPrice > 0)
                        Global.current_rate.bid_value = (double)data.BestBidPrice;
                    if (data.BestBidQuantity > 0)
                        Global.current_rate.bid_quantity_value = (double)data.BestBidQuantity;
                    updateCurRateLabel();
                    Global.current_rate.updatetime = DateTime.Now;
                    Global.max_quantity_buy = (double)Global.future_balance / (Global.current_rate.ask_value / Global.future_leverage);
                    Global.max_quantity_sell = (double)Global.future_balance / (Global.current_rate.bid_value / Global.future_leverage);
                });
                Global.socketClient.FuturesUsdt.SubscribeToKlineUpdates(Global.symbol, Global.timeInterval,
                    message =>
                    {
                        if (Global.candle_data.open_value != 0 && Global.candle_data.open_datetime != message.Data.OpenTime)
                        {
                            CandleData cdTemp = new CandleData();
                            cdTemp.open_value = Global.candle_data.open_value;
                            cdTemp.close_value = Global.candle_data.close_value;
                            cdTemp.high_value = Global.candle_data.high_value;
                            cdTemp.low_value = Global.candle_data.low_value;
                            cdTemp.open_datetime = Global.candle_data.open_datetime;
                            cdTemp.volume_value = Global.candle_data.volume_value;
                            String strCandleData = $"[Open {cdTemp.open_value}, Close {cdTemp.close_value}, High {cdTemp.high_value}, Low {cdTemp.low_value}, OpenDateTime {cdTemp.open_datetime}]";
                            Global.log.addLog(strCandleData);
                            Global.candle_array.Dequeue();
                            Global.candle_array.Enqueue(cdTemp);
                        }
                        if (message.Data.Open > 0)
                            Global.candle_data.open_value = (double)message.Data.Open;
                        if (message.Data.Close > 0)
                            Global.candle_data.close_value = (double)message.Data.Close;
                        if (message.Data.High > 0)
                            Global.candle_data.high_value = (double)message.Data.High;
                        if (message.Data.Low > 0)
                            Global.candle_data.low_value = (double)message.Data.Low;
                        if (message.Data.QuoteVolume > 0)
                            Global.candle_data.volume_value = (double)message.Data.BaseVolume;
                        Global.candle_data.open_datetime = message.Data.OpenTime;
                    });

                Global.socketClient.FuturesUsdt.SubscribeToMarkPriceUpdates(Global.symbol, 1000, data => {
                    Global.current_rate.founding_rate_value = (double)data.FundingRate;
                    Global.current_rate.next_foundig_time = data.NextFundingTime;

                    Global.current_rate.updatetime= DateTime.Now;

                    Global.current_rate.timespan_tonext = Global.current_rate.next_foundig_time - data.EventTime;
                });

                Global.socketClient.FuturesUsdt.SubscribeToUserDataUpdates(StreamListenKey,
                    leverageUpdate =>
                    {
                        Global.future_leverage = leverageUpdate.UpdateData.Leverage;
                        Console.WriteLine($"Future leverageUpdate {leverageUpdate}");
                    },
                    marginUpdate =>
                    {
                        Console.WriteLine($"Future marginUpdate {marginUpdate}");
                    },
                    accountUpdate =>
                    {
                        Console.WriteLine($"Future accountUpdate {accountUpdate}");
                        for (int i = 0; i < accountUpdate.UpdateData.Balances.Count(); i++)
                        {
                            BinanceFuturesStreamBalance balance = accountUpdate.UpdateData.Balances.ElementAt(i);
                            if (Global.symbol.EndsWith(balance.Asset))
                            {
                                Global.future_balance = balance.CrossBalance;
                            }
                        }

                        for (int i = 0; i < accountUpdate.UpdateData.Positions.Count(); i++)
                        {
                            BinanceFuturesStreamPosition position = accountUpdate.UpdateData.Positions.ElementAt(i);
                            if (position.Symbol == Global.symbol)
                            {
                                Global.future_position.Symbol = position.Symbol;
                                Global.future_position.PositionSide = position.PositionSide;
                                Global.future_position.EntryPrice = position.EntryPrice;
                                Global.future_position.PositionAmount = position.PositionAmount;
//                                Global.log.addLog($"Future Position {position.Symbol}, {position.PositionSide}, {position.PositionAmount}, {position.EntryPrice}");
                            }
                        }
                    },
                    orderUpdate =>
                    {
                        String strOrderUpdate = $"Future orderUpdate {orderUpdate.UpdateData.Status}, AvgPrice {orderUpdate.UpdateData.AveragePrice}, Quantity {orderUpdate.UpdateData.Quantity}, QuantityFilled {orderUpdate.UpdateData.QuantityOfLastFilledTrade}, LastPriceFilled {orderUpdate.UpdateData.PriceLastFilledTrade}, Commission {orderUpdate.UpdateData.Commission}";

                        if (orderUpdate.UpdateData.Status == OrderStatus.Filled || orderUpdate.UpdateData.Status == OrderStatus.PartiallyFilled)
                        {
                            Global.log.addLog(strOrderUpdate);
                            updateFilledPrice(orderUpdate.UpdateData.PriceLastFilledTrade);
                        }
                        else
                        {
                          Global.log.addLog(strOrderUpdate);
                        //Console.WriteLine(strOrderUpdate);
                    }
                    },
                    listenkeyExpired =>
                    {
                        Global.log.addLog($"Future listenkeyExpired");
                    });

            }

            return true;
        }

        private void DisconnectWallet()
        {
            Global.socketClient.UnsubscribeAll();
            Global.client = null;
            Global.candle_array.Clear();
            Global.candle_data = new CandleData();
        }
        private bool SetParamFromUI()
        {
            if (this.txtApiKey.Text == "" || this.txtSecuKey.Text == "" || this.comboSymbol.Text == "" || this.comboType.Text == "")
                return false;

            Global.apiKey = this.txtApiKey.Text;
            Global.securityKey = this.txtSecuKey.Text;
            Global.symbol = this.comboSymbol.Text;
            if (this.comboType.Text == Properties.Resources.SPOT)
                Global.type_status = TYPE.SPOT;
            else if (this.comboType.Text == Properties.Resources.MARGIN)
                Global.type_status = TYPE.MARGIN;
            else if (this.comboType.Text == Properties.Resources.FUTURES)
                Global.type_status = TYPE.FUTURES;

            if (this.comboRT.Text == Properties.Resources.TEST)
                Global.wallet_status = WALLET_TYPE.TESTNET;
            else if (this.comboRT.Text == Properties.Resources.REAL)
                Global.wallet_status = WALLET_TYPE.REAL;

            QtyValue = Double.Parse(this.txtQty.Text.ToString());
            ManualQtyValue = Double.Parse(this.txtManulQty.Text.ToString());
            return true;
        }

        private bool checkSellable()
        {
            double qty_value = 0;
            if (Global.allow_auto_trade)
                qty_value = QtyValue;
            else
                qty_value = ManualQtyValue;

            if (Global.type_status == TYPE.SPOT || Global.type_status == TYPE.MARGIN)
            { 
                if (Global.symbol.StartsWith(Global.spot_balance1.Asset))
                    return Global.spot_balance1.Free > 0;

                if (Global.symbol.StartsWith(Global.spot_balance2.Asset))
                    return Global.spot_balance1.Free > 0;
            }
            else if (Global.type_status == TYPE.FUTURES)
            {
                if (qty_value > Global.max_quantity_sell)
                    return false;
                return true;
            }
            return false;
        }
        private bool checkBuyable()
        {
            double qty_value = 0;
            if (Global.allow_auto_trade)
                qty_value = QtyValue;
            else
                qty_value = ManualQtyValue;

            if (Global.type_status == TYPE.SPOT || Global.type_status == TYPE.MARGIN)
            {
                decimal cash_quantity = (decimal)(qty_value * Global.current_rate.ask_value /** Utils.g_settings.usdtbuffer *// 100);
                if (Global.symbol.EndsWith(Global.spot_balance1.Asset))
                    return Global.spot_balance1.Free >= cash_quantity;

                if (Global.symbol.EndsWith(Global.spot_balance2.Asset))
                    return Global.spot_balance2.Free >= cash_quantity;

            }
            else if (Global.type_status == TYPE.FUTURES)
            {
                if (qty_value > Global.max_quantity_sell)
                    return false;
                return true;
            }
            return false;
        }
        private void updateFilledPrice(decimal dPrice)
        {
//            gFilledFuturePrice = dPrice;
//            gReceivedFutureOrder = true;
//            gFutureOrderFilled = DateTime.Now;
//            checkOrderResult();
        }

        private void updateCurRateLabel()
        {
            if (labCuAskPrice.InvokeRequired)
            {
                labCuAskPrice.Invoke((Action)(() => updateCurRateLabel()));
                return;
            }
            labCuAskPrice.Text = Global.current_rate.ask_value.ToString();
            if (labCuBidPrice.InvokeRequired)
            {
                labCuBidPrice.Invoke((Action)(() => updateCurRateLabel()));
                return;
            }
            labCuBidPrice.Text = Global.current_rate.bid_value.ToString();
        }
        async public void placeMarketOrder(OrderSide orderSide)
        {
            GetOrderParamFronUI();
            double qty_value;
            if (Global.trade_mth == TRADE_METHOD.AUTO)
                qty_value = QtyValue;
            else
                qty_value = ManualQtyValue;
            if ( (orderSide == OrderSide.Buy && !checkBuyable()) || (orderSide == OrderSide.Sell && !checkSellable()))
            {
                Global.log.addLog("Not Enough Balance");
                MessageBox.Show("Not Enough Balance");
                return;
            }
            Global.order_condition = false;
            if (Global.type_status == TYPE.SPOT)
            {
                Global.log.addLog($"{Properties.Resources.SPOT} Market Order {Global.symbol}, {orderSide}, {(orderSide == OrderSide.Buy ? Global.current_rate.ask_value : Global.current_rate.bid_value)}, {qty_value}");
                var result = await Global.client.Spot.Order.PlaceOrderAsync(Global.symbol, orderSide, OrderType.Market, quantity: (decimal)qty_value);

                if (result.Success == false)
                    Global.log.addLog($"{Properties.Resources.SPOT} Market Order failed {result.Error}");
                else
                    Global.log.addLog($"{Properties.Resources.SPOT} Market Order Success");
            }
            else if (Global.type_status == TYPE.MARGIN)
            {
                Global.log.addLog($"{Properties.Resources.MARGIN} Market Order {Global.symbol}, {orderSide}, {(orderSide == OrderSide.Buy ? Global.current_rate.ask_value : Global.current_rate.bid_value)}, {qty_value}");
                var result = Global.client.Margin.Order.PlaceMarginOrder(Global.symbol, orderSide, OrderType.Market, quantity: (decimal)qty_value);

                if (result.Success == false)
                    Global.log.addLog($"{Properties.Resources.MARGIN} Market Order failed {result.Error}");
                else
                    Global.log.addLog($"{Properties.Resources.MARGIN} Market Order Success");
            }
            else if (Global.type_status == TYPE.FUTURES)
            {
                Global.log.addLog($"{Properties.Resources.FUTURES} Market Order {Global.symbol}, {orderSide}, {(orderSide == OrderSide.Buy ? Global.current_rate.ask_value : Global.current_rate.bid_value)}, {qty_value}");
                var result = await Global.client.FuturesUsdt.Order.PlaceOrderAsync(Global.symbol, orderSide, OrderType.Market, quantity: (decimal)qty_value);

                if (result.Success == false)
                    Global.log.addLog($"{Properties.Resources.FUTURES} Market Order failed {result.Error}");
                else
                    Global.log.addLog($"{Properties.Resources.FUTURES} Market Order Success");
            }
            Global.order_condition = true;
        }

        async public void placeLimitOrder(OrderSide orderSide)
        {
            GetOrderParamFronUI();
            if ((orderSide == OrderSide.Buy && Global.buy_limit_price == 0) || (orderSide == OrderSide.Sell && Global.sell_limit_price == 0))
            {
                MessageBox.Show("Please input limit price");
                return;
            }
            if ((orderSide == OrderSide.Buy && !checkBuyable()) || (orderSide == OrderSide.Sell && !checkSellable()))
            {
                Global.log.addLog("Not Enough Balance");
                MessageBox.Show("Not Enough Balance");
                return;
            }

            Global.order_condition = false;
            double price = orderSide == OrderSide.Sell ? Global.sell_limit_price : Global.buy_limit_price;
            double qty_value;
            if (Global.trade_mth == TRADE_METHOD.AUTO)
                qty_value = QtyValue;
            else
                qty_value = ManualQtyValue;
            if (Global.type_status == TYPE.SPOT)
            {
                Global.log.addLog($"{Properties.Resources.SPOT} Limit Order {Global.symbol}, {orderSide}, {price}, {qty_value}");

                var result = await Global.client.Spot.Order.PlaceOrderAsync(Global.symbol, orderSide, OrderType.Limit, quantity: (decimal)qty_value, price:(decimal)price, timeInForce: TimeInForce.GoodTillCancel);

                if (result.Success == false)
                    Global.log.addLog($"{Properties.Resources.SPOT} Limit Order failed {result.Error}");
                else
                    Global.log.addLog($"{Properties.Resources.SPOT} Limit Order Success");
            }
            else if (Global.type_status == TYPE.MARGIN)
            {
                Global.log.addLog($"{Properties.Resources.MARGIN} Limit Order {Global.symbol}, {orderSide}, {price}, {qty_value}");
                var result = Global.client.Margin.Order.PlaceMarginOrder(Global.symbol, orderSide, OrderType.Limit, quantity: (decimal)qty_value, price: (decimal)price, timeInForce: TimeInForce.GoodTillCancel);

                if (result.Success == false)
                    Global.log.addLog($"{Properties.Resources.MARGIN} Limit Order failed {result.Error}");
                else
                    Global.log.addLog($"{Properties.Resources.MARGIN} Limit Order Success");
            }
            else if (Global.type_status == TYPE.FUTURES)
            {
                Global.log.addLog($"{Properties.Resources.FUTURES} Limit Order {Global.symbol}, {orderSide}, {price}, {qty_value}");
                var result = await Global.client.FuturesUsdt.Order.PlaceOrderAsync(Global.symbol, orderSide, OrderType.Limit, quantity: (decimal)qty_value, timeInForce: TimeInForce.GoodTillCancel,price: (decimal)price);

                if (result.Success == false)
                    Global.log.addLog($" {Properties.Resources.FUTURES} Limit Order failed {result.Error}");
                else
                    Global.log.addLog($"{Properties.Resources.FUTURES} Limit Order Success");
            }
            Global.order_condition = true;
        }
        private void comboPeriod_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            switch (comboBox.Text)
            {
                case "1m":
                    Global.timeInterval = KlineInterval.OneMinute;
                    break;
                case "3m":
                    Global.timeInterval = KlineInterval.ThreeMinutes;
                    break;
                case "5m":
                    Global.timeInterval = KlineInterval.FiveMinutes;
                    break;
                case "15m":
                    Global.timeInterval = KlineInterval.FifteenMinutes;
                    break;
                case "30m":
                    Global.timeInterval = KlineInterval.ThirtyMinutes;
                    break;
                case "1h":
                    Global.timeInterval = KlineInterval.OneHour;
                    break;
                case "2h":
                    Global.timeInterval = KlineInterval.TwoHour;
                    break;
                case "4h":
                    Global.timeInterval = KlineInterval.FourHour;
                    break;
                case "6h":
                    Global.timeInterval = KlineInterval.SixHour;
                    break;
                case "8h":
                    Global.timeInterval = KlineInterval.EightHour;
                    break;
                case "12h":
                    Global.timeInterval = KlineInterval.TwelveHour;
                    break;
                case "1d":
                    Global.timeInterval = KlineInterval.OneDay;
                    break;
                case "3d":
                    Global.timeInterval = KlineInterval.ThreeDay;
                    break;
                case "1w":
                    Global.timeInterval = KlineInterval.OneWeek;
                    break;
                case "1M":
                    Global.timeInterval = KlineInterval.OneMonth;
                    break;

            }
        }

        private void comboSymbol_SelectedIndexChanged(object sender, EventArgs e)
        {
            Global.symbol = sender.ToString();
        }

        private void comboType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            if (Global.allow_auto_trade)
            {
                comboBox.Text = Properties.Resources.FUTURES;
                return;
            }
            switch (comboBox.Text)
            {
                case "SPOT":
                    Global.type_status = TYPE.SPOT;
                    SetEnableDisableManualBtn(Global.allow_auto_trade ? false : true);
                    break;
                case "MARGIN":
                    Global.type_status = TYPE.MARGIN;
                    SetEnableDisableManualBtn(Global.allow_auto_trade ? false : true);
                    break;
                case "FUTURES":
                    Global.type_status = TYPE.FUTURES;
                    SetEnableDisableManualBtn(Global.allow_auto_trade ? false : true);
                    break;
            }
        }

        private void comboRT_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            switch (comboBox.Text)
            {
                case "TEST":
                    Global.wallet_status = WALLET_TYPE.TESTNET;
                    break;
                case "REAL":
                    Global.wallet_status = WALLET_TYPE.REAL;
                    break;
            }
        }

        private void SetEnableDisableManualBtn(bool flag)
        {
            btnBuyLimit.Enabled = flag;
            btnBuyMar.Enabled = flag;
            btnSellLimit.Enabled = flag;
            btnSellMar.Enabled = flag;
            if (Global.type_status == TYPE.SPOT)
            {
                btnSellLimit.Enabled = false;
                btnSellMar.Enabled = false;
            }
            if (Global.type_status == TYPE.FUTURES)
                txtLeverage.Enabled = flag;
            else
                txtLeverage.Enabled = false;
        }

        private void SetEnableDisableCombobox(bool flag)
        {
            this.comboPeriod.Enabled = flag;
            this.comboRT.Enabled = flag;
            this.comboSymbol.Enabled = flag;
            this.comboType.Enabled = flag;
            this.txtLeverage.Enabled = flag;
            if (this.comboType.Text != Properties.Resources.FUTURES)
                this.txtLeverage.Enabled = false;
        }

        private void btnBuyMar_Click(object sender, EventArgs e)
        {
            if (Global.connection_status == CONNECTION.DISCONNECTED || this.txtManulQty.Text.Length == 0 || Double.Parse(this.txtManulQty.Text.ToString()) == 0)
            {
                MessageBox.Show("Please connect or input Trade Qty(not 0)");
                return;
            }
            if (ComfirmProcess("Do you really to place Buy Market Order?"))
                placeMarketOrder(OrderSide.Buy);
        }

        private void btnSellMar_Click(object sender, EventArgs e)
        {
            if (Global.connection_status == CONNECTION.DISCONNECTED || this.txtManulQty.Text.Length == 0 || Double.Parse(this.txtManulQty.Text.ToString()) == 0)
            {
                MessageBox.Show("Please connect or input Trade Qty(not 0)");
                return;
            }
            if (ComfirmProcess("Do you really to place Sell Market Order?"))
                placeMarketOrder(OrderSide.Sell);
        }

        private void btnBuyLimit_Click(object sender, EventArgs e)
        {
            if (Global.connection_status == CONNECTION.DISCONNECTED || this.txtManulQty.Text.Length == 0 || Double.Parse(this.txtManulQty.Text.ToString()) == 0)
            {
                MessageBox.Show("Please connect or input Trade Qty(not 0)");
                return;
            }
            if (ComfirmProcess("Do you really to place Buy Limit Order?"))
                placeLimitOrder(OrderSide.Buy);
        }

        private void btnSellLimit_Click(object sender, EventArgs e)
        {
            if (Global.connection_status == CONNECTION.DISCONNECTED || this.txtManulQty.Text.Length == 0 || Double.Parse(this.txtManulQty.Text.ToString()) == 0)
            {
                MessageBox.Show("Please connect or input Trade Qty(not 0)");
                return;
            }
            if (ComfirmProcess("Do you really to place Sell Limit Order?"))
                placeLimitOrder(OrderSide.Sell);
        }

        private bool ComfirmProcess(string msgData)
        {
            DialogResult dialogResult = MessageBox.Show(msgData, "Comfirm Process", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
                return true;
            return false;
        }
        private void GetOrderParamFronUI()
        {
            ManualQtyValue = Double.Parse(this.txtManulQty.Text.ToString());
            QtyValue = Double.Parse(this.txtQty.Text.ToString());
            if (Global.trade_mth == TRADE_METHOD.MANUAL)
            { 
                if (this.txtManualSellLimit.Text != "")
                    Global.sell_limit_price = Double.Parse(this.txtManualSellLimit.Text);
                if (this.txtManualBuyLimit.Text != "")
                    Global.buy_limit_price = Double.Parse(this.txtManualBuyLimit.Text);
            }
            else
            {
                Global.sell_limit_price = Double.Parse(this.txtAutoSellLimit.Text);
                Global.buy_limit_price = Double.Parse(this.txtAutoBuyLimit.Text);
            }
        }
        private void setInputNumberOnly(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void CheckKeepAlive()
        {
            while(Global.connection_status == CONNECTION.CONNECTED)
            {
                TimeSpan diffKeepAlive = DateTime.Now - gdtKeepAliveTime;
                if (diffKeepAlive.TotalMinutes > 30)
                {
                    if (Global.type_status == TYPE.SPOT)
                    { 
                        WebCallResult<object> aliveSpot = Global.client.Spot.UserStream.KeepAliveUserStream(StreamListenKey);
                        if (aliveSpot.Success == false)
                        {
                            Global.log.addLog("KeepAlive Spot failed");
                        }
                    }
                    else if (Global.type_status == TYPE.MARGIN)
                    { 
                        WebCallResult<object> aliveFuture = Global.client.Margin.UserStream.KeepAliveUserStream(StreamListenKey);
                        if (aliveFuture.Success == false)
                        {
                            Global.log.addLog("KeepAlive Future failed");
                        }
                    }
                    else if (Global.type_status == TYPE.FUTURES)
                    {
                        WebCallResult<object> aliveFuture = Global.client.FuturesUsdt.UserStream.KeepAliveUserStream(StreamListenKey);
                        if (aliveFuture.Success == false)
                        {
                            Global.log.addLog("KeepAlive Future failed");
                        }
                    }
                    gdtKeepAliveTime = DateTime.Now;
                }
            }
        }

        private void txtManulQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            setInputNumberOnly(sender, e);
        }

        private void txtQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            setInputNumberOnly(sender, e);
        }

        private void txtManualBuyLimit_KeyPress(object sender, KeyPressEventArgs e)
        {
            setInputNumberOnly(sender, e);
        }

        private void txtManualSellLimit_KeyPress(object sender, KeyPressEventArgs e)
        {
            setInputNumberOnly(sender, e);
        }

        private void txtAutoSellLimit_KeyPress(object sender, KeyPressEventArgs e)
        {
            setInputNumberOnly(sender, e);
        }

        private void txtAutoBuyLimit_KeyPress(object sender, KeyPressEventArgs e)
        {
            setInputNumberOnly(sender, e);
        }

        private void lstLog_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                System.Text.StringBuilder copy_buffer = new System.Text.StringBuilder();
                foreach (object item in lstLog.SelectedItems)
                    copy_buffer.AppendLine(item.ToString());
                if (copy_buffer.Length > 0)
                    Clipboard.SetText(copy_buffer.ToString());
            }
        }

        private void chkAllowTrade_Click(object sender, EventArgs e)
        {
            CheckBox check = (CheckBox)sender;
            SetEnableDisableManualBtn(true);
            Global.trade_mth = TRADE_METHOD.MANUAL;
            Global.allow_auto_trade = false;
            if (check.Checked)
            {
                if (!ComfirmProcess("Do you really allow Auto Trade?"))
                {
                    check.Checked = false;
                    return;
                }
                if (txtAutoSellLimit.Text.Length == 0 || txtAutoBuyLimit.Text.Length == 0 || txtQty.Text.Length == 0 || Double.Parse(txtQty.Text.ToString()) == 0)
                {
                    MessageBox.Show("Please Input the Limit Prices and Qty(not 0)");
                    check.Checked = false;
                    return;
                }
                if (Global.type_status != TYPE.FUTURES)
                {
                    MessageBox.Show("Please connect FUTURES to allow Auto Trade");
                    check.Checked = false;
                    return;
                }
                QtyValue = Double.Parse(txtQty.Text.ToString());
                Global.allow_auto_trade = true;
                Global.order_condition = true;
                SetEnableDisableManualBtn(false);
                Global.trade_mth = TRADE_METHOD.AUTO;
                this.comboType.Text = Properties.Resources.FUTURES;
            }
            else
            {
                Global.order_condition = false;
            }
        }
    }
}
