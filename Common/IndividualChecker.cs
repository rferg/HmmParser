using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// Implementation of <see cref="IIndividualChecker"/>
    /// </summary>
    public class IndividualChecker : IIndividualChecker
    {
        private string[] OrgIndicators;

        /// <summary>
        /// Initializes an instance of <see cref="Tagger"/>
        /// </summary>
        public IndividualChecker() : this(new ReferenceLoader())
        {
        }

        internal IndividualChecker(IReferenceLoader referenceLoader)
        {
            OrgIndicators = referenceLoader.GetOrgIndicators();
        }

        /// <summary>
        /// Checks whether name is of a person or of, say, an organization.
        /// </summary>
        /// <param name="words">Formatted words of name. (split, trimmed, lowercased, etc.)</param>
        /// <returns>Whether name with these words is of an individual</returns>
        public bool IsIndividual(string[] words)
        {
            return !words.Intersect(OrgIndicators).Any();
        }
    }
}
