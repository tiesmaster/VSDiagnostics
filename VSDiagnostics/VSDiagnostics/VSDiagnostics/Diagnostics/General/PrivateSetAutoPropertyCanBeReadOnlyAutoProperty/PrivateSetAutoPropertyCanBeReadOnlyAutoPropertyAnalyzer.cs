using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace VSDiagnostics.Diagnostics.General.PrivateSetAutoPropertyCanBeReadOnlyAutoProperty
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class PrivateSetAutoPropertyCanBeReadOnlyAutoPropertyAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = nameof(PrivateSetAutoPropertyCanBeReadOnlyAutoPropertyAnalyzer);
        internal const string Title = "A private set property can be made readonly.";
        internal const string Message = "Property {0} can be made readonly.";
        internal const string Category = "General";
        internal const DiagnosticSeverity Severity = DiagnosticSeverity.Warning;
        internal static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, Message, Category, Severity, true);
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(AnalyzeSymbol, SyntaxKind.PropertyDeclaration);
        }

        private void AnalyzeSymbol(SyntaxNodeAnalysisContext context)
        {
            var property = context.Node as PropertyDeclarationSyntax;
            if (property == null)
            {
                return;
            }

            var setAccessor = property.AccessorList.Accessors.FirstOrDefault(x => x.IsKind(SyntaxKind.SetAccessorDeclaration));
            if (setAccessor == null)
            {
                return;
            }

            if (!setAccessor.Modifiers.Any(SyntaxKind.PrivateKeyword))
            {
                return;
            }

            var propertySymbol = context.SemanticModel.GetDeclaredSymbol(property);
            if (propertySymbol == null)
            {
                return;
            }
            var isStaticProperty = propertySymbol.IsStatic;

            var outerClass = property.Ancestors().OfType<ClassDeclarationSyntax>().LastOrDefault();
            if (outerClass == null)
            {
                return;
            }

            var hasInstanceUsage = false;
            var hasStaticUsage = false;

            foreach (var identifier in outerClass.DescendantNodes().Where(x =>
                x is IdentifierNameSyntax || /* SomeProperty = 42; */
                x is MemberAccessExpressionSyntax) /* this.SomeProperty == 42 */)
            {
                var memberSymbol = context.SemanticModel.GetSymbolInfo(identifier);
                if (memberSymbol.Symbol == null)
                {
                    continue;
                }
                if (memberSymbol.Symbol.Equals(propertySymbol))
                {
                    var constructor = identifier.Ancestors().OfType<ConstructorDeclarationSyntax>().FirstOrDefault();
                    var isInConstructor = constructor != null;
                    var isAssignmentExpression = identifier.Ancestors().OfType<AssignmentExpressionSyntax>().FirstOrDefault() != null;

                    // Skip anything that isn't a setter
                    if (!isAssignmentExpression)
                    {
                        continue;
                    }

                    // if it is a setter but outside the constructor, we don't report any diagnostic
                    if (!isInConstructor)
                    {
                        return;
                    }

                    var isStaticConstructor = context.SemanticModel.GetDeclaredSymbol(constructor).IsStatic;
                    if (isStaticConstructor)
                    {
                        hasStaticUsage = true;
                    }
                    else
                    {
                        hasInstanceUsage = true;
                    }
                }
            }

            if ((hasStaticUsage && isStaticProperty && !hasInstanceUsage) ||
                (hasInstanceUsage && !hasStaticUsage && !isStaticProperty) ||
                (!hasStaticUsage && !hasInstanceUsage))
            {
                context.ReportDiagnostic(Diagnostic.Create(Rule, property.Identifier.GetLocation(), propertySymbol.Name));
            }
        }
    }
}