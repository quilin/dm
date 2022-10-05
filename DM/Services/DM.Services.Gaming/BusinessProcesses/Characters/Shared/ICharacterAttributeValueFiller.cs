using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Gaming.Dto.Output;

namespace DM.Services.Gaming.BusinessProcesses.Characters.Shared;

/// <summary>
/// Filler for character attribute values
/// </summary>
internal interface ICharacterAttributeValueFiller
{
    /// <summary>
    /// Fill character attribute values with needed metadata from game attribute schema
    /// </summary>
    /// <param name="characters"></param>
    /// <param name="schemaId"></param>
    /// <returns></returns>
    Task Fill(IEnumerable<Character> characters, Guid? schemaId);
}