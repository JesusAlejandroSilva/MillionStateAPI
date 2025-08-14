namespace Million.Infrastructure.Common
{
    /// <summary>Reloj del sistema inyectable (Singleton) para testear tiempos</summary>
    public interface ISystemClock
    {
        DateTime UtcNow { get; }
    }

    public sealed class SystemClock : ISystemClock
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
