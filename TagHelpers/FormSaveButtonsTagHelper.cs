using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Lexor.Utilities.TagHelpers
{
    [HtmlTargetElement("form-save-buttons", Attributes = CancelPageAttributeName)]
    public class FormSaveButtonsTagHelper : NestableTagHelper
    {
        private const string CancelPageAttributeName = "asp-cancel-page";

        [HtmlAttributeName(CancelPageAttributeName)]
        public string CancelPage { get; set; }

        public FormSaveButtonsTagHelper(IHtmlGenerator htmlGenerator, HtmlEncoder htmlEncoder) : base(htmlGenerator, htmlEncoder) { }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var cancel = await BuildLinkHtml("Cancel", CancelPage,
                new TagHelperAttributeList
                {
                    { "class", "btn btn-info" }
                });

            var html = $@"<div class='col-sm-2'></div>
<div class='col-sm-10'>
    {cancel}
    <button class='btn btn-primary'>Save</button>
</div>";

            output.TagName = "div";
            output.Attributes.Add("class", "form-group");
            output.Content.SetHtmlContent(html);
        }
    }
}
