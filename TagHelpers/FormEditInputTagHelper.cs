using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Lexor.Utilities.TagHelpers
{
    [HtmlTargetElement("form-edit-input", Attributes = ForAttributeName)]
    public class FormEditInputTagHelper : NestableTagHelper
    {
        private const string ValidateOnAttributeName = "asp-validate-on";
        private const string InputTypeAttributeName = "asp-input-type";
        private const string ItemsAttributeName = "asp-items";

        /// <summary>
        /// Can override the validation message to be on a specific field.
        /// 
        /// Note: cannot use the shortcut like asp-for i.e. must specify Model.Property
        /// </summary>
        [HtmlAttributeName(ValidateOnAttributeName)]
        public ModelExpression ValidateOn { get; set; }

        /// <summary>
        /// Can override default of "input" to be something else e.g. "textarea"
        /// </summary>
        [HtmlAttributeName(InputTypeAttributeName)]
        public string InputType { get; set; } = "input";

        /// <summary>
        /// List of select list items; indicates that a select elements should be created
        /// </summary>
        [HtmlAttributeName(ItemsAttributeName)]
        public IEnumerable<SelectListItem> Items { get; set; }

        public FormEditInputTagHelper(IHtmlGenerator htmlGenerator, HtmlEncoder htmlEncoder) : base(htmlGenerator, htmlEncoder) { }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var label = await BuildLabelHtml(new TagHelperAttributeList {
                { "class", "control-label col-sm-2"}
            });
            var input = await BuildInputHtml(InputType, Items, new TagHelperAttributeList {
                { "class", "form-control"}
            });
            var validationMessage = await BuildValidationMessageHtml(ValidateOn, new TagHelperAttributeList {
                { "class", "text-danger"}
            });

            var html = $@"{label}
<div class='col-sm-10'>
    {input}
    {validationMessage}
</div>";

            output.TagName = "div";
            output.Attributes.Add("class", "form-group");
            output.Content.SetHtmlContent(html);
        }
    }
}
