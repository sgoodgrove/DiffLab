using System.Collections.Generic;

using DiffLab;

namespace DiffLabTests
{
    public static class PersonDiff
    {
        public static IEnumerable<IDiff<Person>> Diff(this IEnumerable<Person> people, IEnumerable<Person> others)
        {
            return Differ.Differences(
                people,
                others,
                (x) => (x),
                (x) => (x.FirstName, x.LastName),
                (x, y) => (x.FirstName == y.FirstName && x.LastName == y.LastName && x.Age == y.Age && x.Dependents.Diff(y.Dependents).AllSame()));
        }
    }

    public class Person
    {
        public int Age { get; set; }

        public List<Person> Dependents { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}