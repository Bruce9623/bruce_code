
namespace BNAAutoTrader
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.txtApiKey = new System.Windows.Forms.TextBox();
            this.comboType = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtSecuKey = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnConnect = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.comboPeriod = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.comboRT = new System.Windows.Forms.ComboBox();
            this.comboSymbol = new System.Windows.Forms.ComboBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lstLog = new System.Windows.Forms.ListBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.labCuBidPrice = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.txtManulQty = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.labCuAskPrice = new System.Windows.Forms.Label();
            this.txtManualSellLimit = new System.Windows.Forms.TextBox();
            this.txtManualBuyLimit = new System.Windows.Forms.TextBox();
            this.btnSellLimit = new System.Windows.Forms.Button();
            this.btnBuyLimit = new System.Windows.Forms.Button();
            this.btnSellMar = new System.Windows.Forms.Button();
            this.btnBuyMar = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.chkAllowTrade = new System.Windows.Forms.CheckBox();
            this.txtQty = new System.Windows.Forms.TextBox();
            this.txtAutoBuyLimit = new System.Windows.Forms.TextBox();
            this.txtAutoSellLimit = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.txtLeverage = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "API Key :";
            // 
            // txtApiKey
            // 
            this.txtApiKey.Location = new System.Drawing.Point(97, 25);
            this.txtApiKey.Name = "txtApiKey";
            this.txtApiKey.Size = new System.Drawing.Size(184, 20);
            this.txtApiKey.TabIndex = 1;
            this.txtApiKey.Text = "5f7a2183c4ba3c6c331a3b75987ff0b0a24f52bfd5427a9ea05a6faf093ebf66";
            // 
            // comboType
            // 
            this.comboType.FormattingEnabled = true;
            this.comboType.Items.AddRange(new object[] {
            "SPOT",
            "MARGIN",
            "FUTURES"});
            this.comboType.Location = new System.Drawing.Point(85, 27);
            this.comboType.Name = "comboType";
            this.comboType.Size = new System.Drawing.Size(184, 21);
            this.comboType.TabIndex = 2;
            this.comboType.Text = "FUTURES";
            this.comboType.SelectedIndexChanged += new System.EventHandler(this.comboType_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Type :";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtSecuKey);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtApiKey);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 9);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(293, 93);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Security Info";
            // 
            // txtSecuKey
            // 
            this.txtSecuKey.Location = new System.Drawing.Point(97, 59);
            this.txtSecuKey.Name = "txtSecuKey";
            this.txtSecuKey.Size = new System.Drawing.Size(184, 20);
            this.txtSecuKey.TabIndex = 8;
            this.txtSecuKey.Text = "4f3b57d5e2b30851cbddb31538cb2e12cc9cc82993f956f2a14f2f457ab7b711";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Security Key :";
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(122, 187);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(93, 32);
            this.btnConnect.TabIndex = 5;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 63);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Symbol :";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtLeverage);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.comboPeriod);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.comboRT);
            this.groupBox2.Controls.Add(this.comboSymbol);
            this.groupBox2.Controls.Add(this.btnConnect);
            this.groupBox2.Controls.Add(this.comboType);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Location = new System.Drawing.Point(12, 108);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(293, 223);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Connection Settings";
            // 
            // comboPeriod
            // 
            this.comboPeriod.FormattingEnabled = true;
            this.comboPeriod.Items.AddRange(new object[] {
            "1m",
            "3m",
            "5m",
            "15m",
            "30m",
            "1h",
            "2h",
            "4h",
            "6h",
            "8h",
            "12h",
            "1d",
            "3d",
            "1w",
            "1M"});
            this.comboPeriod.Location = new System.Drawing.Point(85, 131);
            this.comboPeriod.Name = "comboPeriod";
            this.comboPeriod.Size = new System.Drawing.Size(184, 21);
            this.comboPeriod.TabIndex = 10;
            this.comboPeriod.Text = "1m";
            this.comboPeriod.SelectedIndexChanged += new System.EventHandler(this.comboPeriod_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(16, 134);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(43, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "Period :";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(26, 98);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(33, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "R/T :";
            // 
            // comboRT
            // 
            this.comboRT.FormattingEnabled = true;
            this.comboRT.Items.AddRange(new object[] {
            "TEST",
            "REAL"});
            this.comboRT.Location = new System.Drawing.Point(85, 95);
            this.comboRT.Name = "comboRT";
            this.comboRT.Size = new System.Drawing.Size(184, 21);
            this.comboRT.TabIndex = 7;
            this.comboRT.Text = "TEST";
            this.comboRT.SelectedIndexChanged += new System.EventHandler(this.comboRT_SelectedIndexChanged);
            // 
            // comboSymbol
            // 
            this.comboSymbol.FormattingEnabled = true;
            this.comboSymbol.Items.AddRange(new object[] {
            "BTCUSDT",
            "ETHUSDT",
            "ETHBTC"});
            this.comboSymbol.Location = new System.Drawing.Point(85, 60);
            this.comboSymbol.Name = "comboSymbol";
            this.comboSymbol.Size = new System.Drawing.Size(184, 21);
            this.comboSymbol.TabIndex = 6;
            this.comboSymbol.Text = "BTCUSDT";
            this.comboSymbol.SelectedIndexChanged += new System.EventHandler(this.comboSymbol_SelectedIndexChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lstLog);
            this.groupBox3.Location = new System.Drawing.Point(12, 337);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(604, 322);
            this.groupBox3.TabIndex = 9;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Log";
            // 
            // lstLog
            // 
            this.lstLog.FormattingEnabled = true;
            this.lstLog.HorizontalScrollbar = true;
            this.lstLog.Location = new System.Drawing.Point(15, 25);
            this.lstLog.Name = "lstLog";
            this.lstLog.Size = new System.Drawing.Size(577, 290);
            this.lstLog.TabIndex = 0;
            this.lstLog.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstLog_KeyDown);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.labCuBidPrice);
            this.groupBox4.Controls.Add(this.label12);
            this.groupBox4.Controls.Add(this.txtManulQty);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.label10);
            this.groupBox4.Controls.Add(this.labCuAskPrice);
            this.groupBox4.Controls.Add(this.txtManualSellLimit);
            this.groupBox4.Controls.Add(this.txtManualBuyLimit);
            this.groupBox4.Controls.Add(this.btnSellLimit);
            this.groupBox4.Controls.Add(this.btnBuyLimit);
            this.groupBox4.Controls.Add(this.btnSellMar);
            this.groupBox4.Controls.Add(this.btnBuyMar);
            this.groupBox4.Location = new System.Drawing.Point(312, 9);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(304, 192);
            this.groupBox4.TabIndex = 10;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Manual Trade Settings";
            // 
            // labCuBidPrice
            // 
            this.labCuBidPrice.AutoSize = true;
            this.labCuBidPrice.Location = new System.Drawing.Point(149, 45);
            this.labCuBidPrice.Name = "labCuBidPrice";
            this.labCuBidPrice.Size = new System.Drawing.Size(0, 13);
            this.labCuBidPrice.TabIndex = 12;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(137, 45);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(12, 13);
            this.label12.TabIndex = 11;
            this.label12.Text = "/";
            // 
            // txtManulQty
            // 
            this.txtManulQty.Location = new System.Drawing.Point(131, 77);
            this.txtManulQty.Name = "txtManulQty";
            this.txtManulQty.Size = new System.Drawing.Size(161, 20);
            this.txtManulQty.TabIndex = 10;
            this.txtManulQty.Text = "0";
            this.txtManulQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtManulQty_KeyPress);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 77);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(98, 13);
            this.label7.TabIndex = 9;
            this.label7.Text = "Manual Trade Qty :";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(117, 26);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(51, 13);
            this.label10.TabIndex = 8;
            this.label10.Text = "Ask / Bid";
            // 
            // labCuAskPrice
            // 
            this.labCuAskPrice.AutoSize = true;
            this.labCuAskPrice.Location = new System.Drawing.Point(89, 45);
            this.labCuAskPrice.Name = "labCuAskPrice";
            this.labCuAskPrice.Size = new System.Drawing.Size(0, 13);
            this.labCuAskPrice.TabIndex = 7;
            this.labCuAskPrice.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtManualSellLimit
            // 
            this.txtManualSellLimit.Location = new System.Drawing.Point(131, 155);
            this.txtManualSellLimit.Name = "txtManualSellLimit";
            this.txtManualSellLimit.Size = new System.Drawing.Size(161, 20);
            this.txtManualSellLimit.TabIndex = 5;
            this.txtManualSellLimit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtManualSellLimit_KeyPress);
            // 
            // txtManualBuyLimit
            // 
            this.txtManualBuyLimit.Location = new System.Drawing.Point(131, 112);
            this.txtManualBuyLimit.Name = "txtManualBuyLimit";
            this.txtManualBuyLimit.Size = new System.Drawing.Size(161, 20);
            this.txtManualBuyLimit.TabIndex = 4;
            this.txtManualBuyLimit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtManualBuyLimit_KeyPress);
            // 
            // btnSellLimit
            // 
            this.btnSellLimit.Location = new System.Drawing.Point(15, 146);
            this.btnSellLimit.Name = "btnSellLimit";
            this.btnSellLimit.Size = new System.Drawing.Size(75, 36);
            this.btnSellLimit.TabIndex = 3;
            this.btnSellLimit.Text = "Sell Limit";
            this.btnSellLimit.UseVisualStyleBackColor = true;
            this.btnSellLimit.Click += new System.EventHandler(this.btnSellLimit_Click);
            // 
            // btnBuyLimit
            // 
            this.btnBuyLimit.Location = new System.Drawing.Point(15, 103);
            this.btnBuyLimit.Name = "btnBuyLimit";
            this.btnBuyLimit.Size = new System.Drawing.Size(75, 37);
            this.btnBuyLimit.TabIndex = 2;
            this.btnBuyLimit.Text = "Buy Limit";
            this.btnBuyLimit.UseVisualStyleBackColor = true;
            this.btnBuyLimit.Click += new System.EventHandler(this.btnBuyLimit_Click);
            // 
            // btnSellMar
            // 
            this.btnSellMar.Location = new System.Drawing.Point(219, 26);
            this.btnSellMar.Name = "btnSellMar";
            this.btnSellMar.Size = new System.Drawing.Size(75, 41);
            this.btnSellMar.TabIndex = 1;
            this.btnSellMar.Text = "Sell Market";
            this.btnSellMar.UseVisualStyleBackColor = true;
            this.btnSellMar.Click += new System.EventHandler(this.btnSellMar_Click);
            // 
            // btnBuyMar
            // 
            this.btnBuyMar.Location = new System.Drawing.Point(10, 25);
            this.btnBuyMar.Name = "btnBuyMar";
            this.btnBuyMar.Size = new System.Drawing.Size(75, 40);
            this.btnBuyMar.TabIndex = 0;
            this.btnBuyMar.Text = "Buy Market";
            this.btnBuyMar.UseVisualStyleBackColor = true;
            this.btnBuyMar.Click += new System.EventHandler(this.btnBuyMar_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label11);
            this.groupBox5.Controls.Add(this.chkAllowTrade);
            this.groupBox5.Controls.Add(this.txtQty);
            this.groupBox5.Controls.Add(this.txtAutoBuyLimit);
            this.groupBox5.Controls.Add(this.txtAutoSellLimit);
            this.groupBox5.Controls.Add(this.label9);
            this.groupBox5.Controls.Add(this.label8);
            this.groupBox5.Location = new System.Drawing.Point(312, 208);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(304, 123);
            this.groupBox5.TabIndex = 11;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Auto Trade Settings";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(8, 76);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(85, 13);
            this.label11.TabIndex = 14;
            this.label11.Text = "Auto Trade Qty :";
            // 
            // chkAllowTrade
            // 
            this.chkAllowTrade.AutoSize = true;
            this.chkAllowTrade.Location = new System.Drawing.Point(14, 96);
            this.chkAllowTrade.Name = "chkAllowTrade";
            this.chkAllowTrade.Size = new System.Drawing.Size(107, 17);
            this.chkAllowTrade.TabIndex = 4;
            this.chkAllowTrade.Text = "Allow Auto Trade";
            this.chkAllowTrade.UseVisualStyleBackColor = true;
            this.chkAllowTrade.Click += new System.EventHandler(this.chkAllowTrade_Click);
            // 
            // txtQty
            // 
            this.txtQty.Location = new System.Drawing.Point(120, 73);
            this.txtQty.Name = "txtQty";
            this.txtQty.Size = new System.Drawing.Size(172, 20);
            this.txtQty.TabIndex = 13;
            this.txtQty.Text = "0";
            this.txtQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtQty_KeyPress);
            // 
            // txtAutoBuyLimit
            // 
            this.txtAutoBuyLimit.Location = new System.Drawing.Point(118, 47);
            this.txtAutoBuyLimit.Name = "txtAutoBuyLimit";
            this.txtAutoBuyLimit.Size = new System.Drawing.Size(174, 20);
            this.txtAutoBuyLimit.TabIndex = 3;
            this.txtAutoBuyLimit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtAutoBuyLimit_KeyPress);
            // 
            // txtAutoSellLimit
            // 
            this.txtAutoSellLimit.Location = new System.Drawing.Point(118, 21);
            this.txtAutoSellLimit.Name = "txtAutoSellLimit";
            this.txtAutoSellLimit.Size = new System.Drawing.Size(174, 20);
            this.txtAutoSellLimit.TabIndex = 2;
            this.txtAutoSellLimit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtAutoSellLimit_KeyPress);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 50);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(82, 13);
            this.label9.TabIndex = 1;
            this.label9.Text = "Buy Limit Price :";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(13, 25);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(81, 13);
            this.label8.TabIndex = 0;
            this.label8.Text = "Sell Limit Price :";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(1, 163);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(58, 13);
            this.label13.TabIndex = 11;
            this.label13.Text = "Leverage :";
            // 
            // txtLeverage
            // 
            this.txtLeverage.Location = new System.Drawing.Point(85, 162);
            this.txtLeverage.Name = "txtLeverage";
            this.txtLeverage.Size = new System.Drawing.Size(184, 20);
            this.txtLeverage.TabIndex = 12;
            this.txtLeverage.Text = "20";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(623, 694);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "BNAAutoTrader";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtApiKey;
        private System.Windows.Forms.ComboBox comboType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtSecuKey;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox comboSymbol;
        private System.Windows.Forms.ComboBox comboRT;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ListBox lstLog;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox comboPeriod;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btnBuyMar;
        private System.Windows.Forms.Button btnSellMar;
        private System.Windows.Forms.Button btnSellLimit;
        private System.Windows.Forms.Button btnBuyLimit;
        private System.Windows.Forms.TextBox txtManualBuyLimit;
        private System.Windows.Forms.TextBox txtManualSellLimit;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtAutoBuyLimit;
        private System.Windows.Forms.TextBox txtAutoSellLimit;
        private System.Windows.Forms.Label labCuAskPrice;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtQty;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.CheckBox chkAllowTrade;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtManulQty;
        private System.Windows.Forms.Label labCuBidPrice;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtLeverage;
        private System.Windows.Forms.Label label13;
    }
}

