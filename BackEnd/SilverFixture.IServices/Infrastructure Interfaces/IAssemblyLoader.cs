using System.Collections.Generic;

namespace SilverFixture.IServices.Infrastructure_Interfaces
{
    public interface IAssemblyLoader
    {
        string AssembliesPath { get; set; } 
        IEnumerable<TInterface> GetImplementations<TInterface>();
    }
}