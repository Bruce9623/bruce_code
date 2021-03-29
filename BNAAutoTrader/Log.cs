using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BNAAutoTrader
{
    class Log
    {
        private Form frm;
        private ListBox lLog;
        string logfile = @"BNALog.log";
        public List<string> logArray;
        public Mutex mt;
        public Log(ListBox logParam, Form frmParam)
        {
            lLog = logParam;
            frm = frmParam;
            logArray = new List<string>();
            if (!File.Exists(logfile))
                File.CreateText(logfile);
            mt = new Mutex();
            Thread th = new Thread(WriteLogFile);
            th.IsBackground = true;
            th.Start();
        }

        public void addLog(string log)
        {
            DateTime localDate = DateTime.Now;
            string logLine = "", logfileLine = "";
            string time_st = localDate.Hour.ToString("00") + ":" + localDate.Minute.ToString("00") + ":" + localDate.Second.ToString("00") + "   ";
            string log_time_st = localDate.Year.ToString("0000") + "-" + localDate.Month.ToString("00") + "-" + localDate.Day.ToString("00") + " " + localDate.Hour.ToString("00") + ":" + localDate.Minute.ToString("00") + ":" + localDate.Second.ToString("00") + "   ";
            logLine = time_st + log;
            logfileLine = log_time_st + log;
            SetText(logLine);
            logArray.Add(logfileLine);
        }

        private void WriteLogFile()
        {
            while(true)
            {
                for (int index = 0; index < logArray.Count; index ++)
                {
                    mt.WaitOne();
                    string log_temp = logArray.ElementAt(index);
                    using (StreamWriter sw = File.AppendText(logfile))
                    {
                        sw.WriteLine(log_temp);
                    }
                    logArray.Remove(log_temp);
                    mt.ReleaseMutex();
                }
                Thread.Sleep(100);
            }
        }
        delegate void SetTextCallback(string text);

        private void SetText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.lLog.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                frm.Invoke(d, new object[] { text });
            }
            else
            {
                this.lLog.Items.Add(text);
            }
        }
    }
}
