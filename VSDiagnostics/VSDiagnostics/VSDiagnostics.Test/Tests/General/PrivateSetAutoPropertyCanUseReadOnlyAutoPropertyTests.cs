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
        public int SomeProperty { get; private set; }
   
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
        public int SomeProperty { get; }
   
        MyClass()
        {
            SomeProperty = 42;
        }
    }
}";


            var expectedDiagnostic = new DiagnosticResult
            {
                Id = PrivateSetAutoPropertyCanBeReadOnlyAutoPropertyAnalyzer.DiagnosticId,
                Message = string.Format(string.Format(PrivateSetAutoPropertyCanBeReadOnlyAutoPropertyAnalyzer.Message, "SomeProperty"), "SomeProperty"),
                Severity = PrivateSetAutoPropertyCanBeReadOnlyAutoPropertyAnalyzer.Severity,
                Locations =
                    new[]
                    {
                        new DiagnosticResultLocation("Test0.cs", 9, 20)
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
        public int SomeProperty { get; private set; }

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
        public int SomeProperty { get; }

        void SomeMethod()
        {
            Console.WriteLine(SomeProperty);
        }
    }
}";

            var expectedDiagnostic = new DiagnosticResult
            {
                Id = PrivateSetAutoPropertyCanBeReadOnlyAutoPropertyAnalyzer.DiagnosticId,
                Message = string.Format(PrivateSetAutoPropertyCanBeReadOnlyAutoPropertyAnalyzer.Message, "SomeProperty"),
                Severity = PrivateSetAutoPropertyCanBeReadOnlyAutoPropertyAnalyzer.Severity,
                Locations =
                    new[]
                    {
                        new DiagnosticResultLocation("Test0.cs", 9, 20)
                    }
            };


            VerifyDiagnostic(original, expectedDiagnostic);
            //VerifyFix(original, expected);
        }

        [TestMethod]
        public void PrivateSetAutoPropertyCanUseReadOnlyAutoPropertyAnalyzer_WithMethodSetAccess_DoesNotInvokeWarning()
        {
            var original = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        public int SomeProperty { get; private set; }

        void SomeMethod()
        {
            SomeProperty = 10;
        }
    }
}";

            VerifyDiagnostic(original);
        }

        [TestMethod]
        public void PrivateSetAutoPropertyCanUseReadOnlyAutoPropertyAnalyzer_WithGetAndSetAccess_InvokesWarning()
        {
            var original = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        public int SomeProperty { get; private set; }

        MyClass()
        {
            SomeProperty = 10;
        }

        void Method()
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
        public int SomeProperty { get; }

        MyClass()
        {
            SomeProperty = 10;
        }

        void Method()
        {
            Console.WriteLine(SomeProperty);
        }
    }
}";

            var expectedDiagnostic = new DiagnosticResult
            {
                Id = PrivateSetAutoPropertyCanBeReadOnlyAutoPropertyAnalyzer.DiagnosticId,
                Message = string.Format(PrivateSetAutoPropertyCanBeReadOnlyAutoPropertyAnalyzer.Message, "SomeProperty"),
                Severity = PrivateSetAutoPropertyCanBeReadOnlyAutoPropertyAnalyzer.Severity,
                Locations =
                    new[]
                    {
                        new DiagnosticResultLocation("Test0.cs", 9, 20)
                    }
            };


            VerifyDiagnostic(original, expectedDiagnostic);
            //VerifyFix(original, expected);
        }

        [TestMethod]
        public void PrivateSetAutoPropertyCanUseReadOnlyAutoPropertyAnalyzer_WithoutPrivateSetter_DoesNotInvokeWarning()
        {
            var original = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        public int SomeOtherProperty { get; set; };
   
        MyClass()
        {
            SomeOtherProperty = 27;
        }

        void SomeMethod()
        {
            Console.WriteLine(SomeOtherProperty);
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
            SomeProperty = 42;
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
            SomeProperty = 42;
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
                Message = string.Format(PrivateSetAutoPropertyCanBeReadOnlyAutoPropertyAnalyzer.Message, "SomeProperty"),
                Severity = PrivateSetAutoPropertyCanBeReadOnlyAutoPropertyAnalyzer.Severity,
                Locations = new[]
                {
                    new DiagnosticResultLocation("Test0.cs", 9, 20)
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
                SomeProperty = 42;
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
                SomeProperty = 42;
            }
        }
    }
}";

            var expectedDiagnostic = new DiagnosticResult
            {
                Id = PrivateSetAutoPropertyCanBeReadOnlyAutoPropertyAnalyzer.DiagnosticId,
                Message = string.Format(PrivateSetAutoPropertyCanBeReadOnlyAutoPropertyAnalyzer.Message, "SomeProperty"),
                Severity = PrivateSetAutoPropertyCanBeReadOnlyAutoPropertyAnalyzer.Severity,
                Locations = new[]
                {
                    new DiagnosticResultLocation("Test0.cs", 11, 26)
                }
            };


            VerifyDiagnostic(original, expectedDiagnostic);
            //VerifyFix(original, expected);
        }

        [TestMethod]
        public void PrivateSetAutoPropertyCanUseReadOnlyAutoPropertyAnalyzer_AssignmentWithThisKeyword_InvokesWarning()
        {
            var original = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        public int SomeProperty { get; private set; }
   
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
        public int SomeProperty { get; }
   
        MyClass()
        {
            this.SomeProperty = 42;
        }
    }
}";
            var expectedDiagnostic = new DiagnosticResult
            {
                Id = PrivateSetAutoPropertyCanBeReadOnlyAutoPropertyAnalyzer.DiagnosticId,
                Message = string.Format(PrivateSetAutoPropertyCanBeReadOnlyAutoPropertyAnalyzer.Message, "SomeProperty"),
                Severity = PrivateSetAutoPropertyCanBeReadOnlyAutoPropertyAnalyzer.Severity,
                Locations =
                    new[]
                    {
                        new DiagnosticResultLocation("Test0.cs", 9, 20)
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
        public static int SomeProperty { get; private set; }

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
        public static int SomeProperty { get; }

        static MyClass()
        {
            SomeProperty = 42;
        }
    }
}";
            var expectedDiagnostic = new DiagnosticResult
            {
                Id = PrivateSetAutoPropertyCanBeReadOnlyAutoPropertyAnalyzer.DiagnosticId,
                Message = string.Format(PrivateSetAutoPropertyCanBeReadOnlyAutoPropertyAnalyzer.Message, "SomeProperty"),
                Severity = PrivateSetAutoPropertyCanBeReadOnlyAutoPropertyAnalyzer.Severity,
                Locations =
                    new[]
                    {
                        new DiagnosticResultLocation("Test0.cs", 9, 27)
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
        public static int SomeProperty { get; private set; }

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

        [TestMethod]
        public void PrivateSetAutoPropertyCanUseReadOnlyAutoPropertyAnalyzer_AssignmentWithMemberAccessExpression_InvokesWarning()
        {
            var original = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        public static int SomeProperty { get; private set; }
   
        static MyClass()
        {
            MyClass.SomeProperty = 42;
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
        public static int SomeProperty { get; }
   
        static MyClass()
        {
            MyClass.SomeProperty = 42;
        }
    }
}";
            var expectedDiagnostic = new DiagnosticResult
            {
                Id = PrivateSetAutoPropertyCanBeReadOnlyAutoPropertyAnalyzer.DiagnosticId,
                Message = string.Format(PrivateSetAutoPropertyCanBeReadOnlyAutoPropertyAnalyzer.Message, "SomeProperty"),
                Severity = PrivateSetAutoPropertyCanBeReadOnlyAutoPropertyAnalyzer.Severity,
                Locations =
                    new[]
                    {
                        new DiagnosticResultLocation("Test0.cs", 9, 27)
                    }
            };


            VerifyDiagnostic(original, expectedDiagnostic);
            //VerifyFix(original, expected);
        }

        [TestMethod]
        public void PrivateSetAutoPropertyCanUseReadOnlyAutoPropertyAnalyzer_AssigningPropertyToSomethingElse_InvokesWarning()
        {
            var original = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        public int SomeProperty { get; private set; }

        void Method()
        {
            int x = SomeProperty;
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
        public int SomeProperty { get; }

        void Method()
        {
            int x = SomeProperty;
        }
    }
}";
            var expectedDiagnostic = new DiagnosticResult
            {
                Id = PrivateSetAutoPropertyCanBeReadOnlyAutoPropertyAnalyzer.DiagnosticId,
                Message = string.Format(PrivateSetAutoPropertyCanBeReadOnlyAutoPropertyAnalyzer.Message, "SomeProperty"),
                Severity = PrivateSetAutoPropertyCanBeReadOnlyAutoPropertyAnalyzer.Severity,
                Locations =
                    new[]
                    {
                        new DiagnosticResultLocation("Test0.cs", 9, 20)
                    }
            };


            VerifyDiagnostic(original, expectedDiagnostic);
            //VerifyFix(original, expected);
        }

        [TestMethod]
        public void PrivateSetAutoPropertyCanUseReadOnlyAutoPropertyAnalyzer_WithStaticProperty_WithStaticUsage_FromNestedClass_DoesNotInvokeWarning()
        {
            var original = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    public class MyClass
    {
        public static int SomeProperty { get; private set; }

        static MyClass()
        {
            SomeProperty = 42;
        }

        private class NestedClass
        {
            static NestedClass()
            {
                SomeProperty = 43;
            }
        }
    }
}";
            VerifyDiagnostic(original);
        }
    }
}