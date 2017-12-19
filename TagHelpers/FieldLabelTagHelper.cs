using Lexor.Utilities.Extensions;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Lexor.Utilities.TagHelpers
{
    [HtmlTargetElement("field-label", Attributes = ForAttributeName)]
    public class FieldLabelTagHelper : TagHelper
    {
        private const string ForAttributeName = "asp-for";
        private const string TagAttributeName = "asp-tag";

        [HtmlAttributeName(ForAttributeName)]
        public ModelExpression For { get; set; }

        [HtmlAttributeName(TagAttributeName)]
        public string Tag { get; set; } = "th";

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = Tag;
            output.Content.SetHtmlContent(DisplayName);
        }

        private string DisplayName
        {
            get
            {
                var displayName = For.Metadata.DisplayName;
                if (string.IsNullOrEmpty(displayName))
                {
                    // If there's no DisplayName set, try to make it look nice by splitting up "CamelCaseName" to "Camel Case Name"
                    displayName = For.Name.SplitCamelCase();
                }
                return displayName;
            }
        }
    }
}
