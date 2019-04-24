using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HmmNameParser.Validation
{
    internal class ValidationData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            List<object[]> data = new List<object[]>()
            {
                new object[]
                {
                    "John Smith",
                    new Name()
                    {
                        FirstName = "John",
                        LastName = "Smith"
                    }
                },
                new object[]
                {
                    "Mr. John Smith",
                    new Name()
                    {
                        Prefix = "Mr.",
                        FirstName = "John",
                        LastName = "Smith"
                    }
                },
                new object[]
                {
                    "Mr. John Smith, Jr.",
                    new Name()
                    {
                        Prefix = "Mr.",
                        FirstName = "John",
                        LastName = "Smith",
                        Suffix = "Jr."
                    }
                },
                new object[]
                {
                    "John Smith, Jr.",
                    new Name()
                    {
                        FirstName = "John",
                        LastName = "Smith",
                        Suffix = "Jr."
                    }
                },
                new object[]
                {
                    "Lt. Col. John Smith, Jr. (Ret.)",
                    new Name()
                    {
                        Prefix = "Lt. Col.",
                        FirstName = "John",
                        LastName = "Smith",
                        Suffix = "Jr. (Ret.)"
                    }
                },
                new object[]
                {
                    "John D. Smith, Jr.",
                    new Name()
                    {
                        FirstName = "John",
                        MiddleName = "D.",
                        LastName = "Smith",
                        Suffix = "Jr."
                    }
                },
                new object[]
                {
                    "Rev. John D. Smith, Jr.",
                    new Name()
                    {
                        Prefix = "Rev.",
                        FirstName = "John",
                        MiddleName = "D.",
                        LastName = "Smith",
                        Suffix = "Jr."
                    }
                },
                new object[]
                {
                    "Karen Killinger-Humes",
                    new Name()
                    {
                        FirstName = "Karen",
                        LastName = "Killinger-Humes"
                    }
                },
                new object[]
                {
                    "C. S. Lewis",
                    new Name()
                    {
                        FirstName = "C.",
                        MiddleName = "S.",
                        LastName = "Lewis"
                    }
                }
            };

            for (int i = 0; i < data.Count(); i++)
            {
                yield return data[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
