namespace VessiFlowers.UnitTests.MockContainers
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Moq;

    public abstract class MockContainer
    {
        protected void MockField<TContext, T>(Mock<TContext> mockContext, List<T> data, Expression<Func<TContext, object>> field)
            where TContext : DbContext
            where T : class
        {
            var mockSet = new Mock<DbSet<T>>();

            mockSet.As<IDbAsyncEnumerable<T>>()
               .Setup(m => m.GetAsyncEnumerator())
               .Returns(new AsyncEnumerator<T>(data.GetEnumerator()));

            mockSet.As<IQueryable<T>>()
                .Setup(m => m.Provider)
                .Returns(new AsyncQueryProvider<T>(data.AsQueryable().Provider));

            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.AsQueryable().Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.AsQueryable().ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            mockSet.Setup(d => d.Add(It.IsAny<T>())).Callback<T>((s) => data.Add(s));
            mockSet.Setup(d => d.Include(It.IsAny<string>())).Returns(mockSet.Object);
            mockContext.Setup(field).Returns(mockSet.Object);
        }

        private class AsyncQueryProvider<TEntity> : IDbAsyncQueryProvider
        {
            private readonly IQueryProvider inner;

            internal AsyncQueryProvider(IQueryProvider inner)
            {
                this.inner = inner;
            }

            public IQueryable CreateQuery(Expression expression) => new AsyncEnumerable<TEntity>(expression);

            public IQueryable<TElement> CreateQuery<TElement>(Expression expression) => new AsyncEnumerable<TElement>(expression);

            public object Execute(Expression expression) => this.inner.Execute(expression);

            public TResult Execute<TResult>(Expression expression) => this.inner.Execute<TResult>(expression);

            public Task<object> ExecuteAsync(Expression expression, CancellationToken cancellationToken) => Task.FromResult(this.Execute(expression));

            public Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken) => Task.FromResult(this.Execute<TResult>(expression));
        }

        private class AsyncEnumerable<T> : EnumerableQuery<T>, IDbAsyncEnumerable<T>, IQueryable<T>
        {
            public AsyncEnumerable(Expression expression)
                : base(expression)
            {
            }

            IQueryProvider IQueryable.Provider => new AsyncQueryProvider<T>(this);

            public IDbAsyncEnumerator<T> GetAsyncEnumerator() => new AsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());

            IDbAsyncEnumerator IDbAsyncEnumerable.GetAsyncEnumerator() => this.GetAsyncEnumerator();
        }

        private class AsyncEnumerator<T> : IDbAsyncEnumerator<T>
        {
            private readonly IEnumerator<T> inner;

            public AsyncEnumerator(IEnumerator<T> inner)
            {
                this.inner = inner;
            }

            public T Current => this.inner.Current;

            object IDbAsyncEnumerator.Current => this.Current;

            public void Dispose() => this.inner.Dispose();

            public Task<bool> MoveNextAsync(CancellationToken cancellationToken) => Task.FromResult(this.inner.MoveNext());
        }
    }
}
