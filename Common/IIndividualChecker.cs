using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// Provides method for checking whether name is of a person or non-person, e.g. organization
    /// </summary>
    public interface IIndividualChecker
    {
        bool IsIndividual(string[] words);
    }
}
