using System;

namespace EirinDuran.Domain
{
    public class DomainException : Exception
    {
             
        public DomainException(object data, string reason, Exception exception = null) 
            : base($"{data} is invalid because {reason}.")
        {
            
        }

        
        public DomainException(object data, Exception exception = null) 
            : base($"{data} is invalid.")
        {
            
        }
    }
}
