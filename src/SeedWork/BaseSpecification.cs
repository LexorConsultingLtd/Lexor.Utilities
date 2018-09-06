using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Utilities.DataTables;

namespace Utilities.SeedWork
{
    // https://github.com/dotnet-architecture/eShopOnWeb
    public abstract class BaseSpecification<T> : ISpecification<T>
    {
        protected BaseSpecification(Expression<Func<T, bool>> criteria = null, DataTablesAjaxPostModel model = null)
        {
            Criteria = criteria;
            Model = model;
        }

        public Expression<Func<T, bool>> Criteria { get; protected set; }
        public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();
        public List<string> IncludeStrings { get; } = new List<string>();

        public DataTablesAjaxPostModel Model { get; protected set; }

        protected virtual void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }

        // string-based includes allow for including children of children, e.g. Basket.Items.Product
        protected virtual void AddInclude(params string[] includeString)
        {
            IncludeStrings.Add(string.Join(".", includeString));
        }

        #region SpecIncludeNode

        protected class SpecIncludeNode
        {
            public string Name { get; }
            public SpecIncludeNode[] Children { get; }

            public SpecIncludeNode(string name, SpecIncludeNode[] children = null)
            {
                Name = name;
                Children = children;
            }
        }

        #endregion

        protected virtual void AddInclude(SpecIncludeNode includeNode, string[] parentIncludes = null)
        {
            var includes = new List<string>();
            if (parentIncludes != null)
                includes.AddRange(parentIncludes);

            includes.Add(includeNode.Name);
            var includesArray = includes.ToArray();
            AddInclude(includesArray);

            foreach (var child in includeNode.Children ?? new SpecIncludeNode[] { })
            {
                AddInclude(child, includesArray);
            }
        }
    }
}
