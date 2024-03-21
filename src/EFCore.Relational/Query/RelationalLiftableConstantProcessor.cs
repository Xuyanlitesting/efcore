// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.EntityFrameworkCore.Query.Internal;

namespace Microsoft.EntityFrameworkCore.Query;

#pragma warning disable EF1001 // LiftableConstantProcessor is internal

/// <summary>
/// TODO
/// </summary>
public class RelationalLiftableConstantProcessor : LiftableConstantProcessor
{
    private readonly RelationalMaterializerLiftableConstantContext _relationalMaterializerLiftableConstantContext;

    /// <summary>
    /// TODO
    /// </summary>
    public RelationalLiftableConstantProcessor(
        ShapedQueryCompilingExpressionVisitorDependencies dependencies,
        RelationalShapedQueryCompilingExpressionVisitorDependencies relationalDependencies)
        : base(dependencies)
        => _relationalMaterializerLiftableConstantContext = new(dependencies, relationalDependencies);

    /// <inheritdoc/>
    protected override ConstantExpression InlineConstant(LiftableConstantExpression liftableConstant)
    {
        if (liftableConstant.ResolverExpression is Expression<Func<RelationalMaterializerLiftableConstantContext, object>>
            resolverExpression)
        {
            var resolver = resolverExpression.Compile(preferInterpretation: true);
            var value = resolver(_relationalMaterializerLiftableConstantContext);
            return Expression.Constant(value, liftableConstant.Type);
        }

        return base.InlineConstant(liftableConstant);
    }
}