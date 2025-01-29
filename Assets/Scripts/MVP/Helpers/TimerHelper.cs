using System.Text;

namespace MVP.Helpers
{
    public static class TimerHelper
    {
        public static string FormatTime(StringBuilder timeBuilder, int totalSeconds)
        {
            int minutes = totalSeconds / 60;
            int seconds = totalSeconds % 60;

            timeBuilder.Clear();
            timeBuilder.Append(minutes).Append(":").Append(seconds.ToString("00"));

            return timeBuilder.ToString();
        }
    }
}