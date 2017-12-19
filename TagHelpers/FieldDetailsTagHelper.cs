using System;
using Lexor.Utilities.Extensions;
using Lexor.Utilities.SeedWork;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Lexor.Utilities.TagHelpers
{
    [HtmlTargetElement("field-details", Attributes = ForAttributeName)]
    public class FieldDetailsTagHelper : TagHelper
    {
        private const string ForAttributeName = "asp-for";

        [HtmlAttributeName(ForAttributeName)]
        public ModelExpression For { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var html = $"<tr><th>{DisplayName}</th><td>{ValueText(For.Model)}</td></tr>";
            output.TagName = "";
            output.Content.SetHtmlContent(html);
        }

        private string DisplayName
        {
            get
            {
                var displayName = For.Metadata.DisplayName;
                if (string.IsNullOrEmpty(displayName))
                {
                    // If there's no DisplayName set, try to make it look nice by splitting up "CamelCaseName" to "Camel Case Name"
                    // Need to first remove leading parents i.e. anything before the last '.'
                    displayName = For.Name.Substring(For.Name.LastIndexOf('.') + 1).SplitCamelCase();
                }
                return displayName;
            }
        }

        private static string ValueText(object value)
        {
            if (value == null) return "";

            switch (value)
            {
                case bool b:
                    return b.FormatYesNo();

                case DateTime d:
                    return d.Format();

                case Enumeration e:
                    return e.Name;

                default:
                    return value.ToString();
            }
        }
    }
}
