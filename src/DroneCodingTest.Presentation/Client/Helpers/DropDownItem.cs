namespace DroneCodingTest.Client.Helpers
{
    public class DropDownItem
    {
        public string Text { get; set; }
        public int Value { get; set; }
        public string Description { get; set; }

        public DropDownItem(string text, int value, string description)
        {
            Text = text;
            Value = value;
            Description = description;
        }
    }
}
