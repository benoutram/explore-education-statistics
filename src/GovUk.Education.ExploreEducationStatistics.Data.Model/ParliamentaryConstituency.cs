namespace GovUk.Education.ExploreEducationStatistics.Data.Model
{
    public class ParliamentaryConstituency : IObservationalUnit
    {
        public string Code { get; set; }
        public string Name { get; set; }

        private ParliamentaryConstituency()
        {
        }

        public ParliamentaryConstituency(string code, string name)
        {
            Code = code;
            Name = name;
        }

        public static ParliamentaryConstituency Empty()
        {
            return new ParliamentaryConstituency(null, null);
        }

        protected bool Equals(ParliamentaryConstituency other)
        {
            return string.Equals(Code, other.Code);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ParliamentaryConstituency) obj);
        }

        public override int GetHashCode()
        {
            return (Code != null ? Code.GetHashCode() : 0);
        }
    }
}