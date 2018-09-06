using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;
using System.Linq;

namespace Utilities.Extensions
{
    // ReSharper disable once InconsistentNaming
    public static class ITempDataDictionaryExtensions
    {
        private enum AlertMessageType
        {
            Info,
            Success,
            Warning,
            Danger
        }

        private const string AlertPrefix = "Alert";

        private static void AddAlertMessage(this ITempDataDictionary tempData, AlertMessageType alertMessageType, string message)
        {
            tempData[AlertPrefix + alertMessageType] = message;
        }

        public static void AddInfoMessage(this ITempDataDictionary tempData, string message) => AddAlertMessage(tempData, AlertMessageType.Info, message);
        public static void AddSuccessMessage(this ITempDataDictionary tempData, string message) => AddAlertMessage(tempData, AlertMessageType.Success, message);
        public static void AddWarningMessage(this ITempDataDictionary tempData, string message) => AddAlertMessage(tempData, AlertMessageType.Warning, message);
        public static void AddErrorMessage(this ITempDataDictionary tempData, string message) => AddAlertMessage(tempData, AlertMessageType.Danger, message);

        public static IEnumerable<(string, string)> GetAlertMessages(this ITempDataDictionary tempData)
        {
            var alertMessages = tempData.Keys.Where(k => k.StartsWith(AlertPrefix)).ToList();
            foreach (var alertMessage in alertMessages)
            {
                yield return (alertMessage.Replace(AlertPrefix, "").ToLower(), (string)tempData[alertMessage]);
            }
        }
    }
}
