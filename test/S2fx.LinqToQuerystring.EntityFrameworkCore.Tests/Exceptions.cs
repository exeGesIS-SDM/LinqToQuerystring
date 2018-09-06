namespace LinqToQueryString.EntityFrameworkCore.Tests
{
    using System;
    using System.Data;
    using System.Linq;

    using LinqToQuerystring;

    using Machine.Specifications;
    using Microsoft.EntityFrameworkCore;

    /* We have an Id column already, so it won't happen
    public class When_using_skip_on_unordered_data : SqlPagingAndOrdering
    {
        private static Exception ex;

        private Because of = () => ex = Catch.Exception(() => result = testDb.ConcreteClasses.LinqToQuerystring("?$skip=1").ToList());

        private It should_throw_an_exception = () => ex.ShouldBeOfExactType<NotSupportedException>();
    }
    */

    /* Guess what, on EFCore we can do it.
    public class When_trying_to_order_by_complex_types : SqlPagingAndOrdering
    {
        private static Exception ex;

        private Because of = () => ex = Catch.Exception(() => complexResult = testDb.ComplexClasses.LinqToQuerystring("?$orderby=concrete").ToList());

        private It should_throw_an_exception = () => ex.ShouldBeOfExactType<ArgumentException>();
    }
    */

    /* EFCore don't have such exception named EntityCommandCompilationException
    public class When_filtering_on_endswith_function : SqlFunctions
    {
        private static Exception ex;

        private Because of = () => ex = Catch.Exception(() => testDb.ConcreteCollection.LinqToQuerystring("?$filter=endswith(Name,'day')").ToList());

        private It should_throw_an_exception = () => ex.ShouldBeOfExactType<EntityCommandCompilationException>();

        private It should_fail_due_to_SQL_CE_not_supporting_endswith =
            () =>
            ex.InnerException.Message.ShouldEqual("The function 'Reverse' is not supported by SQL Server Compact.");
    }
    */
}
