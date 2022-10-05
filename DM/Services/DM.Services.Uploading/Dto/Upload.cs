using System;
using DM.Services.Core.Dto;

namespace DM.Services.Uploading.Dto;

/// <summary>
/// DTO model for user upload
/// </summary>
public class Upload
{
    /// <summary>
    /// Upload identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Creation moment
    /// </summary>
    public DateTimeOffset CreateDate { get; set; }

    /// <summary>
    /// File owner
    /// </summary>
    public GeneralUser Owner { get; set; }

    /// <summary>
    /// File name to display
    /// </summary>
    public string FileName { get; set; }

    /// <summary>
    /// Path to file in storage
    /// </summary>
    public string FilePath { get; set; }
}