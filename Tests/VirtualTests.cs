using System;
using System.Diagnostics;
using NUnit.Framework;
// ReSharper disable NotAccessedField.Local

namespace VirtualVsSealedTests
{
    [TestFixture]
    public class VirtualTests
    {

        public class ClassWithNonEmptyMethods
        {
            double x;
            double y;

            public virtual void VirtualMethod()
            {
                x++;
            }

            public void SealedMethod()
            {
                y++;
            }
        }

        const int iterations = 1000000000;

// ReSharper disable once UnusedMember.Local
        static void Main()
        {
            var virtualTests = new VirtualTests();
            virtualTests.NonEmptyMethodTest();
        }

        [Test]
        [Explicit]
        public void NonEmptyMethodTest()
        {

            var foo = new ClassWithNonEmptyMethods();
            //Pre-call
            foo.VirtualMethod();
            foo.SealedMethod();

            var virtualWatch = new Stopwatch();
            virtualWatch.Start();
            for (var i = 0; i < iterations; i++)
            {
                foo.VirtualMethod();
            }
            virtualWatch.Stop();
            Console.WriteLine("virtual total {0}ms", virtualWatch.ElapsedMilliseconds);
            Console.WriteLine("per call virtual {0}ns", (float) virtualWatch.ElapsedMilliseconds * 1000000 / iterations);


            var sealedWatch = new Stopwatch();
            sealedWatch.Start();
            for (var i = 0; i < iterations; i++)
            {
                foo.SealedMethod();
            }
            sealedWatch.Stop();
            Console.WriteLine("sealed total {0}ms", sealedWatch.ElapsedMilliseconds);
            Console.WriteLine("per call sealed {0}ns", (float) sealedWatch.ElapsedMilliseconds * 1000000 / iterations);
        }

    }
}