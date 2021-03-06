namespace GovUk.Education.ExploreEducationStatistics.Data.Model
{
    public class LocalEnterprisePartnership : IObservationalUnit
    {
        public string Code { get; set; }
        public string Name { get; set; }

        private LocalEnterprisePartnership()
        {
        }

        public LocalEnterprisePartnership(string code, string name)
        {
            Code = code;
            Name = name;
        }

        public static LocalEnterprisePartnership Empty()
        {
            return new LocalEnterprisePartnership(null, null);
        }

        protected bool Equals(LocalEnterprisePartnership other)
        {
            return string.Equals(Code, other.Code);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((LocalEnterprisePartnership) obj);
        }

        public override int GetHashCode()
        {
            return (Code != null ? Code.GetHashCode() : 0);
        }
    }
}