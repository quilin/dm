using System.Collections.Generic;
using BBCodeParser;
using BBCodeParser.Tags;

namespace DM.Web.Core.Parsers
{
    public class BbParserFactory : IBbParserFactory
    {
        private const string CodeClassName = "code";
        private const string SpoilerHeadClassName = "spoiler-head";
        private const string SpoilerClassName = "spoiler";
        private const string ImageClassName = "image";
        private const string QuoteClassName = "quote";
        private const string QuoteHeaderClassName = "quote-author";
        private const string HeaderClassName = "info-head";
        private const string PrivateClassName = "private-message";
        private const string PrivateHeaderClassName = "private-message-header";

        private static readonly Tag Strong = new Tag("b", "<strong>", "</strong>");
        private static readonly Tag Italic = new Tag("i", "<em>", "</em>");
        private static readonly Tag Underlined = new Tag("u", "<u>", "</u>");
        private static readonly Tag Strike = new Tag("s", "<s>", "</s>");
        private static readonly Tag Preformatted = new Tag("pre", $"<pre class=\"{CodeClassName}\">", "</pre>");
        private static readonly ListTag OrderedList = new ListTag("ol", "<ol>", "</ol>");
        private static readonly ListTag UnorderedList = new ListTag("ul", "<ul>", "</ul>");
        private static readonly Tag ListItem = new Tag("li", "<li>", "</li>");

        private static readonly Tag Head = new Tag("head", $"<h4 class=\"{HeaderClassName}\">", "</h4>");

        private static readonly Tag Spoiler = new Tag("spoiler",
            $"<a href=\"javascript:void(0)\" class=\"{SpoilerHeadClassName}\" data-swaptext=\"Скрыть содержимое\">Показать содержимое</a><div class=\"{SpoilerClassName}\">",
            "</div>");

        private static readonly Tag Quote = new Tag("quote",
            $"<div class=\"{QuoteClassName}\"><div class=\"{QuoteHeaderClassName}\">{{value}}</div>", "</div>", true,
            false);

        private static readonly Tag Image = new Tag("img",
            $"<a href=\"{{value}}\" target=\"_blank\"><img src=\"{{value}}\" class=\"{ImageClassName}\" /></a>", true);

        private static readonly Tag Link = new Tag("link", "<a href=\"{value}\">", "</a>", true);

        private static readonly Tag SafeImage = new Tag("img",
            $"<a href=\"javascript:void(0)\" class=\"{SpoilerHeadClassName}\" data-swaptext=\"Скрыть изображение\">Показать изображение</a><div class=\"{SpoilerClassName}\"><a href=\"{{value}}\" target=\"_blank\"><img src=\"{{value}}\" class=\"{ImageClassName}\" /></a></div>",
            true);

        private static readonly Tag Tab = new Tag("tab", "&nbsp;&nbsp;&nbsp;");

        private static readonly CodeTag Code = new CodeTag("code", $"<pre class=\"{CodeClassName}\">", "</pre>");

        private static readonly Tag Private = new Tag("private", $"<div class=\"{PrivateClassName}\">",
            $"</div><div class=\"{PrivateHeaderClassName}\">Получатели: {{value}}</div>", true, false);

        private static readonly Dictionary<string, string> CommonSubstitutions = new Dictionary<string, string>
        {
            {"---", "&mdash;"},
            {"--", "&ndash;"},
            {"\r\n", "<br />"}
        };

        private static readonly Dictionary<string, string> ConversationMessageSubstitutions =
            new Dictionary<string, string>
            {
                {"---", "&mdash;"},
                {"--", "&ndash;"},
            };

        private static readonly Dictionary<string, string> InfoSubstitutions = new Dictionary<string, string>
        {
            {"---", "&mdash;"},
            {"--", "&ndash;"},
            {"\r\n___\r\n", "<hr />"},
            {"\r\n", "<br />"},
        };

        private static readonly Dictionary<string, string> SafeSubstitutions = new Dictionary<string, string>
        {
            {"---", "&mdash;"},
            {"--", "&ndash;"},
            {"\r\n", "<br />"}
            // TODO: матофильтр
        };

        private static TagSet DefaultTags => new TagSet(new[]
        {
            Strong, Italic, Underlined, Strike,
            Preformatted, Spoiler, Quote, Image,
            UnorderedList, OrderedList, ListItem,
            Link, Tab, Code
        });

        private static TagSet DefaultSafeTags => DefaultTags.Without(Preformatted, Image).With(SafeImage);

        // base and default parser, used for everything
        private static readonly IBbParser CommonParser =
            new BbParser(DefaultTags.ToArray(), BbParser.SecuritySubstitutions, CommonSubstitutions);

        // only used inside modules
        private static readonly IBbParser PostParser = new BbParser(DefaultTags.With(Private).ToArray(),
            BbParser.SecuritySubstitutions, CommonSubstitutions);

        // only used inside module infos
        private static readonly IBbParser InfoParser = new BbParser(DefaultTags.With(Head).ToArray(),
            BbParser.SecuritySubstitutions, InfoSubstitutions);

        // only used on conversations list page
        private static readonly IBbParser ConversationMessageParser = new BbParser(DefaultTags.ToArray(),
            BbParser.SecuritySubstitutions, ConversationMessageSubstitutions);

        private static readonly IBbParser GeneralChatMessageParser =
            new BbParser(DefaultSafeTags.With(Preformatted).ToArray(), BbParser.SecuritySubstitutions,
                CommonSubstitutions);

        // only used for Best Post of the Week
        private static readonly IBbParser SafePostParser = new BbParser(DefaultSafeTags.With(Private).ToArray(),
            BbParser.SecuritySubstitutions, SafeSubstitutions);

        private static readonly IBbParser SafeRatingParser =
            new BbParser(DefaultSafeTags.ToArray(), BbParser.SecuritySubstitutions, SafeSubstitutions);

        public IBbParser CreateConversationMessage()
        {
            return ConversationMessageParser;
        }

        public IBbParser CreateCommon()
        {
            return CommonParser;
        }

        public IBbParser CreateInfo()
        {
            return InfoParser;
        }

        public IBbParser CreatePost()
        {
            return PostParser;
        }

        public IBbParser CreateSafePost()
        {
            return SafePostParser;
        }

        public IBbParser CreateSafeRating()
        {
            return SafeRatingParser;
        }

        public IBbParser CreateGeneralChat()
        {
            return GeneralChatMessageParser;
        }
    }
}