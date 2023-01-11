namespace CronoTrain.Models
{
    public class HangTime
    {
        public int Id { get; set; }
        public int SessionId { get; set; }
        public TimeSpan Duration { get; set; }

        public static HangTime Zero => new HangTime(TimeSpan.Zero);

        public HangTime(TimeSpan duration)
        {
            Duration = duration;
        }

        public override string ToString()
        {
            return Duration.ToString(@"mm\:ss\.ff");
        }
    }
}
