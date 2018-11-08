using System.Collections.Generic;

namespace EirinDuran.IServices.Infrastructure_Interfaces
{
    public interface IAssemblyLoader
    {
        string AssembliesPath { get; set; } 
        IEnumerable<TInterface> GetImplementations<TInterface>();
    }
}