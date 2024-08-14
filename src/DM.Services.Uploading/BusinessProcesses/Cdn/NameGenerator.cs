using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DM.Services.Core.Extensions;
using DM.Services.Core.Implementation;
using DM.Services.Uploading.Dto;

namespace DM.Services.Uploading.BusinessProcesses.Cdn;

/// <inheritdoc />
internal class NameGenerator : INameGenerator
{
    private readonly IGuidFactory guidFactory;

    /// <inheritdoc />
    public NameGenerator(
        IGuidFactory guidFactory)
    {
        this.guidFactory = guidFactory;
    }

    private static readonly IDictionary<string, string> FileExtensions = new Dictionary<string, string>
    {
        {FileMimeTypeNames.Image.Gif, "gif"},
        {FileMimeTypeNames.Image.Jpeg, "jpg"},
        {FileMimeTypeNames.Image.Pjpeg, "jpg"},
        {FileMimeTypeNames.Image.Png, "png"},
        {FileMimeTypeNames.Image.Svg, "svg"},
        {FileMimeTypeNames.Image.Tiff, "tif"},

        {FileMimeTypeNames.Application.Pdf, "pdf"},
        {FileMimeTypeNames.Application.Zip, "zip"},
        {FileMimeTypeNames.Application.Gzip, "gzip"},

        {FileMimeTypeNames.Text.Plain, "txt"},
        {FileMimeTypeNames.Text.Html, "htm"}
    };

    /// <inheritdoc />
    public Task<(string name, string extension)> Generate(CreateUpload createUpload)
    {
        var extension = FileExtensions.TryGetValue(createUpload.ContentType, out var matchingExtension)
            ? $".{matchingExtension}"
            : Path.GetExtension(createUpload.FileName);

        var originalNameHash = Convert.ToBase64String(Encoding.UTF8.GetBytes(createUpload.FileName!));
        var saltHash = Convert.ToBase64String(guidFactory.Create().ToByteArray());
        var fileName = Regex.Replace(originalNameHash + saltHash, @"\W", string.Empty);

        return Task.FromResult((fileName, extension));
    }
}