using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace EirinDuran.AssemblyLoader
{
    public class AssemblyLoader
    {
        private readonly string assembliesPath;

        public AssemblyLoader()
        {
            assembliesPath = Directory.GetCurrentDirectory();
        }

        public AssemblyLoader(string assembliesPath)
        {
            this.assembliesPath = assembliesPath;
        }

        public IEnumerable<TInterface> GetImplementations<TInterface>()
        {
            List<Assembly> assemblies = GetAssembliesInDirectory();
            return GetImplementationsFromAssemblies<TInterface>(assemblies);
        }

        private IEnumerable<TInterface> GetImplementationsFromAssemblies<TInterface>(List<Assembly> assemblies)
        {
            List<TInterface> implementations = new List<TInterface>();
            Func<Type, bool> typeIsImplementation = t => typeof(TInterface).IsAssignableFrom(t) && !t.IsInterface;
            
            foreach (Assembly assembly in assemblies)
            {
                IEnumerable<Type> implementationsType = assembly.GetTypes().Where(typeIsImplementation);
                IEnumerable<TInterface> assemblyImplementations = implementationsType.Select(t => Activator.CreateInstance(t)).Cast<TInterface>();
                
               implementations.AddRange(assemblyImplementations);
            }

            return implementations;
        }

        private List<Assembly> GetAssembliesInDirectory()
        {
            List<Assembly> assemblies = new List<Assembly>();
            IEnumerable<string> files = Directory.GetFiles(assembliesPath);
            foreach (string file in files)
            {
                AddAssemblyIfExists(file, assemblies);
            }

            return assemblies;
        }

        private void AddAssemblyIfExists(string file, List<Assembly> assemblies)
        {
            try
            {
                Assembly assembly = Assembly.LoadFile(file);
                assemblies.Add((assembly));
            }
            catch (System.BadImageFormatException)
            {
                
            }
        }
    }
}