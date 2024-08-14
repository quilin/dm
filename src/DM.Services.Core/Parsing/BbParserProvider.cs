using System;
using System.Collections.Generic;
using BBCodeParser;
using BBCodeParser.Tags;

namespace DM.Services.Core.Parsing;

/// <inheritdoc />
public class BbParserProvider : IBbParserProvider
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

    private static readonly Tag Strong = new("b", "<strong>", "</strong>");
    private static readonly Tag Italic = new("i", "<em>", "</em>");
    private static readonly Tag Underlined = new("u", "<u>", "</u>");
    private static readonly Tag Strike = new("s", "<s>", "</s>");
    private static readonly Tag Preformatted = new("pre", $"<pre class=\"{CodeClassName}\">", "</pre>");
    private static readonly ListTag OrderedList = new("ol", "<ol>", "</ol>");
    private static readonly ListTag UnorderedList = new("ul", "<ul>", "</ul>");
    private static readonly Tag ListItem = new("li", "<li>", "</li>");

    private static readonly Tag Head = new("head", $"<h4 class=\"{HeaderClassName}\">", "</h4>");

    private static readonly Tag Spoiler = new("spoiler",
        $"<a href=\"javascript:void(0)\" class=\"{SpoilerHeadClassName}\" data-swaptext=\"Скрыть содержимое\">Показать содержимое</a><div class=\"{SpoilerClassName}\">",
        "</div>");

    private static readonly Tag Quote = new("quote",
        $"<div class=\"{QuoteClassName}\"><div class=\"{QuoteHeaderClassName}\">{{value}}</div>", "</div>", true,
        false);

    private static readonly Tag Image = new("img",
        $"<a href=\"{{value}}\" target=\"_blank\"><img src=\"{{value}}\" class=\"{ImageClassName}\" /></a>", true);

    private static readonly Tag Link = new("link", "<a href=\"{value}\">", "</a>", true);

    private static readonly Tag SafeImage = new("img",
        $"<a href=\"javascript:void(0)\" class=\"{SpoilerHeadClassName}\" data-swaptext=\"Скрыть изображение\">Показать изображение</a><div class=\"{SpoilerClassName}\"><a href=\"{{value}}\" target=\"_blank\"><img src=\"{{value}}\" class=\"{ImageClassName}\" /></a></div>",
        true);

    private static readonly Tag Tab = new("tab", "&nbsp;&nbsp;&nbsp;");

    private static readonly CodeTag Code = new("code", $"<pre class=\"{CodeClassName}\">", "</pre>");

    private static readonly Tag Private = new("private", $"<div class=\"{PrivateClassName}\">",
        $"</div><div class=\"{PrivateHeaderClassName}\">Получатели: {{value}}</div>", true, false);

    private static readonly Dictionary<string, string> CommonSubstitutions = new()
    {
        {"---", "&mdash;"},
        {"--", "&ndash;"},
        {"\n", "<br />"}
    };

    private static readonly Dictionary<string, string> ConversationMessageSubstitutions =
        new()
        {
            {"---", "&mdash;"},
            {"--", "&ndash;"},
        };

    private static readonly Dictionary<string, string> InfoSubstitutions = new()
    {
        {"---", "&mdash;"},
        {"--", "&ndash;"},
        {"\n___", "<hr />"},
        {"\n", "<br />"},
    };

    private static readonly Dictionary<string, string> SafeSubstitutions = new()
    {
        {"---", "&mdash;"},
        {"--", "&ndash;"},
        {"\n", "<br />"}
    };

    private static TagSetBuilder DefaultTags => new(new[]
    {
        Strong, Italic, Underlined, Strike,
        Preformatted, Spoiler, Quote, Image,
        UnorderedList, OrderedList, ListItem,
        Link, Tab, Code
    });

    private static TagSetBuilder DefaultSafeTags => DefaultTags.Without(Preformatted, Image).With(SafeImage);

    private static readonly Lazy<IBbParser> CommonParser = new(() =>
        new BbParser(DefaultTags.Build(),
            BbParser.SecuritySubstitutions, CommonSubstitutions));

    private static readonly Lazy<IBbParser> PostParser = new(
        new BbParser(DefaultTags.With(Private).Build(),
            BbParser.SecuritySubstitutions, CommonSubstitutions));

    private static readonly Lazy<IBbParser> InfoParser = new(
        new BbParser(DefaultTags.With(Head).Build(),
            BbParser.SecuritySubstitutions, InfoSubstitutions));

    private static readonly Lazy<IBbParser> ConversationMessageParser = new(
        new BbParser(DefaultTags.Build(),
            BbParser.SecuritySubstitutions, ConversationMessageSubstitutions));

    private static readonly Lazy<IBbParser> GeneralChatMessageParser = new(
        new BbParser(DefaultSafeTags.With(Preformatted).Build(),
            BbParser.SecuritySubstitutions, CommonSubstitutions));

    private static readonly Lazy<IBbParser> SafePostParser = new(
        new BbParser(DefaultSafeTags.With(Private).Build(),
            BbParser.SecuritySubstitutions, SafeSubstitutions));

    private static readonly Lazy<IBbParser> SafeRatingParser = new(
        new BbParser(DefaultSafeTags.Build(),
            BbParser.SecuritySubstitutions, SafeSubstitutions));

    /// <inheritdoc />
    public IBbParser CurrentCommon => CommonParser.Value;

    /// <inheritdoc />
    public IBbParser CurrentInfo => InfoParser.Value;

    /// <inheritdoc />
    public IBbParser CurrentConversationMessage => ConversationMessageParser.Value;

    /// <inheritdoc />
    public IBbParser CurrentPost => PostParser.Value;

    /// <inheritdoc />
    public IBbParser CurrentSafePost => SafePostParser.Value;

    /// <inheritdoc />
    public IBbParser CurrentSafeRating => SafeRatingParser.Value;

    /// <inheritdoc />
    public IBbParser CurrentGeneralChat => GeneralChatMessageParser.Value;
}