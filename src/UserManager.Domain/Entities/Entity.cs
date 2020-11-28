using System.Collections.Generic;

namespace UserManager.Domain.Entities
{
    public abstract class Entity
    {
        public long Id { get; set; }
        
        internal List<string> _errors;
        public List<string> Errors => _errors;

        public abstract bool Validate();
    }
}
