namespace StudentApi.Entities
{
    public class MasterScheduler
    {
        public int Idx { get; set; }
        public string JobName { get; set; } = string.Empty;
        public DateTime DateTimeRunJob { get; set; }
        public int IntervalJob { get; set; }
    }
}
