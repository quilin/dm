namespace DM.Services.Core.Dto.Enums
{
    /// <summary>
    /// Game commentaries access mode
    /// </summary>
    public enum CommentariesAccessMode
    {
        /// <summary>
        /// Everyone may read and write commentaries in the game
        /// </summary>
        Public = 0,

        /// <summary>
        /// Everyone may read, but only game players may write commentaries
        /// </summary>
        Readonly = 1,

        /// <summary>
        /// Only game players may read or write commentaries
        /// </summary>
        Private = 2
    }
}