using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoslynTester.Helpers.CSharp;
using VSDiagnostics.Diagnostics.General.FieldCanBeMadeReadonly;

namespace VSDiagnostics.Test.Tests.General
{
    [TestClass]
    public class FieldCanBeMadeReadonlyTests : CSharpDiagnosticVerifier
    {
        private int x;

        protected override DiagnosticAnalyzer DiagnosticAnalyzer => new FieldCanBeMadeReadonlyAnalyzer();

        [TestMethod]
        public void FieldCanBeMadeReadonly_OnlyAssignedInCtor_InvokesWarning()
        {
            var original = @"
namespace ConsoleApplication1
{
    class MyClass
    {
        private int x;
        MyClass()
        {
            x = 1;
        }
    }
}";

            var result = @"
namespace ConsoleApplication1
{
    class MyClass
    {
        private readonly int x;
        MyClass()
        {
            x = 1;
        }
    }
}";

            VerifyDiagnostic(original, FieldCanBeMadeReadonlyAnalyzer.Rule.MessageFormat.ToString());
            //VerifyFix(original, result);
        }

        [TestMethod]
        public void FieldCanBeMadeReadonly_WithReadonlyKeyword_DoesNotDisplayWarning()
        {
            var original = @"
namespace ConsoleApplication1
{
    class MyClass
    {
        private readonly int x;
        MyClass()
        {
            x = 1;
        }
    }
}";


            VerifyDiagnostic(original);
        }
    }
}
