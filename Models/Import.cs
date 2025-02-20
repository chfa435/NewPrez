namespace NewTiceAI.Models
{
    public class Import
    {

        private DateTime _dateImported;
        private DateTime? _dateReverted;

        public int Id { get; set; }
        public DateTime ImportDate {
            get
            {
                return _dateImported;
            }

            set
            {
                _dateImported = value.ToUniversalTime();
            }

        }
        public DateTime? RevertDate {
            get => _dateReverted;
            set
            {
                if (value.HasValue)
                {
                    _dateReverted = value.Value.ToUniversalTime();
                }
                else
                {
                    _dateReverted = null;
                }
            }
        }
        public string? ImportComment { get; set; }

        public string? RevertReason { get; set; }

        //Nav Properties
        public virtual ICollection<Contact> Contacts { get; set; } = new HashSet<Contact>();
    }
}
