using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HmmParser.Validation
{
    internal class NameValidationData : IEnumerable<object[]>
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
                    "John Smith IV",
                    new Name()
                    {
                        FirstName = "John",
                        LastName = "Smith",
                        Suffix = "IV"
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
                    "John D. Smith Sr.",
                    new Name()
                    {
                        FirstName = "John",
                        MiddleName = "D.",
                        LastName = "Smith",
                        Suffix = "Sr."
                    }
                },
                new object[]
                {
                    "Rev. John D. Smith III",
                    new Name()
                    {
                        Prefix = "Rev.",
                        FirstName = "John",
                        MiddleName = "D.",
                        LastName = "Smith",
                        Suffix = "III"
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
                },
                new object[]
                {
                    "Senator John McCain",
                    new Name()
                    {
                        Prefix = "Senator",
                        FirstName = "John",
                        LastName = "McCain"
                    }
                },
                new object[]
                {
                    "Friends of John McCain",
                    new Name()
                    {
                        LastName = "Friends Of John McCain"
                    }
                },
                new object[]
                {
                    "National Association Of Broadcasters PAC",
                    new Name()
                    {
                        LastName = "National Association Of Broadcasters Pac"
                    }
                },
                new object[]
                {
                    "A Corporation Inc.",
                    new Name()
                    {
                        LastName = "A Corporation Inc."
                    }
                },
                new object[]
                {
                    "Dr. Robert Van de Graaff",
                    new Name()
                    {
                        Prefix = "Dr.",
                        FirstName = "Robert",
                        LastName = "Van De Graaff"
                    }
                },
                new object[]
                {
                    "Oscar De La Hoya",
                    new Name()
                    {
                        FirstName = "Oscar",
                        LastName = "De La Hoya"
                    }
                }
            };

            for (int i = 0; i < data.Count; i++)
            {
                yield return data[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
