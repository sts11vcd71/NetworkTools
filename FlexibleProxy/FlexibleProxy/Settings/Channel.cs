namespace FlexibleProxy.Settings
{
    public class Channel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string DestinationHost { get; set; }

        public int DestinationPort { get; set; }

        public int SourcePort { get; set; }

        public bool Active { get; set; }
    }
}
