using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Lexor.Utilities.TagHelpers
{
    /// <inheritdoc />
    /// <summary>
    /// The logic behind this tag helper was largely influenced by this article: https://goo.gl/4b3EJ4
    /// </summary>
    public abstract class NestableTagHelper : TagHelper
    {
        protected const string ForAttributeName = "asp-for";

        [HtmlAttributeName(ForAttributeName)]
        public ModelExpression For { get; set; }

        [ViewContext]
        public ViewContext ViewContext { get; set; }

        protected IHtmlGenerator HtmlGenerator { get; }
        protected HtmlEncoder HtmlEncoder { get; }

        protected NestableTagHelper(IHtmlGenerator htmlGenerator, HtmlEncoder htmlEncoder)
        {
            HtmlGenerator = htmlGenerator;
            HtmlEncoder = htmlEncoder;
        }

        protected async Task<string> GetGeneratedContent(string tagName, TagMode tagMode, ITagHelperComponent tagHelper, TagHelperAttributeList attributes = null)
        {
            if (attributes == null)
            {
                attributes = new TagHelperAttributeList();
            }

            var output = new TagHelperOutput(tagName, attributes, (arg1, arg2) =>
            {
                return Task.Factory.StartNew<TagHelperContent>(() => new DefaultTagHelperContent());
            })
            {
                TagMode = tagMode
            };
            var context = new TagHelperContext(attributes, new Dictionary<object, object>(), Guid.NewGuid().ToString());

            tagHelper.Init(context);
            await tagHelper.ProcessAsync(context, output);

            using (var writer = new StringWriter())
            {
                output.WriteTo(writer, HtmlEncoder);
                return writer.ToString();
            }
        }

        #region Html Builders


        protected async Task<string> BuildLinkHtml(string caption, string page, TagHelperAttributeList attributes = null)
        {
            var helper = new AnchorTagHelper(HtmlGenerator)
            {
                Page = page,
                ViewContext = ViewContext
            };
            var html = await GetGeneratedContent("a", TagMode.StartTagAndEndTag, helper, attributes);
            return html.Replace("></a>", $">{caption}</a>"); //TODO: Figure out how to elegantly set the link caption
        }

        protected async Task<string> BuildLabelHtml(TagHelperAttributeList attributes = null)
        {
            var helper = new LabelTagHelper(HtmlGenerator)
            {
                For = For,
                ViewContext = ViewContext
            };
            return await GetGeneratedContent("label", TagMode.StartTagAndEndTag, helper, attributes);
        }

        protected async Task<string> BuildInputHtml(string inputType, IEnumerable<SelectListItem> items = null, TagHelperAttributeList attributes = null)
        {
            if (items != null) return await BuildSelectListHtml(items, attributes);

            var helper = GetInputTagHelper(inputType);
            return await GetGeneratedContent(inputType, TagMode.StartTagAndEndTag, helper, attributes);
        }

        private TagHelper GetInputTagHelper(string inputType)
        {
            switch (inputType)
            {
                case "textarea":
                    return new TextAreaTagHelper(HtmlGenerator)
                    {
                        For = For,
                        ViewContext = ViewContext
                    };
                case "input":
                    return new InputTagHelper(HtmlGenerator)
                    {
                        For = For,
                        ViewContext = ViewContext
                    };
                default:
                    throw new ArgumentOutOfRangeException($"Unexpected value '{inputType}' for {nameof(inputType)} in {nameof(GetInputTagHelper)}");
            }
        }

        private async Task<string> BuildSelectListHtml(IEnumerable<SelectListItem> items, TagHelperAttributeList attributes)
        {
            var helper = new SelectTagHelper(HtmlGenerator)
            {
                For = For,
                Items = items,
                ViewContext = ViewContext
            };
            return await GetGeneratedContent("select", TagMode.StartTagAndEndTag, helper, attributes);
        }

        protected async Task<string> BuildValidationMessageHtml(ModelExpression validateOn = null, TagHelperAttributeList attributes = null)
        {
            var helper = new ValidationMessageTagHelper(HtmlGenerator)
            {
                For = validateOn ?? For,
                ViewContext = ViewContext
            };
            return await GetGeneratedContent("span", TagMode.StartTagAndEndTag, helper, attributes);
        }

        #endregion
    }
}
