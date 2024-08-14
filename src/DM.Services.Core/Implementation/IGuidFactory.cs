using System;

namespace DM.Services.Core.Implementation;

/// <summary>
/// Generates GUIDs
/// </summary>
public interface IGuidFactory
{
    /// <summary>
    /// Creates GUID
    /// </summary>
    /// <returns>New GUID</returns>
    Guid Create();
}