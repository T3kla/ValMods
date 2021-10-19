namespace Areas.Containers
{
    public struct Area
    {
        public string name { get; set; }
        public string cfg { get; set; }
        public int? layer { get; set; }
        public bool? passthrough { get; set; }
        public float[] centre { get; set; }
        public float[] radius { get; set; }
    }
}