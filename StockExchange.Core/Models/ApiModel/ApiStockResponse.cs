using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockExchange.Core.Models.ApiModel
{
    public class StockData
    {
        public HisseYuzeysel hisseYuzeysel { get; set; }
        public object stc_5_3 { get; set; }
        public object rsI14 { get; set; }
        public object mov10 { get; set; }
        public object ccI14 { get; set; }
    }

    public class HisseYuzeysel
    {
        public int sembolid { get; set; }
        public string sembol { get; set; }
        public DateTime tarih { get; set; }
        public int sektorid { get; set; }
        public double alis { get; set; }
        public double satis { get; set; }
        public double acilis { get; set; }
        public double yuksek { get; set; }
        public double yukseK1 { get; set; }
        public double yukseK2 { get; set; }
        public double dusuk { get; set; }
        public double dusuK1 { get; set; }
        public double dusuK2 { get; set; }
        public double kapanis { get; set; }
        public double kapaniS1 { get; set; }
        public double kapaniS2 { get; set; }
        public int hacimlot { get; set; }
        public int hacimloT1 { get; set; }
        public int hacimloT2 { get; set; }
        public double aort { get; set; }
        public double aorT1 { get; set; }
        public double aorT2 { get; set; }
        public int hacimtldun { get; set; }
        public int hacimyuzdedegisim { get; set; }
        public int hacimtl { get; set; }
        public int hacimtL1 { get; set; }
        public int hacimtL2 { get; set; }
        public double dunkukapanis { get; set; }
        public double oncekikapanis { get; set; }
        public int izafikapanis { get; set; }
        public double tavan { get; set; }
        public double taban { get; set; }
        public double yilyuksek { get; set; }
        public double yildusuk { get; set; }
        public double ayyuksek { get; set; }
        public double aydusuk { get; set; }
        public double haftayuksek { get; set; }
        public double haftadusuk { get; set; }
        public int oncekiyilkapanis { get; set; }
        public double oncekiaykapanis { get; set; }
        public double oncekihaftakapanis { get; set; }
        public double yilortalama { get; set; }
        public double ayortalama { get; set; }
        public double haftaortalama { get; set; }
        public double yuzdedegisimS1 { get; set; }
        public double yuzdedegisimS2 { get; set; }
        public double yuzdedegisim { get; set; }
        public double fiyatadimi { get; set; }
        public int kaykar { get; set; }
        public long sermaye { get; set; }
        public double saklamaor { get; set; }
        public long netkar { get; set; }
        public double net { get; set; }
        public double fiyatkaz { get; set; }
        public double piydeg { get; set; }
        public object kapanisfark { get; set; }
        public string donem { get; set; }
        public long ozsermaye { get; set; }
        public double beta { get; set; }
        public double xU100AG { get; set; }
        public string aciklama { get; set; }
    }

    public class ApiStockResponse
    {
        public string code { get; set; }
        public StockData data { get; set; }
    }
}
