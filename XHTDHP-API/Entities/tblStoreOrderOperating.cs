using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XHTDHP_API.Entities
{
    public class tblStoreOrderOperating
    {
        public int Id { get; set; }
        public string Vehicle { get; set; }
        public string DriverName { get; set; }
        public string NameDistributor { get; set; }
        public Nullable<double> ItemId { get; set; }
        public string NameProduct { get; set; }
        public string CatId { get; set; }
        public Nullable<decimal> SumNumber { get; set; }
        public Nullable<System.DateTime> TimeIn33 { get; set; }
        public string CardNo { get; set; }
        public Nullable<int> OrderId { get; set; }
        public string DeliveryCode { get; set; }
        public string DeliveryCodeParent { get; set; }
        public Nullable<System.DateTime> OrderDate { get; set; }
        public string TypeProduct { get; set; }
        public Nullable<System.DateTime> TimeIn21 { get; set; }
        public Nullable<System.DateTime> TimeIn22 { get; set; }
        public Nullable<int> Confirm1 { get; set; }
        public Nullable<System.DateTime> TimeConfirm1 { get; set; }
        public Nullable<int> Confirm2 { get; set; }
        public Nullable<System.DateTime> TimeConfirm2 { get; set; }
        public Nullable<int> Confirm3 { get; set; }
        public Nullable<System.DateTime> TimeConfirm3 { get; set; }
        public Nullable<int> Confirm4 { get; set; }
        public Nullable<System.DateTime> TimeConfirm4 { get; set; }
        public Nullable<int> Confirm5 { get; set; }
        public Nullable<System.DateTime> TimeConfirm5 { get; set; }
        public Nullable<int> Confirm6 { get; set; }
        public Nullable<System.DateTime> TimeConfirm6 { get; set; }
        public Nullable<int> Confirm7 { get; set; }
        public Nullable<System.DateTime> TimeConfirm7 { get; set; }
        public Nullable<int> Confirm8 { get; set; }
        public Nullable<System.DateTime> TimeConfirm8 { get; set; }
        public Nullable<int> Confirm9 { get; set; }
        public Nullable<System.DateTime> TimeConfirm9 { get; set; }
        public string Confirm9Note { get; set; }
        public string Confirm9Image { get; set; }
        public Nullable<int> Step { get; set; }
        public Nullable<int> IndexOrder { get; set; }
        public Nullable<int> IndexOrder1 { get; set; }
        public Nullable<int> IndexOrder2 { get; set; }
        public Nullable<int> Trough { get; set; }
        public Nullable<int> Trough1 { get; set; }
        public Nullable<int> NumberVoice { get; set; }
        public string State { get; set; }
        public Nullable<int> Prioritize { get; set; }
        public Nullable<System.DateTime> DayCreate { get; set; }
        public Nullable<int> IDDistributorSyn { get; set; }
        public Nullable<int> AreaId { get; set; }
        public string AreaName { get; set; }
        public string CodeStore { get; set; }
        public string NameStore { get; set; }
        public string DriverUserName { get; set; }
        public Nullable<System.DateTime> DriverAccept { get; set; }
        public Nullable<int> IndexOrderTemp { get; set; }
        public Nullable<int> WeightIn { get; set; }
        public Nullable<System.DateTime> WeightInTime { get; set; }
        public Nullable<int> WeightOut { get; set; }
        public Nullable<System.DateTime> WeightOutTime { get; set; }
        public string NoteFinish { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public Nullable<int> CountReindex { get; set; }
        public Nullable<bool> IsVoiced { get; set; }
        public string LocationCode { get; set; }
        public Nullable<bool> LockInDbet { get; set; }
        public string LogJobAttach { get; set; }
        public Nullable<bool> IsSyncedByNewWS { get; set; }
        public string TroughLineCode { get; set; }
        public Nullable<bool> IsScaleAuto { get; set; }
        public Nullable<System.DateTime> TimeConfirmHistory { get; set; }
        public string LogHistory { get; set; }
        public string MoocCode { get; set; }
        public string LogProcessOrder { get; set; }
        public Nullable<int> DriverPrintNumber { get; set; }
        public Nullable<System.DateTime> DriverPrintTime { get; set; }
        public Nullable<bool> WarningNotCall { get; set; }
        public string XiRoiAttatchmentFile { get; set; }
        public string PackageNumber { get; set; }
        public Nullable<int> Shifts { get; set; }
        public Nullable<bool> AutoScaleOut { get; set; }
        public Nullable<System.DateTime> CreateDay { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateDay { get; set; }
        public string UpdateBy { get; set; }
    }
}