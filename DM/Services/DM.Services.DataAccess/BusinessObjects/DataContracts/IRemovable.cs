namespace DM.Services.DataAccess.BusinessObjects.DataContracts
{
    /// <summary>
    /// Removable entity contract
    /// </summary>
    public interface IRemovable
    {
        /// <summary>
        /// Removed flag
        /// </summary>
        bool IsRemoved { get; set; }
    }
}