namespace StoryCLM.Events.CUD
{
    /// Событие о добавлении с сообщением типа T
    public class InsertEvent<TArgs> : ArgsEvent<TArgs>
    {
        public InsertEvent(object args) : base(args) { }
    }
}
