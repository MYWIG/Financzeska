namespace FinanczeskaServerApp.Data
{
    public class StatisticalDataFromS4H
    {
        private Dictionary<string, string> filterNames { get; set; }

        public long DSL_ID { get; set; }
        public string DSL_VATSymbol { get; set; }
        public DateTime DSL_DataOperacji { get; set; }
        public long DSN_ID { get; set; }
        public DateTime DSN_DataDokumentu { get; set; }
        public byte DSN_DzienTygodnia { get; set; }
        public byte DSN_Tydzien { get; set; }
        public long DSR_ID { get; set; }

        public int DSR_SSPID { get; set; }
        public string SSP_Nazwa { get; set; }

        public byte DSR_TORID { get; set; }
        public string TOR_Nazwa { get; set; }

        public byte DSR_TZRID { get; set; }
        public string TZR_Nazwa { get; set; }

        public int DSR_STOID { get; set; }
        public string STO_Nazwa { get; set; }

        public int DSR_OPRID { get; set; }
        public string OPR_Nazwa { get; set; }

        public Int16 SSG_ID { get; set; }
        public string SSG_Nazwa { get; set; }

        public int LOK_ID { get; set; }
        public string LOK_Nazwa { get; set; }

        public Int16 LGR_ID { get; set; }
        public string LGR_Nazwa { get; set; }

        public int KAT_ID { get; set; }
        public string KAT_Nazwa { get; set; }

        public int ART_ID { get; set; }
        public string ART_Nazwa { get; set; }
        public int ART_KRMID { get; set; }
        public string KRM_Nazwa { get; set; }

        public int OPG_ID { get; set; }
        public string OPG_Nazwa { get; set; }

        public byte FPL_ID { get; set; }
        public string FPL_Nazwa { get; set; }

        public decimal DSL_Koszt { get; set; }

        public decimal closedBillGross { get; set; }
        public decimal closedBillNet { get; set; }
        public decimal notClosedBillGross { get; set; }
        public decimal notClosedBillNet { get; set; }
        public decimal totalGross { get; set; }
        public decimal totalNet { get; set; }
        public int notClosedBillCount { get; set; }
        public Int64 ClosedBillIds { get; set; }
        public decimal ART_Count { get; set; }
        public decimal GuestCount { get; set; }
        public decimal wycofaniaGross { get; set; }
        public decimal wycofaniaNet { get; set; }
        public decimal korektyGross { get; set; }
        public decimal korektyNet { get; set; }
        public decimal rabatyGross { get; set; }
        public decimal rabatyNet { get; set; }
        public decimal stratyGross { get; set; }
        public decimal stratyNet { get; set; }
        public decimal serwisyGross { get; set; }
        public decimal serwisyNet { get; set; }
        public byte Godzina { get; set; }
        public Int16 NumerDnia { get; set; }
        public DateTime DataStorna { get; set; }
        public bool? DSL_CzyPoZamowieniu { get; set; }
        public DateTime DSR_DataZamkniecia { get; set; }

    }
}
