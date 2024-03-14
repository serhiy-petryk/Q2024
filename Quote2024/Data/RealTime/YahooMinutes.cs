﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Data.Helpers;

namespace Data.RealTime
{
    public class YahooMinutes
    {
        // private const string UrlTemplate = @"https://query2.finance.yahoo.com/v8/finance/chart/{0}?period1={1}&period2={2}&interval=1m&events=history";
        private const string UrlTemplate = @"https://query2.finance.yahoo.com/v8/finance/chart/{0}?period1={1}&period2={2}&interval=1m&includePrePost=true&events=history";
        
        private const string FileTemplate = @"{0}.json";

        public static async Task<(Dictionary<string, byte[]>, Dictionary<string, Exception>)> CheckSymbols()
        {
            Logger.AddMessage($"Check Yahoo minute data");
            var from = new DateTimeOffset(DateTime.UtcNow.AddMinutes(-30)).ToUnixTimeSeconds();
            var to = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();

            var urlsAndFileNames = _symbols
                .Select(s => (string.Format(UrlTemplate, s, from, to), string.Format(FileTemplate, s)));

            var sw = new Stopwatch();
            sw.Start();

            var tasks = new ConcurrentDictionary<string, Task<byte[]>>();
            foreach (var symbol in _symbols)
            {
                var task = Download.DownloadToBytesAsync(string.Format(UrlTemplate, symbol, from, to));
                tasks[symbol] = task;
            }

            var validSymbols = new Dictionary<string, byte[]>();
            var invalidSymbols = new Dictionary<string, Exception>();
            foreach (var kvp in tasks)
            {
                try
                {
                    // release control to the caller until the task is done, which will be near immediate for each task following the first
                    validSymbols.Add(kvp.Key, await kvp.Value);
                }
                catch (Exception ex)
                {
                    invalidSymbols.Add(kvp.Key, ex);
                    Debug.Print($"{DateTime.Now.TimeOfDay}. Symbol: {kvp.Key}. Error: {ex.Message}");
                }
            }

            sw.Stop();
            Debug.Print($"SW: {sw.ElapsedMilliseconds}. From: {from}. To: {to}");

            Logger.AddMessage($"Yahoo minute data checked. {invalidSymbols.Count} invalid symbols, {validSymbols.Count} valid symbols.");
            return (validSymbols, invalidSymbols);
        }

        public static async void Start(string[] symbols)
        {
            const int minutes = 30;
            Logger.AddMessage($"Download Yahoo minute data from {DateTime.UtcNow.AddMinutes(-minutes):HH:mm:ss} to {DateTime.UtcNow:HH:mm:ss} UTC");

            var from = new DateTimeOffset(DateTime.UtcNow.AddMinutes(-minutes)).ToUnixTimeSeconds();
            var to = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();

            var urlsAndFileNames =
                symbols.Select(s => (string.Format(UrlTemplate, s, from, to), string.Format(FileTemplate, s)));

            var sw = new Stopwatch();
            sw.Start();

            var tasks = new ConcurrentDictionary<string, Task<byte[]>>();
            foreach (var symbol in symbols)
            {
                var task = Download.DownloadToBytesAsync(string.Format(UrlTemplate, symbol, from, to));
                tasks[symbol] = task;
            }

            // await Task.WhenAll(tasks.Values);
            var results = new Dictionary<string, byte[]>();
            foreach (var kvp in tasks)
            {
                try
                {
                    // release control to the caller until the task is done, which will be near immediate for each task following the first
                    results.Add(kvp.Key, await kvp.Value);
                }
                catch (Exception e)
                {
                    results.Add(kvp.Key, Array.Empty<byte>());
                    Debug.Print($"{DateTime.Now.TimeOfDay}. Error: {e.Message}");
                }
            }

            sw.Stop();
            Debug.Print($"SW: {sw.ElapsedMilliseconds}. From: {from}. To: {to}");

            SaveResult(results);

            var aa1 = CsUtils.MemoryUsedInBytes / 1024 / 1024;
            tasks.Clear();
            var aa2 = CsUtils.MemoryUsedInBytes / 1024 / 1024;
        }

        public static void SaveResult(Dictionary<string, byte[]> results)
        {
            var virtualFileEntries =
                results.Select(kvp => new VirtualFileEntry(string.Format(FileTemplate, kvp.Key), kvp.Value));

            ZipUtils.ZipVirtualFileEntries($@"E:\Quote\WebData\RealTime\YahooMinute\RTYahooMinutes_{DateTime.Now:yyyyMMddHHmmss}.zip", virtualFileEntries);
        }

        public static string[] _symbols = new[]
        {
            "AAP", "TRP", "PWR", "PACK", "ETR", "LH", "APTV", "STLA", "KHC", "LW", "IVZ", "COWZ", "CIEN", "JD", "NEM",
            "LRN", "DLR", "MSFT", "BITX", "UAA", "HBAN", "DV", "ARKK", "YUMC", "SOXX", "M", "CALF", "AI", "NKE", "EFG",
            "OUT", "ESGU", "EWBC", "XRT", "PDD", "SPLV", "OIH", "LSXMA", "QYLD", "EWZ", "GLD", "TRU", "SGLY", "TMUS",
            "VALE", "CNHI", "CCJ", "AAPL", "ESTC", "XLP", "VICI", "ALL", "EWT", "INVH", "CMS", "SPR", "QLYS", "SWKS",
            "BBWI", "BN", "GWRE", "BALL", "MGA", "MINT", "AMT", "PARA", "RIO", "ACWX", "KOLD", "GEN", "STNG", "PTEN",
            "XBI", "DHR", "TMF", "CMA", "XME", "UDOW", "CPNG", "ING", "ZION", "YANG", "IGLB", "CELH", "KRYS", "DUK",
            "FRT", "APP", "AXP", "DOW", "CRI", "VXF", "VXUS", "SPLK", "TRMB", "MKC", "AMP", "QLD", "MS", "LSXMK",
            "PCOR", "EFV", "NVDX", "MSTR", "XEL", "RPRX", "HOLO", "VGT", "PFGC", "TPX", "FUTU", "SITE", "ALV", "TXG",
            "LNT", "SRPT", "ADP", "EXR", "CZR", "ES", "HODL", "XLG", "HWM", "CLS", "WPC", "CRWD", "CRM", "IXUS", "HST",
            "FLS", "EPD", "GIS", "AZN", "AL", "CBAY", "HYG", "WIX", "MARA", "VT", "PR", "FSLR", "JNPR", "NTR", "WM",
            "EL", "ZTO", "TRGP", "PBR-A", "TAL", "HSY", "COF", "ARW", "TSLT", "SDOW", "COIN", "RIVN", "CAVA", "BIV",
            "SVIX", "GOOG", "MOS", "VIXY", "EEM", "MNST", "WY", "CAG", "BTCO", "LKQ", "PFF", "EW", "WTRG", "ZBH", "BSV",
            "IBN", "DGRO", "DECK", "VCIT", "RIOT", "AGNC", "BPMC", "NCLH", "SPHQ", "SCHW", "SGOL", "IEFA", "ROIV",
            "MMC", "FTI", "AFL", "FE", "RUN", "CRL", "CMI", "STLD", "GLDM", "JETS", "RUM", "GLW", "VTI", "EQT", "TEAM",
            "GFS", "DKS", "MTH", "KBH", "VRTX", "DKNG", "DXJ", "TXT", "CLRO", "HLT", "PLUG", "SAP", "CLX", "BNDX",
            "USMV", "EFA", "INFY", "RF", "AU", "ARKG", "ORLY", "BHVN", "SCHP", "IJH", "BF-B", "GS", "VOYA", "PODD",
            "OKTA", "QDEL", "KEYS", "CYBR", "CARR", "GEHC", "AXTA", "KO", "LQD", "ITW", "SNPS", "SPGI", "DOC", "SYF",
            "VGK", "SQQQ", "AAL", "ACWI", "VIG", "CINF", "INTC", "BTI", "SOXS", "MAT", "VRSN", "SLV", "SMIN", "GBTC",
            "HON", "IUSB", "PLNT", "VAL", "TBLA", "XLU", "T", "FNGD", "QQQM", "NDSN", "BR", "AKAM", "COST", "ZTS",
            "IHI", "PFE", "BILI", "CHH", "ACN", "MOAT", "BLV", "ARE", "IVE", "O", "IWM", "PSTG", "VLUE", "IVW", "GM",
            "CHWY", "DINO", "KBR", "QID", "WING", "EFX", "NICE", "DOCU", "CALM", "SCHX", "XYL", "EPRT", "CTVA", "AEM",
            "MUB", "ARES", "ASX", "NTES", "LEA", "XOP", "NXST", "VEEV", "TELL", "EXPD", "SEE", "GDX", "SWAV", "ACGL",
            "HAS", "HUBS", "AIT", "MTUM", "CNP", "REG", "CM", "V", "CNI", "LABD", "IGIB", "FXI", "VTR", "MUSA", "GFI",
            "TFC", "HES", "XLE", "SRLN", "HPQ", "MDY", "REXR", "VGIT", "BEAM", "BCS", "JAAA", "UBER", "BK", "VYM",
            "FMX", "LYV", "CBRE", "URBN", "UPST", "LULU", "OLED", "EQR", "EMN", "SOFI", "APO", "LNG", "TD", "SPSM",
            "AEO", "AGG", "VO", "FANG", "ZM", "UVIX", "BP", "BAX", "PYPL", "VGLT", "FNGU", "UAL", "EQH", "AGR", "KLAC",
            "BAC", "IWY", "BJ", "STM", "WBA", "XLV", "SOUN", "IGV", "WDC", "SHY", "HCA", "SCHF", "BITI", "ED", "SHOP",
            "EQNR", "SE", "GNRC", "TOST", "IONQ", "MPC", "NSIT", "DOOR", "FHN", "POOL", "ENB", "VUG", "AJG", "EMXC",
            "FND", "VRSK", "NOW", "VCLT", "FL", "VST", "FDX", "NVS", "NVDA", "RL", "WSM", "BX", "RGEN", "CCEP", "IWF",
            "BEN", "KWEB", "JNJ", "SPXS", "IEI", "COP", "DTE", "AEP", "FICO", "IRWD", "SEDG", "FOUR", "ARMK", "EIX",
            "TKO", "KIM", "WCN", "SMH", "FAS", "PBR", "IWN", "ARCC", "CSCO", "VB", "GRAB", "MANH", "TDOC", "SIRI",
            "BBEU", "ITB", "SCCO", "GOVT", "PLD", "POWL", "NXE", "KMX", "FN", "URTY", "OC", "NVST", "DELL", "LEG",
            "BIO", "AOS", "JPST", "RCL", "PNW", "VEU", "SBUX", "ALLY", "PENN", "HAL", "FERG", "ROP", "CNX", "MPWR",
            "RYAAY", "AWK", "AER", "GDXJ", "WEC", "HRL", "MDGL", "QQQ", "FNV", "CTSH", "EXC", "GRMN", "EWL", "WST",
            "ELV", "GOOGL", "XLI", "YUM", "ISRG", "H", "AMH", "BKLN", "XHB", "IEMG", "MKTX", "ATMU", "CHD", "MPLX",
            "XOM", "PI", "MAR", "RMBS", "SPHY", "IR", "AA", "VOO", "XLY", "IWO", "GLOB", "BIL", "RACE", "FCNCA", "A",
            "IWB", "QTWO", "OVV", "ST", "SHEL", "AAXJ", "NUGT", "IEF", "CTLT", "TECH", "MA", "TQQQ", "SFM", "CB",
            "DDOG", "CNQ", "SSO", "UNG", "GOLD", "ATI", "EWH", "NYT", "TT", "GTES", "FEZ", "AYX", "FAST", "AMCR", "LI",
            "JEPQ", "ROKU", "PAYX", "GPK", "BITB", "WAB", "DAR", "CCI", "HYLB", "LNW", "ORCL", "VKTX", "DIA", "DE",
            "FRPT", "UVXY", "ROST", "DPZ", "CIVI", "VOOG", "ICE", "RDN", "RBLX", "TLT", "STT", "GL", "SPLG", "GERN",
            "XLF", "ETRN", "VGSH", "MO", "HUM", "UTHR", "SPG", "ENPH", "BWXT", "OLN", "IREN", "EWY", "HSBC", "SJNK",
            "AGCO", "PSN", "ITOT", "AXSM", "AME", "AZEK", "SPYV", "USHY", "ROL", "PM", "IYR", "MGEE", "NVDL", "FWONK",
            "GDDY", "BOX", "BROS", "SYK", "CP", "IBB", "BOIL", "VCSH", "EWJ", "RXRX", "WFC", "VTV", "SOXL", "FIX",
            "ADM", "QTEC", "WRB", "TME", "MTG", "AVTR", "LDOS", "ETN", "AMR", "SRE", "AZO", "CDW", "ACM", "UNP", "ARM",
            "FNF", "IWP", "SCHD", "VDE", "SDY", "BWA", "EWU", "KKR", "DGX", "QSR", "NBIX", "LVS", "AIG", "KNX", "OKE",
            "LII", "ALC", "AVUV", "WPM", "USFR", "ATKR", "SVXY", "NET", "ACAD", "VTWO", "TEL", "HIG", "ALB", "GLPI",
            "MELI", "PAYC", "TSN", "D", "CPRT", "AMD", "SDS", "CFG", "NKLA", "CBOE", "NVT", "SPTL", "BITO", "SPIB",
            "SPXU", "EWC", "MMM", "PEP", "SPDW", "CRC", "PNR", "BRBR", "SHAK", "STNE", "BLDR", "TTD", "WYNN", "VIPS",
            "IAU", "MP", "HDB", "CCL", "BTCW", "XLRE", "PRU", "TW", "KBE", "TSM", "BEKE", "ZBRA", "JDST", "SBAC", "RH",
            "PATH", "CVNA", "ARKB", "CART", "HOOD", "BURL", "EME", "LBRT", "TWLO", "TDG", "KIE", "NVDS", "PRGO", "FLT",
            "VONG", "MHK", "GD", "PSX", "MGK", "PSQ", "YETI", "TECK", "EDU", "ANET", "EXPE", "GPN", "ITA", "ASHR",
            "NOVA", "CLSK", "LYB", "SHV", "XLK", "TSLA", "KGC", "EZU", "IBIT", "SMAR", "CF", "ONTO", "SUM", "ALK",
            "BRO", "WU", "CRH", "BBJP", "TSLQ", "AON", "NVO", "MCK", "DPST", "RRX", "APD", "SPXL", "DFS", "IRTC", "SKX",
            "XP", "MRNA", "PCAR", "ATO", "VBR", "PTC", "COHR", "PBF", "VBK", "MSCI", "SG", "QUAL", "UPRO", "GILD",
            "ODFL", "DEO", "ICLR", "DBX", "TMV", "RSP", "SCHG", "ERIC", "XLB", "IWD", "SPY", "XPO", "NU", "FTNT", "WCC",
            "NTNX", "EVR", "IYW", "TECL", "MRK", "RYAN", "ZS", "OMC", "NYCB", "VOD", "IP", "RGLS", "MGY", "FTV", "APH",
            "BSX", "BTU", "VRT", "YINN", "BILL", "JNK", "TTWO", "TIP", "BBD", "BND", "VYX", "QRVO", "SPYG", "VTEB",
            "VEA", "ALLE", "TER", "ADC", "PULS", "IWR", "SH", "SYM", "INSP", "DXCM", "WSO", "MEDP", "CHRW", "DVA",
            "NWSA", "SGOV", "TS", "AMZN", "EWW", "ROK", "MRVL", "COR", "JEPI", "BNS", "K", "CAH", "USO", "DHI", "VFH",
            "REGN", "CFLT", "RY", "GPC", "KEY", "TNA", "CPB", "CLF", "TREX", "DDS", "TTE", "ELF", "VNQ", "BCE", "XLC",
            "OXY", "STX", "SAIA", "LABU", "CI", "FOXA", "TXRH", "WRK", "DOX", "IT", "HSIC", "FBTC", "INDA", "SMCI",
            "WEX", "WH", "JCI", "OTIS", "MT", "ACHC", "CCK", "URA", "IJR", "THO", "TECS", "DAY", "WSC", "VXX", "CVE",
            "WFRD", "IVV", "VWO", "TZA", "INTU", "CPT", "BLK", "ASML", "SCHO", "ULTA", "IDEV", "TPR", "NTRA", "CAT",
            "W", "BRKR", "KRE", "HCC", "BAH", "AEE", "VTRS", "GPS", "PEG", "CERE", "LHX", "CTAS", "TGT", "MOH", "NNN",
            "EMB", "CRBG", "BHP", "KVUE", "WAL", "JBHT", "X", "DRI", "PCTY", "ALGN", "DVY", "PEN", "FI", "TSCO", "RBA",
            "ADBE", "DIS", "PKG", "ADT", "PPL", "ANSS", "HPE", "VZ", "AFRM", "BMY", "VFC", "CHKP", "TTC", "S", "NSC",
            "LIN", "IGSB", "UMC", "CYTK", "ETSY", "IPG", "BSY", "GE", "MNDY", "CNC", "CNK", "UDR", "CVX", "ITRI", "BKR",
            "MTD", "MOD", "ZIM", "MGM", "NTRS", "FITB", "VLO", "ARGX", "RJF", "WOLF", "MCHI", "EMR", "DB", "THC", "BA",
            "CRSP", "NTAP", "STZ", "SWN", "HCP", "TMO", "FCX", "SM", "RPM", "PINS", "JWN", "BBY", "MET", "TDW", "XPEV",
            "IBM", "NXT", "BIIB", "AVB", "FIVE", "COLB", "Z", "UL", "GSK", "LAD", "SJM", "FTRE", "NEE", "FDS", "WTW",
            "MU", "ECL", "CCOI", "TOL", "CTRA", "ARRY", "HOG", "CHTR", "FIVN", "DOV", "INCY", "BRK-B", "LLY", "PCG",
            "KMB", "IBKR", "XRAY", "CIBR", "DAL", "ADI", "WMS", "QCOM", "JBL", "VLTO", "WBD", "NRG", "LRCX", "LEGN",
            "MMSI", "IEX", "MDLZ", "UNH", "AMGN", "EBAY", "PGR", "HMY", "BMRN", "ENTG", "CDNS", "MDB", "KDP", "JKHY",
            "SNA", "RHI", "WMT", "CNM", "SYY", "UHS", "PFG", "SLB", "EQIX", "ARCH", "ET", "KRTX", "PANW", "ZI", "NLY",
            "NOC", "HTHT", "SO", "FCN", "CMCSA", "WHR", "FFIV", "LEN", "ON", "TXN", "CMG", "EXAS", "META", "JBLU",
            "NIO", "IQV", "AVGO", "IDXX", "PPG", "ILMN", "APPF", "ALTM", "KR", "RMD", "RVMD", "BZ", "CSX", "COO", "MLM",
            "HEI", "TRV", "CROX", "STE", "AXON", "IFF", "EOG", "UPS", "NUE", "KSS", "AIZ", "ITUB", "BL", "DLTR", "ASAN",
            "IOT", "SNOW", "PVH", "UNM", "DUOL", "FMC", "NVR", "C", "TROW", "SUI", "GTLB", "SHW", "LCID", "TDY", "JPM",
            "EXP", "ALNY", "MDT", "EA", "ABNB", "IRM", "PH", "MTB", "NXPI", "MBB", "MDC", "MCO", "TAP", "CG", "CHK",
            "U", "IOVA", "CL", "COLD", "HOLX", "SN", "BDX", "EG", "FLEX", "PLTR", "EPAM", "MKL", "WMB", "HII", "ACLS",
            "PNC", "MNSO", "CME", "LPX", "PG", "ELAN", "LPLA", "LSCC", "EXEL", "TYL", "VSCO", "UBS", "USB", "WAT",
            "JAZZ", "YMM", "MPW", "CSGP", "SWK", "BRK-A", "ASO", "LMT", "MSI", "RS", "MCHP", "SPOT", "AES", "SNAP", "J",
            "RIG", "AMAT", "ADSK", "MTZ", "TJX", "LYFT", "CHRD", "F", "ONON", "CEG", "GLNG", "MUR", "PSA", "GWW", "CVS",
            "AVY", "MBLY", "APA", "NFLX", "PXD", "BIDU", "MAS", "NDAQ", "DVN", "MKSI", "APLS", "MTN", "TM", "WELL",
            "SQ", "TEVA", "CONL", "EGP", "BKNG", "CASY", "URI", "ALGM", "KMI", "JXN", "RNR", "KNSL", "LUV", "PHM",
            "IDA", "CE", "RTX", "MCD", "FIS", "HUBB", "ICLN", "VMC", "RSG", "DQ", "MTCH", "BG", "ESS", "CLH", "BABA",
            "RRC", "EVRG", "NI", "TSLL", "MRO", "GCT", "DASH", "SU", "DT", "WDAY", "AR", "ESGE", "CSL", "MAA", "SQM",
            "HD", "TCOM", "ABT", "ACHR", "DG", "ABBV", "BLD", "LOW", "DD", "HNRA", "ANF"
        };
    }
}
