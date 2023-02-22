namespace Main.Extensions
{
    public class Parameter
    {
        public string key { get; set; }
        public string name { get; set; }
        public string value { get; set; }
        public Parameter(string Key,string Value,string Name)
        {
            this.key = Key;
            this.value = Value;
            this.name = Name;
        }
        public static Parameter Create(string Key,string Value,string Name)
        {
            return new Parameter(Key,Value,Name);
        }
    }
}
