namespace ToDoAPI.Models
{
    public class WeatherResponse
    {
        public WeatherCurrent Current { get; set; }
    }

    public class WeatherCurrent
    {
        public double Temp_C { get; set; }
        public WeatherCondition Condition { get; set; }
    }

    public class WeatherCondition
    {
        public string Text { get; set; }
    }

}
