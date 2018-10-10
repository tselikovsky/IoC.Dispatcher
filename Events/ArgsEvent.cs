namespace StoryCLM.Events
{
    public class ArgsEvent<TArgs>: IEvent 
    {
        public ArgsEvent(object message)
        {
            Message = (TArgs)message;
        }
        public TArgs Message { get; set; }
    }
}
