using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoslynTester.DiagnosticResults;
using RoslynTester.Helpers.CSharp;
using VSDiagnostics.Diagnostics.General.PrivateSetAutoPropertyCanBeReadOnlyAutoProperty;

namespace VSDiagnostics.Test.Tests.General
{
    [TestClass]
    public class PrivateSetAutoPropertyCanUseReadOnlyAutoPropertyTests : CSharpCodeFixVerifier
    {
        protected override CodeFixProvider CodeFixProvider => null;
        protected override DiagnosticAnalyzer DiagnosticAnalyzer => new PrivateSetAutoPropertyCanBeReadOnlyAutoPropertyAnalyzer();

        [TestMethod]
        public void PrivateSetAutoPropertyCanUseReadOnlyAutoPropertyAnalyzer_WithConstructorSetAccess_InvokesWarning()
        {
            var original = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        int SomeProperty { get; private set; }
   
        MyClass()
        {
            this.SomeProperty = 42;
        }
    }
}";

            var expected = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        int SomeProperty { get; }
   
        MyClass()
        {
            this.SomeProperty = 42;
        }
    }
}";


            var expectedDiagnostic = new DiagnosticResult
            {
                Id = PrivateSetAutoPropertyCanBeReadOnlyAutoPropertyAnalyzer.DiagnosticId,
                Message = PrivateSetAutoPropertyCanBeReadOnlyAutoPropertyAnalyzer.Message,
                Severity = PrivateSetAutoPropertyCanBeReadOnlyAutoPropertyAnalyzer.Severity,
                Locations =
                    new[]
                    {
                        new DiagnosticResultLocation("Test0.cs", 9, 33)
                    }
            };


            VerifyDiagnostic(original, expectedDiagnostic);
            //VerifyFix(original, expected);
        }

        [TestMethod]
        public void PrivateSetAutoPropertyCanUseReadOnlyAutoPropertyAnalyzer_WithMethodGetAccess_InvokesWarning()
        {
            var original = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        int SomeProperty { get; private set; }

        void SomeMethod()
        {
            Console.WriteLine(SomeProperty);
        }
    }
}";

            var expected = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        int SomeProperty { get; }

        void SomeMethod()
        {
            Console.WriteLine(SomeProperty);
        }
    }
}";

            var expectedDiagnostic = new DiagnosticResult
            {
                Id = PrivateSetAutoPropertyCanBeReadOnlyAutoPropertyAnalyzer.DiagnosticId,
                Message = PrivateSetAutoPropertyCanBeReadOnlyAutoPropertyAnalyzer.Message,
                Severity = PrivateSetAutoPropertyCanBeReadOnlyAutoPropertyAnalyzer.Severity,
                Locations =
                    new[]
                    {
                        new DiagnosticResultLocation("Test0.cs", 9, 33)
                    }
            };


            VerifyDiagnostic(original, expectedDiagnostic);
            //VerifyFix(original, expected);
        }

        [TestMethod]
        public void PrivateSetAutoPropertyCanUseReadOnlyAutoPropertyAnalyzer_WithMethodSetAccess_DoesNotInvokesWarning()
        {
            var original = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        int SomeProperty { et; private set; }

        void SomeMethod()
        {
            this.SomeProperty = 10;
        }
    }
}";

            VerifyDiagnostic(original);
        }

        [TestMethod]
        public void PrivateSetAutoPropertyCanUseReadOnlyAutoPropertyAnalyzer_WithoutPrivateSetter_DoesNotInvokesWarning()
        {
            var original = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        int SomeOtherProperty { get; set; };
   
        MyClass()
        {
            this.SomeOtherProperty = 27;
        }

        void SomeMethod()
        {
            Console.WriteLine(this.SomeOtherProperty);
        }
    }
}";

            VerifyDiagnostic(original);
        }

        [TestMethod]
        public void PrivateSetAutoPropertyCanUseReadOnlyAutoPropertyAnalyzer_WithNestedClass_WithoutStaticProperty_InvokesWarning()
        {
            var original = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    public class MyClass
    {
        public int SomeProperty { get; private set; }

        MyClass()
        {
            this.SomeProperty = 42;
        }

        private class NestedClass
        {
            internal int AnotherProperty { get; set; }

            void Foo()
            {
                AnotherProperty = 10;
            }
        }
    }
}";

            var expected = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    public class MyClass
    {
        public int SomeProperty { get; }

        MyClass()
        {
            this.SomeProperty = 42;
        }

        private class NestedClass
        {
            internal int AnotherProperty { get; set; }

            void Foo()
            {
                AnotherProperty = 10;
            }
        }
    }
}";

            var expectedDiagnostic = new DiagnosticResult
            {
                Id = PrivateSetAutoPropertyCanBeReadOnlyAutoPropertyAnalyzer.DiagnosticId,
                Message = PrivateSetAutoPropertyCanBeReadOnlyAutoPropertyAnalyzer.Message,
                Severity = PrivateSetAutoPropertyCanBeReadOnlyAutoPropertyAnalyzer.Severity,
                Locations = new[]
                {
                    new DiagnosticResultLocation("Test0.cs", 9, 40)
                }
            };


            VerifyDiagnostic(original, expectedDiagnostic);
            //VerifyFix(original, expected);
        }

        [TestMethod]
        public void PrivateSetAutoPropertyCanUseReadOnlyAutoPropertyAnalyzer_WithNestedClass_InvokesWarning()
        {
            var original = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    public class MyClass
    {
        private class NestedClass
        {
            internal int SomeProperty { get; private set; }

            NestedClass()
            {
                this.SomeProperty = 42;
            }
        }
    }
}";

            var expected = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    public class MyClass
    {
        private class NestedClass
        {
            internal int SomeProperty { get; }

            NestedClass()
            {
                this.SomeProperty = 42;
            }
        }
    }
}";

            var expectedDiagnostic = new DiagnosticResult
            {
                Id = PrivateSetAutoPropertyCanBeReadOnlyAutoPropertyAnalyzer.DiagnosticId,
                Message = PrivateSetAutoPropertyCanBeReadOnlyAutoPropertyAnalyzer.Message,
                Severity = PrivateSetAutoPropertyCanBeReadOnlyAutoPropertyAnalyzer.Severity,
                Locations = new[]
                {
                    new DiagnosticResultLocation("Test0.cs", 11, 46)
                }
            };


            VerifyDiagnostic(original, expectedDiagnostic);
            //VerifyFix(original, expected);
        }

        [TestMethod]
        public void PrivateSetAutoPropertyCanUseReadOnlyAutoPropertyAnalyzer_PropertyAssignmentWithoutThisKeyword_InvokesWarning()
        {
            var original = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        int SomeProperty { get; private set; }
   
        MyClass()
        {
            SomeProperty = 42;
        }
    }
}";

            var expected = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        int SomeProperty {get; }
   
        MyClass()
        {
            SomeProperty = 42;
        }
    }
}";
            var expectedDiagnostic = new DiagnosticResult
            {
                Id = PrivateSetAutoPropertyCanBeReadOnlyAutoPropertyAnalyzer.DiagnosticId,
                Message = PrivateSetAutoPropertyCanBeReadOnlyAutoPropertyAnalyzer.Message,
                Severity = PrivateSetAutoPropertyCanBeReadOnlyAutoPropertyAnalyzer.Severity,
                Locations =
                    new[]
                    {
                        new DiagnosticResultLocation("Test0.cs", 9, 32)
                    }
            };


            VerifyDiagnostic(original, expectedDiagnostic);
            //VerifyFix(original, expected);
        }

        [TestMethod]
        public void PrivateSetAutoPropertyCanUseReadOnlyAutoPropertyAnalyzer_WithStaticProperty_WithInstanceUsage_DoesNotInvokeWarning()
        {
            var original = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    public class MyClass
    {
        public static int SomeProperty { get; private set; }

        MyClass()
        {
            SomeProperty = 42;
        }
    }
}";
            VerifyDiagnostic(original);
        }

        [TestMethod]
        public void PrivateSetAutoPropertyCanUseReadOnlyAutoPropertyAnalyzer_WithStaticProperty_WithOnlyStaticUsage_InvokesWarning()
        {
            var original = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        static int SomeProperty { get; private set; }

        static MyClass()
        {
            SomeProperty = 42;
        }
    }
}";

            var expected = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        static int SomeProperty { get; }

        static MyClass()
        {
            SomeProperty = 42;
        }
    }
}";
            var expectedDiagnostic = new DiagnosticResult
            {
                Id = PrivateSetAutoPropertyCanBeReadOnlyAutoPropertyAnalyzer.DiagnosticId,
                Message = PrivateSetAutoPropertyCanBeReadOnlyAutoPropertyAnalyzer.Message,
                Severity = PrivateSetAutoPropertyCanBeReadOnlyAutoPropertyAnalyzer.Severity,
                Locations =
                    new[]
                    {
                        new DiagnosticResultLocation("Test0.cs", 9, 32)
                    }
            };


            VerifyDiagnostic(original, expectedDiagnostic);
            //VerifyFix(original, expected);
        }

        [TestMethod]
        public void PrivateSetAutoPropertyCanUseReadOnlyAutoPropertyAnalyzer_WithStaticProperty_WithMixedStaticAndInstanceUsage_DoesNotInvokeWarning()
        {
            var original = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        static int SomeProperty { get; private set; }

        static MyClass()
        {
            SomeProperty = 42;
        }

        MyClass()
        {
            SomeProperty = 43;
        }
    }
}";
            VerifyDiagnostic(original);
        }
    }
}