namespace StoryCLM.Events.CUD
{
    /// <summary>
    /// Событие об удалении с сообщением типа T
    /// </summary>
    public class DeleteEvent<TArgs> : ArgsEvent<TArgs> 
    {
        public DeleteEvent(object args) : base(args) { }
    }
}
