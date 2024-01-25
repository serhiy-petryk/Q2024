﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;
using CefSharp.OffScreen;
using Quote2024.Helpers;

namespace Quote2024
{
    public partial class MainForm : Form
    {
        private ChromiumWebBrowser browser;// = new ChromiumWebBrowser("www.eoddata.com");
        private System.Net.CookieContainer eoddataCookies = new System.Net.CookieContainer();

        public MainForm()
        {
            InitializeComponent();

            /*dataGridView1.Paint += new PaintEventHandler(dataGridView1_Paint);
            dataGridView1.DataSource = Data.Models.LoaderItem.DataGridLoaderItems;*/

            //=========================
            StatusLabel.Text = "";
            /*clbIntradayDataList.Items.AddRange(IntradayResults.ActionList.Select(a => a.Key).ToArray());
            cbIntradayStopInPercent_CheckedChanged(null, null);
            for (var item = 0; item < clbIntradayDataList.Items.Count; item++)
            {
                clbIntradayDataList.SetItemChecked(item, true);
            }

            StartImageAnimation();*/

            // Logger.MessageAdded += (sender, args) => StatusLabel.Text = args.FullMessage;
            Data.Helpers.Logger.MessageAdded += (sender, args) => this.BeginInvoke((Action)(() => StatusLabel.Text = args.FullMessage));

            if (LicenseManager.UsageMode == LicenseUsageMode.Runtime)
            {
                browser = new ChromiumWebBrowser("www.eoddata.com");
                browser.FrameLoadEnd += Browser_FrameLoadEnd;
            }
        }

        private async void Browser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            if (e.Frame.IsMain)
            {
                var isLogged = await IsLogged();
                if (isLogged)
                {
                    // MessageBox.Show("OK");
                    var cookieManager = Cef.GetGlobalCookieManager();
                    var visitor = new CookieCollector();

                    cookieManager.VisitUrlCookies(browser.Address, true, visitor);

                    var cookies = await visitor.Task; // AWAIT !!!!!!!!!
                    eoddataCookies = new System.Net.CookieContainer();
                    foreach (var cookie in cookies)
                    {
                        eoddataCookies.Add(new System.Net.Cookie()
                        {
                            Name = cookie.Name,
                            Value = cookie.Value,
                            Domain = cookie.Domain
                        });
                    }

                    Data.Actions.Eoddata.EoddataCommon.FnGetEoddataCookies = () => { return eoddataCookies; };
                    Invoke(new Action(() =>
                    {
                        lblEoddataLogged.Text = "Logged in eoddata.com";
                        lblEoddataLogged.BackColor = System.Drawing.Color.Green;
                    }));

                    // var cookieHeader = CookieCollector.GetCookieHeader(cookies);
                    return;
                }

                var userAndPassword = Data.Helpers.CsUtils.GetApiKeys("eoddata.com")[0].Split('^');
                var script = $"document.getElementById('ctl00_cph1_lg1_txtEmail').value='{userAndPassword[0]}';" +
                    $"document.getElementById('ctl00_cph1_lg1_txtPassword').value='{userAndPassword[1]}';" +
                    "document.getElementById('ctl00_cph1_lg1_chkRemember').checked = true;" +
                    "document.getElementById('ctl00_cph1_lg1_btnLogin').click();";
                browser.ExecuteScriptAsync(script);
            }
        }

        private async Task<bool> IsLogged()
        {
            const string script = @"(function()
{
  return document.getElementById('ctl00_cph1_lg1_lblName') == undefined ? null : document.getElementById('ctl00_cph1_lg1_lblName').innerHTML;
  })();";

            var response = await browser.EvaluateScriptAsync(script);
            var logged = response.Success && Equals(response.Result, "Sergei Petrik");
            Invoke(new Action(() =>
            {
                if (logged)
                {
                    lblEoddataLogged.Text = "Logged in eoddata.com";
                    lblEoddataLogged.BackColor = System.Drawing.Color.Green;
                }
                else
                {
                    lblEoddataLogged.Text = "Not logged in eoddata.com";
                    lblEoddataLogged.BackColor = System.Drawing.Color.Red;
                }
            }));

            return logged;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            browser?.Dispose();
            CefSharp.Cef.Shutdown();
        }

        private async void btnTest_Click(object sender, EventArgs e)
        {
            btnTest.Enabled = false;
            
            await Task.Factory.StartNew(Data.Actions.Polygon.PolygonMinuteScan.Start);

            btnTest.Enabled = true;
        }
    }
}
