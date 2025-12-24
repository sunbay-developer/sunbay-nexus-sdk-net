using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Sunbay.Nexus.Sdk.Utilities
{
    /// <summary>
    /// Helper class for generating User-Agent header value
    /// Format: SunbayNexusSDK-{Language}/{SDKVersion} {Language}/{LanguageVersion} {OS}/{OSVersion}
    /// Example: SunbayNexusSDK-CSharp/1.0.0 .NET/8.0.0 Darwin/25.1.0
    /// </summary>
    internal static class UserAgentHelper
    {
        private static readonly Lazy<string> _userAgent = new Lazy<string>(BuildUserAgent, System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);
        
        /// <summary>
        /// Gets the User-Agent string for the SDK
        /// </summary>
        public static string UserAgent => _userAgent.Value;
        
        /// <summary>
        /// Builds the User-Agent string dynamically
        /// </summary>
        private static string BuildUserAgent()
        {
            var sdkVersion = GetSdkVersion();
            var languageVersion = GetLanguageVersion();
            var osInfo = GetOperatingSystemInfo();
            
            return $"SunbayNexusSDK-CSharp/{sdkVersion} {languageVersion} {osInfo}";
        }
        
        /// <summary>
        /// Gets SDK version from assembly metadata
        /// Priority: AssemblyInformationalVersionAttribute > AssemblyFileVersionAttribute > AssemblyVersionAttribute
        /// </summary>
        private static string GetSdkVersion()
        {
            try
            {
                var assembly = typeof(UserAgentHelper).Assembly;
                
                // Try AssemblyInformationalVersionAttribute first (NuGet package version)
#if NETSTANDARD2_0
                var informationalVersionAttr = assembly.GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute), false)
                    .FirstOrDefault() as AssemblyInformationalVersionAttribute;
#else
                var informationalVersionAttr = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
#endif
                if (informationalVersionAttr != null && !string.IsNullOrWhiteSpace(informationalVersionAttr.InformationalVersion))
                {
                    // Extract version number (remove any suffix like +sha.abc123)
                    var version = informationalVersionAttr.InformationalVersion;
                    var plusIndex = version.IndexOf('+');
                    if (plusIndex >= 0)
                    {
#if NETSTANDARD2_0
                        version = version.Substring(0, plusIndex);
#else
                        version = version[..plusIndex];
#endif
                    }
                    if (!string.IsNullOrWhiteSpace(version))
                    {
                        return version;
                    }
                }
                
                // Fallback to AssemblyFileVersionAttribute
#if NETSTANDARD2_0
                var fileVersionAttr = assembly.GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false)
                    .FirstOrDefault() as AssemblyFileVersionAttribute;
#else
                var fileVersionAttr = assembly.GetCustomAttribute<AssemblyFileVersionAttribute>();
#endif
                if (fileVersionAttr != null && !string.IsNullOrWhiteSpace(fileVersionAttr.Version))
                {
                    return fileVersionAttr.Version;
                }
                
                // Fallback to AssemblyVersionAttribute
                var assemblyVersion = assembly.GetName().Version;
                if (assemblyVersion != null)
                {
                    return assemblyVersion.ToString();
                }
            }
            catch
            {
                // Ignore exceptions and fall back to default
            }
            
            return "1.0.0";
        }
        
        /// <summary>
        /// Gets C#/.NET runtime version dynamically
        /// Format: CSharp/{version} (e.g., CSharp/8.0.0)
        /// </summary>
        private static string GetLanguageVersion()
        {
            try
            {
                // Use RuntimeInformation.FrameworkDescription for detailed framework info
                // Example: ".NET 8.0.0" or ".NET Framework 4.8.0"
                var frameworkDescription = RuntimeInformation.FrameworkDescription;
                
                // Extract version number from framework description
                // Format: ".NET 8.0.0" or ".NET Framework 4.8.0"
                var parts = frameworkDescription.Split(' ');
                if (parts.Length >= 2)
                {
                    var version = parts[parts.Length - 1];
                    // Use "CSharp" as language name to match format: SunbayNexusSDK-CSharp/1.0.0 CSharp/8.0.0
                    return $"CSharp/{version}";
                }
                
                // Fallback to Environment.Version (CLR version)
                var clrVersion = Environment.Version;
                return $"CSharp/{clrVersion.Major}.{clrVersion.Minor}.{clrVersion.Build}";
            }
            catch
            {
                // Fallback to Environment.Version
                try
                {
                    var version = Environment.Version;
                    return $"CSharp/{version.Major}.{version.Minor}.{version.Build}";
                }
                catch
                {
                    return "CSharp/Unknown";
                }
            }
        }
        
        /// <summary>
        /// Gets operating system information dynamically
        /// </summary>
        private static string GetOperatingSystemInfo()
        {
            try
            {
                // Use RuntimeInformation.OSDescription for cross-platform OS info
                // Example: "Darwin 25.1.0", "Linux 5.4.0", "Microsoft Windows 10.0.19042"
                var osDescription = RuntimeInformation.OSDescription;
                
                // Parse OS description to extract OS name and version
                // Format varies by OS:
                // - Darwin: "Darwin 25.1.0"
                // - Linux: "Linux 5.4.0-74-generic"
                // - Windows: "Microsoft Windows 10.0.19042"
                
                var parts = osDescription.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length >= 2)
                {
                    // For Windows, format is "Microsoft Windows 10.0.19042"
                    if (parts[0] == "Microsoft" && parts.Length >= 3)
                    {
                        var osName = parts[1]; // "Windows"
                        var osVersion = parts[2]; // "10.0.19042"
                        return $"{osName}/{osVersion}";
                    }
                    // For Unix-like systems (Darwin, Linux), format is "OSName Version"
                    else
                    {
                        var osName = parts[0]; // "Darwin" or "Linux"
                        var osVersion = parts[1]; // "25.1.0" or "5.4.0-74-generic"
                        return $"{osName}/{osVersion}";
                    }
                }
                
                // Fallback: use OSDescription as-is
                return osDescription.Replace(' ', '/');
            }
            catch
            {
                // Fallback to Environment.OSVersion
                try
                {
                    var osVersion = Environment.OSVersion;
                    var platform = osVersion.Platform.ToString();
                    var version = osVersion.Version.ToString();
                    return $"{platform}/{version}";
                }
                catch
                {
                    return "Unknown/Unknown";
                }
            }
        }
    }
}

