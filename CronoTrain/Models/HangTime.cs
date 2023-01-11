namespace CronoTrain.Models
{
    public class HangTime
    {
        public TimeSpan Duration { get; set; }

        //TODO: add session id (create Session model with id and (end) time stamp)

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
