using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Interview.Models
{
    //TODO: move enums elsewhere
    //TODO: consider changing Id from string to Guid
    
    public enum TransactionType
    {
        [EnumMember(Value = "Debit")]
        Debit,
        [EnumMember(Value = "Credit")]
        Credit
    }

    public enum TransactionSummary
    {
        [EnumMember(Value = "Payment")]
        Payment,
        [EnumMember(Value = "Refund")]
        Refund
    }

    public class Transaction
    {
        public string Id { get; set; }
        public int ApplicationId { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public TransactionType Type { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public TransactionSummary Summary { get; set; }
        public decimal Amount { get; set; }
        public DateTime PostingDate { get; set; }
        public bool IsCleared { get; set; }
        public DateTime? ClearedDate { get; set; }
    }
}