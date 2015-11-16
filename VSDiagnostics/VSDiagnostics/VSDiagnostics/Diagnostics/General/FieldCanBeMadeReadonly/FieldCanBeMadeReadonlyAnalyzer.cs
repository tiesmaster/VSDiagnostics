using System;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using VSDiagnostics.Utilities;

namespace VSDiagnostics.Diagnostics.General.FieldCanBeMadeReadonly
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class FieldCanBeMadeReadonlyAnalyzer : DiagnosticAnalyzer
    {
        private const DiagnosticSeverity Severity = DiagnosticSeverity.Warning;

        private static readonly string Category = VSDiagnosticsResources.GeneralCategory;
        private static readonly string Message = VSDiagnosticsResources.FieldCanBeMadeReadonlyAnalyzerMessage;
        private static readonly string Title = VSDiagnosticsResources.FieldCanBeMadeReadonlyAnalyzerTitle;
        internal static DiagnosticDescriptor Rule
            => new DiagnosticDescriptor(DiagnosticId.FieldCanBeMadeReadonly, Title, Message, Category, Severity, true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(AnalyzeFieldDeclaration, SyntaxKind.FieldDeclaration);
        }

        private void AnalyzeFieldDeclaration(SyntaxNodeAnalysisContext context)
        {
            var fieldNode = context.Node as FieldDeclarationSyntax;
            if (fieldNode == null)
            {
                return;
            }

            //if (fieldNode.Modifiers.Any(SyntaxKind.ReadOnlyKeyword))
            //{
            //    return;
            //}

            var declaration = fieldNode.Declaration;
            // TODO: check all variable declarations
            var node = declaration.Variables.First();

            var declaredSymbol = context.SemanticModel.GetDeclaredSymbol(node);
            var root = fieldNode.SyntaxTree.GetRoot();
            var identifiers = root.DescendantNodes().OfType<IdentifierNameSyntax>().ToArray();
            foreach (var id in identifiers)
            {
                var si = context.SemanticModel.GetSymbolInfo(id);
                var s = si.Symbol;
                if (s.Equals(declaredSymbol))
                {
                    var parentMethod = id.Ancestors().OfType<MethodDeclarationSyntax>();
                }

            }
            
        }
    }
}
