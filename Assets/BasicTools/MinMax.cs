namespace BasicTools
{
    public class MinMax
    {
        public float Max { get; private set; }
        public float Min { get; private set; }

        public MinMax()
        {
            this.Max = float.MinValue;
            this.Min = float.MaxValue;
        }

        public void AddValue(float value)
        {
            if (value > Max)
            {
                Max = value;
            }
            if (value < Min)
            {
                Min = value;
            }
        }
    }

}
