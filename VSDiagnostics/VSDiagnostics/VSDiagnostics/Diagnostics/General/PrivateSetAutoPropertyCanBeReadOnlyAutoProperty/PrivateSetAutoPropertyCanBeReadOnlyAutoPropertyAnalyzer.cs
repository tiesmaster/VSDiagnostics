using System.Collections.Generic;
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

        private readonly HashSet<PropertyDeclarationSyntax> _affectedProperties = new HashSet<PropertyDeclarationSyntax>(); 

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(AnalyzeSymbol, SyntaxKind.SimpleAssignmentExpression, SyntaxKind.PropertyDeclaration);
            context.RegisterCompilationAction(ReportDiagnostics);
        }

        private void ReportDiagnostics(CompilationAnalysisContext context)
        {
            foreach (var property in _affectedProperties)
            {
                context.ReportDiagnostic(Diagnostic.Create(Rule, property.Identifier.GetLocation(), property.Identifier.ValueText));
            }
        }

        /// <summary>
        ///     The overarching idea behind this approach is that we register for an action on each assignment expression.
        ///     This has as result that the analyzer is triggered each time someone wants to assign a value to something else.
        ///     We do this so the analyzer will be triggered when the property in question would be assigned.
        ///     If we would instead trigger the analyzer on property declaration, the analyzer would only be triggered at the
        ///     loading of the document and whenever we change something about the property's syntax.
        ///     Each time the analyzer is run, we gather all properties in the outer class and check for each of them if they apply
        ///     for a warning or not.
        ///     Thoughts for an optimization: check each property at load time but only check the affected property when triggered
        ///     at runtime. (currently not implemented)
        /// </summary>
        private void AnalyzeSymbol(SyntaxNodeAnalysisContext context)
        {
            var outerClass = context.Node.Ancestors().OfType<ClassDeclarationSyntax>().LastOrDefault();
            if (outerClass == null)
            {
                return;
            }

            foreach (var property in outerClass.DescendantNodes().OfType<PropertyDeclarationSyntax>())
            {
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

                        // If it is a setter but outside the constructor, we don't report any diagnostic
                        if (!isInConstructor)
                        {
                            return;
                        }

                        // If we're assigning the property from a nested class, we don't report anything either
                        var currentClass = context.SemanticModel.GetDeclaredSymbol(identifier.Ancestors().OfType<ClassDeclarationSyntax>().FirstOrDefault());
                        var propertyDeclaringClass = context.SemanticModel.GetDeclaredSymbol(property.Ancestors().OfType<ClassDeclarationSyntax>().FirstOrDefault());
                        if (!currentClass.Equals(propertyDeclaringClass))
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
                    _affectedProperties.Add(property);
                }
            }
        }
    }
}