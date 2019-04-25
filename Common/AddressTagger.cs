using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// Exposes method for tagging words in an address with address-part tags based on provided reference arrays.
    /// This converts array of words into array of <see cref="int"/> that can be passed to HMM.
    /// For example, 'Blvd.' might be tagged as <see cref="AddressTag.StreetType"/>.
    /// Implements <see cref="ITagger"/>
    /// </summary>
    public class AddressTagger : ITagger
    {
        private string[] StreetTypes;
        private string[] UnitTypes;
        private string[] Boxes;
        private string[] States;

        /// <summary>
        /// Creates instance of <see cref="AddressTagger"/>
        /// </summary>
        public AddressTagger() : this(new ReferenceLoader())
        {
        }

        internal AddressTagger(IReferenceLoader referenceLoader)
        {
            StreetTypes = referenceLoader.GetStreetTypes();
            UnitTypes = referenceLoader.GetUnitTypes();
            Boxes = referenceLoader.GetBoxes();
            States = referenceLoader.GetUSStates();
        }

        /// <summary>
        /// Creates array of <see cref="int"/> representing underlying value of <see cref="AddressTag"/>
        /// corresponding to words in input array.  For convenience so that returned value can be passed
        /// directly to HMM.
        /// Input should already be split, trimmed, cased, and preprocessed.
        /// </summary>
        /// <param name="input">Array of words in address. Should already be formatted correctly.</param>
        /// <returns><see cref="int"/> array</returns>
        public int[] TagInput(string[] input)
        {
            return input.Select(word => (int)TagWord(word)).ToArray();
        }

        private AddressTag TagWord(string word)
        {
            if (IsDigitsOnly(word))
            {
                return AddressTag.Number;
            }

            if (Boxes.Contains(word))
            {
                return AddressTag.Box;
            }

            if (States.Contains(word))
            {
                return AddressTag.State;
            }

            if (UnitTypes.Contains(word))
            {
                return AddressTag.UnitType;
            }

            if (StreetTypes.Contains(word))
            {
                return AddressTag.StreetType;
            }

            return AddressTag.Unknown;
        }

        // cf. https://stackoverflow.com/a/7461095/8881332
        private bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }
    }
}
