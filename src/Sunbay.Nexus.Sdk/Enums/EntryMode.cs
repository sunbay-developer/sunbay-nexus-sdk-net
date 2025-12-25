using System.Runtime.Serialization;

namespace Sunbay.Nexus.Sdk.Enums
{
    /// <summary>
    /// Entry mode enum
    /// </summary>
    public enum EntryMode
    {
        /// <summary>
        /// Manual entry
        /// </summary>
        [EnumMember(Value = "MANUAL")]
        Manual,
        
        /// <summary>
        /// Swipe card
        /// </summary>
        [EnumMember(Value = "SWIPE")]
        Swipe,
        
        /// <summary>
        /// Fallback swipe
        /// </summary>
        [EnumMember(Value = "FALLBACK_SWIPE")]
        FallbackSwipe,
        
        /// <summary>
        /// Contact chip
        /// </summary>
        [EnumMember(Value = "CONTACT")]
        Contact,
        
        /// <summary>
        /// Contactless
        /// </summary>
        [EnumMember(Value = "CONTACTLESS")]
        Contactless
    }
}

