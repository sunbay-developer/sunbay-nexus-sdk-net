using System.Runtime.Serialization;

namespace Sunbay.Nexus.Sdk.Models.Enums
{
    /// <summary>
    /// Payment category enum
    /// </summary>
    public enum PaymentCategory
    {
        /// <summary>
        /// Card payment
        /// </summary>
        [EnumMember(Value = "CARD")]
        Card,
        
        /// <summary>
        /// Credit card network
        /// </summary>
        [EnumMember(Value = "CARD-CREDIT")]
        CardCredit,
        
        /// <summary>
        /// Debit card network
        /// </summary>
        [EnumMember(Value = "CARD-DEBIT")]
        CardDebit,
        
        /// <summary>
        /// QR code merchant presented mode
        /// </summary>
        [EnumMember(Value = "QR-MPM")]
        QrMpm,
        
        /// <summary>
        /// QR code customer presented mode
        /// </summary>
        [EnumMember(Value = "QR-CPM")]
        QrCpm
    }
}
