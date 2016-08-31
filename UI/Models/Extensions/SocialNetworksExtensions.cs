using System.Text;
using System.Web;
using System.Web.Mvc;

namespace UI.Models.Extensions
{
	public static class SocialNetworksExtensions
	{
		public static MvcHtmlString TwitterLink(this HtmlHelper helper, string title,bool showCount=false, string url = null)
		{
			var socialLink = new TagBuilder("a");
			socialLink.Attributes.Add("href","https://twitter.com/share");
			socialLink.Attributes.Add("class","twitter-share-button");
			socialLink.Attributes.Add("data-via", "a1771550Kevin");
			socialLink.Attributes.Add("data-count", showCount ? "horizontal" : "none");
			socialLink.Attributes.Add("data-text", title);
			socialLink.Attributes.Add("data-url", url ?? HttpContext.Current.Request.Url.AbsolutePath);
			socialLink.SetInnerText("Tweet");
			
			return new MvcHtmlString(socialLink.ToString(TagRenderMode.Normal));
		}

		public static MvcHtmlString FacebookLink(this HtmlHelper helper, string title, bool showCount=false, string url = null)
		{
			var builder = new StringBuilder();
			var socialLink = new TagBuilder("div");
			socialLink.Attributes.Add("class","fb-like");
			socialLink.Attributes.Add("data-send","false");
			socialLink.Attributes.Add("data-stream","false");
			socialLink.Attributes.Add("data-show-faces","false");
			socialLink.Attributes.Add("data-layout",showCount?"button_count":"button");
			socialLink.Attributes.Add("data-font", "arial");
			socialLink.Attributes.Add("data-href", url ?? HttpContext.Current.Request.Url.AbsolutePath);

			builder.Append(socialLink.ToString(TagRenderMode.Normal));
			return new MvcHtmlString(builder.ToString());
		}

		public static MvcHtmlString GooglePlusOneLink(this HtmlHelper helper, string title, bool showCount=false, string url = null, string callback=null)
		{
			var socialLink = new TagBuilder("div");
			socialLink.Attributes.Add("class","g-plusone");
			socialLink.Attributes.Add("data-size","medium");
			socialLink.Attributes.Add("data-annotation",showCount?"inline":"none");
			socialLink.Attributes.Add("data-href", url ?? HttpContext.Current.Request.Url.AbsolutePath);
			socialLink.Attributes.Add("data-callback", callback);
			return new MvcHtmlString(socialLink.ToString(TagRenderMode.Normal));
		}

		public static MvcHtmlString AllSocialNetworkLinks(this HtmlHelper helper, string title, bool showCount=false, string url = null, string googleCallback=null)
		{
			var builder = new StringBuilder();
			var ul = new TagBuilder("ul");
			ul.AddCssClass("social");

			// Google
			var google = new TagBuilder("li");
			google.InnerHtml = helper.GooglePlusOneLink(title,showCount, url, googleCallback).ToHtmlString();
			google.AddCssClass("social-google");
			ul.InnerHtml += google.ToString();

			// Twitter
			var twitter = new TagBuilder("li");
			twitter.InnerHtml = helper.TwitterLink(title,showCount,url).ToHtmlString();
			twitter.AddCssClass("social-twitter");
			ul.InnerHtml += twitter.ToString();

			// Facebook
			var facebook = new TagBuilder("li");
			facebook.InnerHtml = helper.FacebookLink(title,showCount,url).ToHtmlString();
			facebook.AddCssClass("social-facebook");
			ul.InnerHtml += facebook.ToString();

			builder.Append(ul.ToString(TagRenderMode.Normal));
			return new MvcHtmlString(builder.ToString());
		}
	}
}