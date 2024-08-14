using System;

namespace DM.Services.MessageQueuing.GeneralBus;

/// <inheritdoc />
public class InvokedEventException : Exception
{
    /// <inheritdoc />
    public InvokedEventException(string message) : base(message)
    {
    }
}