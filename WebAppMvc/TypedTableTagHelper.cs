using System;
using System.Drawing;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Routing;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace WebAppMvc;


public class EmailTagHelper : TagHelper {
    private const string EmailDomain = "contoso.com";

    public string MailTo { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output) {
        output.TagName = "a";    // Replaces <email> with <a> tag

        var address = MailTo + "@" + EmailDomain;
        output.Attributes.SetAttribute("href", "mailto:" + address);
        output.Content.SetContent(address);
    }
}


[HtmlTargetElement("tablev1", Attributes = "for")]
public class DataTableTagHelper : TagHelper {
    [HtmlAttributeName("for")]
    public ModelExpression For { get; set; }

    public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {

        output.TagName = "table";
        output.TagMode = TagMode.StartTagAndEndTag;

        output.Content.AppendHtml("<table class=\"table\">");
        output.Content.AppendHtml("<thead>");
        output.Content.AppendHtml("<tr>");
        
        foreach (var prop in For.Metadata.ElementMetadata.Properties) {
                output.Content.AppendHtml("<th>");
                output.Content.AppendHtml(prop.Name);
        }

        output.Content.AppendHtml("<th>");
        output.Content.AppendHtml("Actions");

        output.Content.AppendHtml("</tr>");
        output.Content.AppendHtml("</thead>");

        output.Content.AppendHtml("<tbody>");

        foreach (var item in For.Model as System.Collections.IEnumerable) {
            output.Content.AppendHtml("<tr>");
            foreach (var prop in For.Metadata.ElementMetadata.Properties) {
                output.Content.AppendHtml("<td>");
                output.Content.AppendHtml(prop.PropertyGetter(item).ToString());
            }

            output.Content.AppendHtml("<td>");
            output.Content.AppendHtml($"<a asp-action = \"Edit\" asp-route-id = \"{For.Metadata.ElementMetadata.Properties.First().PropertyGetter(item)}\" class=\"icon-link\" title=\"Edit\"><i class=\"bi bi-pencil-square\"></i></a>");
            output.Content.AppendHtml($"<a asp-action = \"Delete\" asp-route-id = \"{For.Metadata.ElementMetadata.Properties.First().PropertyGetter(item)}\" class=\"icon-link\" title=\"Delete\"><i class=\"bi bi-trash\"></i></a>");
            output.Content.AppendHtml("</td>");

            output.Content.AppendHtml("</tbody>");
            output.Content.AppendHtml("</tr>");
        }

        
        output.Content.AppendHtml("</table>");

        return Task.CompletedTask;
    }
}


[HtmlTargetElement("tablev2", Attributes = "model")]
public class TypedTableTagHelper<TModel> : TagHelper {
    [HtmlAttributeName("model")]
    public IEnumerable<TModel>? Model { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output) {
        output.TagName = "table";
        output.TagMode = TagMode.StartTagAndEndTag;

        var headersBuilder = new StringBuilder("<thead><tr>");
        var bodyBuilder = new StringBuilder("<tbody>");

        PropertyInfo[] properties = typeof(TModel).GetProperties();

        foreach (var prop in properties) {
            headersBuilder.Append($"<th>{prop.Name}</th>");
        }

        headersBuilder.Append("</tr></thead>");

        foreach (var item in Model) {
            bodyBuilder.Append("<tr>");
            foreach (var prop in properties) {
                var value = prop.GetValue(item, null);
                bodyBuilder.Append($"<td>{value}</td>");
            }
            bodyBuilder.Append("</tr>");
        }

        bodyBuilder.Append("</tbody>");

        output.Content.AppendHtml(headersBuilder.ToString());
        output.Content.AppendHtml(bodyBuilder.ToString());
    }
}
