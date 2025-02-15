﻿using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models;

namespace Our.Umbraco.TagHelpers
{
    [HtmlTargetElement("our-link")]
    public class LinkTagHelper : TagHelper
    {
        [HtmlAttributeName("Link")]
        public Link? Link { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            // Ensure we have a Url set on the LinkPicker property editor in Umbraco
            if (string.IsNullOrWhiteSpace(Link?.Url))
            {
                output.SuppressOutput();
                return;
            }

            // If the <our-link /> is self closing
            // Ensure that our <a></a> always has a matching end tag
            output.TagMode = TagMode.StartTagAndEndTag;

            output.TagName = "a";
            var childContent = await output.GetChildContentAsync();

            // If we use the TagHelper <umb-link></umb-link>
            // Without child DOM elements then it will use the Link Name property inside the <a> it generates
            if (childContent.IsEmptyOrWhiteSpace)
            {
                output.Content.SetContent(Link.Name);
            }

            // Set the HREF of the <a>
            output.Attributes.SetAttribute("href", Link.Url);

            if (string.IsNullOrWhiteSpace(Link.Target))
            {
                return;
            }

            // Set the <a target=""> attribute such as _blank etc...
            output.Attributes.SetAttribute("target", Link.Target);

            // If the target is _blank & not an internal picked content node & external
            // Ensure we set the <a rel="noopener"> attribute
            if (Link.Target == "_blank" && Link.Type == LinkType.External)
            {
                output.Attributes.SetAttribute("rel", "noopener");
            }
        }
    }
}
