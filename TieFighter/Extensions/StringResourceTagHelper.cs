using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TieFighter.Services;

namespace TieFighter.Extensions.TagHelpers
{
    [HtmlTargetElement("String-Resource")]
    public class StringResourceTagHelper : TagHelper
    {
        public StringResourceTagHelper(IStringResourceService stringResourceService)
        {
            _stringResourceService = stringResourceService;
        }

        private IStringResourceService _stringResourceService;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagMode = TagMode.StartTagAndEndTag;
            output.TagName = "div";
            output.Content.SetContent(_stringResourceService[context.TagName]);
        }
    }
}
