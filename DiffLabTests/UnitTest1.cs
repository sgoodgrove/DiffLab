using System.Collections.Generic;
using System.Linq;

using DiffLab;
using NUnit.Framework;

namespace DiffLabTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var a = new[] { 1, 2, 3, 4, 5 };
            var b = new[] { 1, 2, 3, 5 };
            var results = DiffLab.Differ.Differences(a, b, (x) => (x), (x) => x, (x, y) => x == y).ToList();
            Assert.Pass();
        }

        [Test]
        public void Test2()
        {
            var a = new[]
            {
                new Person { FirstName = "Jay", LastName = "Jones", Age = 50, Dependents = new List<Person> { new Person { FirstName = "Bobby", LastName = "Tables", Age = 10 } } },
                new Person { FirstName = "Ken", LastName = "Watts", Age = 40 },
                new Person { FirstName = "Beck", LastName = "Place", Age = 30 },
                new Person { FirstName = "Juan", LastName = "Fernandez", Age = 99 }
            };
            var b = new[]
            {
                new Person { FirstName = "Jay", LastName = "Jones", Age = 50 },
                new Person { FirstName = "Ken", LastName = "Wats", Age = 40 },
                new Person { FirstName = "Beck", LastName = "Place", Age = 20 },
                new Person { FirstName = "Juan", LastName = "Fernandez", Age = 99 }
            };

            var diff = a.Diff(b);

            Assert.That(diff.Where(x => x.DiffType == DiffType.Same), Has.Exactly(1).Items);
            Assert.That(diff.Where(x => x.DiffType == DiffType.Different), Has.Exactly(2).Items);
        }

        [Test]
        public void Test3()
        {
            var a = new[]
            {
                new Person { FirstName = "Jay", LastName = "Jones", Age = 50, Dependents = new List<Person> { new Person { FirstName = "Bobby", LastName = "Tables", Age = 10 } } },
                new Person { FirstName = "Ken", LastName = "Watts", Age = 40 },
                new Person { FirstName = "Beck", LastName = "Place", Age = 30 },
                new Person { FirstName = "Juan", LastName = "Fernandez", Age = 99 }
            };
            var b = new[]
            {
                new Person { FirstName = "Jay", LastName = "Jones", Age = 50 },
                new Person { FirstName = "Ken", LastName = "Wats", Age = 40 },
                new Person { FirstName = "Beck", LastName = "Place", Age = 20 },
                new Person { FirstName = "Juan", LastName = "Fernandez", Age = 99 }
            };

            var m = new MemoizedDiffer<IEnumerable<Person>>(a, b);
            var diff = m.Differences(
                (x) => (x),
                (x) => (x.FirstName, x.LastName),
                (x, y) => (x.FirstName == y.FirstName && x.LastName == y.LastName && x.Age == y.Age && x.Dependents.Diff(y.Dependents).AllSame()));
            var diff2 = m.Differences(
                (x) => (x),
                (x) => (x.FirstName, x.LastName),
                (x, y) => (x.FirstName == y.FirstName && x.LastName == y.LastName && x.Age == y.Age && x.Dependents.Diff(y.Dependents).AllSame()));

            Assert.That(diff.Where(x => x.DiffType == DiffType.Same), Has.Exactly(1).Items);
            Assert.That(diff.Where(x => x.DiffType == DiffType.Different), Has.Exactly(2).Items);
        }
    }
}