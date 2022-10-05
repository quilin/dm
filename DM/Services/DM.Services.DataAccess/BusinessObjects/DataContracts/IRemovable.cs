namespace DM.Services.DataAccess.BusinessObjects.DataContracts;

/// <summary>
/// Removable entity contract
/// </summary>
internal interface IRemovable
{
    /// <summary>
    /// Removed flag
    /// </summary>
    bool IsRemoved { get; set; }
}