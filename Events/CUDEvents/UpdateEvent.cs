namespace StoryCLM.Events.CUD
{
    public class UpdateEvent<TArgs> : ArgsEvent<TArgs> 
    {
        public UpdateEvent(object args) : base(args) { }
    }
}
