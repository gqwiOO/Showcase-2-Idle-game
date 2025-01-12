namespace Core.Mechanics.Timer
{
    public interface ITimer
    {
        public float StartTime { get; }
        public float EndTime { get; }
        public float CurrentTime { get; }
    }
}