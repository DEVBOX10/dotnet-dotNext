using System;
using System.Linq.Expressions;
using static System.Threading.Thread;

namespace DotNext.Metaprogramming
{
    using Linq.Expressions;

    /// <summary>
    /// Represents compound expresssion builder.
    /// </summary>
    /// <remarks>
    /// Any derived expression builder is not thread-safe and event cannot
    /// be shared between threads.
    /// </remarks>
    /// <typeparam name="E">Type of expression to be constructed.</typeparam>
    public abstract class ExpressionBuilder<E> : IExpressionBuilder<E>
        where E : Expression
    {
        private LexicalScope currentScope;

        private Type expressionType;
        private readonly int ownerThread;

        private protected ExpressionBuilder(LexicalScope currentScope)
        {
            this.currentScope = currentScope;
            ownerThread = CurrentThread.ManagedThreadId;
        }

        private protected Type Type => expressionType ?? typeof(void);

        private protected void VerifyCaller()
        {
            if (ownerThread != CurrentThread.ManagedThreadId)
                throw new InvalidOperationException();
        }

        /// <summary>
        /// Changes type of the expression.
        /// </summary>
        /// <remarks>
        /// By default, type of expression is <see cref="void"/>.
        /// </remarks>
        /// <param name="expressionType">The expression type.</param>
        /// <returns>This builder.</returns>
        public ExpressionBuilder<E> OfType(Type expressionType)
        {
            VerifyCaller();
            this.expressionType = expressionType;
            return this;
        }

        /// <summary>
        /// Changes type of the expression.
        /// </summary>
        /// <typeparam name="T">The expression type.</typeparam>
        /// <returns>This builder.</returns>
        public ExpressionBuilder<E> OfType<T>() => OfType(typeof(T));

        private protected abstract E Build();

        E IExpressionBuilder<E>.Build() => Build();

        /// <summary>
        /// Finalizes construction of the expression
        /// and adds constructed expression as statement to the entire lexical scope.
        /// </summary>
        /// <exception cref="InvalidOperationException">The expression has been constructed already.</exception>
        public void End()
        {
            if(currentScope is null)
                throw new InvalidOperationException();
            currentScope.AddStatement(Build());
            currentScope = null;
        }
    }
}