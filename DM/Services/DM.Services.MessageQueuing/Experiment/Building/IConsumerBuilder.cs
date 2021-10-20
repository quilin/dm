namespace DM.Services.MessageQueuing.Experiment.Building
{
    /// <summary>
    /// Построитель консюмера
    /// </summary>
    /// <typeparam name="TMessage">Тип входящего сообщения</typeparam>
    public interface IConsumerBuilder<out TMessage>
    {
        IConsumerBuilder<TMessage> WithDecoder<TDecoder>();
    }
}