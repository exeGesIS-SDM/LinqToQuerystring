﻿namespace LinqToQueryString.EntityFrameworkCore.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;

    using LinqToQueryString.Tests;

    using LinqToQuerystring;

    using Machine.Specifications;

    public abstract class SqlFunctions
    {
        protected static TestDbContext testDb;

        protected static List<ConcreteClass> result;

        protected static List<ConcreteClass> concreteCollection;

        private Establish context = () =>
        {
            testDb = new TestDbContext();

            testDb.Database.ExecuteSqlCommand("UPDATE ComplexClasses SET ConcreteId = NULL");
            testDb.Database.ExecuteSqlCommand("DELETE FROM EdgeCaseClasses");
            testDb.Database.ExecuteSqlCommand("DELETE FROM ConcreteClasses");
            testDb.Database.ExecuteSqlCommand("DELETE FROM ComplexClasses");

            testDb.ConcreteClasses.Add(
                InstanceBuilders.BuildConcrete("Saturday", 1, new DateTime(2001, 01, 01), true));
            testDb.ConcreteClasses.Add(InstanceBuilders.BuildConcrete("Satnav", 2, new DateTime(2002, 01, 01), false));
            testDb.ConcreteClasses.Add(InstanceBuilders.BuildConcrete("Saturnalia", 3, new DateTime(2003, 01, 01), true));
            testDb.ConcreteClasses.Add(InstanceBuilders.BuildConcrete("Saturn", 4, new DateTime(2004, 01, 01), true));
            testDb.ConcreteClasses.Add(InstanceBuilders.BuildConcrete("Monday", 5, new DateTime(2005, 01, 01), true));
            testDb.ConcreteClasses.Add(InstanceBuilders.BuildConcrete("Tuesday", 5, new DateTime(2005, 01, 01), true));
            testDb.ConcreteClasses.Add(InstanceBuilders.BuildConcrete("Burns", 5, new DateTime(2005, 01, 01), true));

            testDb.SaveChanges();

            concreteCollection = testDb.ConcreteClasses.ToList();

            testDb = new TestDbContext();
        };
    }

    public class When_filtering_on_startswith_function : SqlFunctions
    {
        private Because of =
            () => result = testDb.ConcreteClasses.LinqToQuerystring("?$filter=startswith(Name,'Sat')").ToList();

        private It should_return_four_records = () => result.Count().ShouldEqual(4);

        private It should_only_return_records_where_name_starts_with_Sat =
            () => result.ShouldEachConformTo(o => o.Name.StartsWith("Sat"));
    }

    public class When_filtering_on_substringof_function : SqlFunctions
    {
        private Because of =
            () => result = testDb.ConcreteClasses.LinqToQuerystring("?$filter=substringof('urn',Name)").ToList();

        private It should_return_three_records = () => result.Count().ShouldEqual(3);

        private It should_only_return_records_where_name_contains_urn =
            () => result.ShouldEachConformTo(o => o.Name.Contains("urn"));
    }

    public class When_filtering_on_multiple_substringof_functions : SqlFunctions
    {
        private Because of =
            () =>
            result =
            testDb.ConcreteClasses.LinqToQuerystring(
                "?$filter=(substringof('Mond',Name)) or (substringof('Tues',Name))").ToList();

        private It should_return_three_records = () => result.Count().ShouldEqual(2);

        private It should_only_return_records_where_name_contains_urn =
            () => result.ShouldEachConformTo(o => o.Name.Contains("Mond") || o.Name.Contains("Tues"));
    }

    public class When_filtering_on_substringof_function_with_escape_character : SqlFiltering
    {
        private Because of =
            () => edgeCaseResult = testDb.EdgeCaseClasses.LinqToQuerystring(@"?$filter=substringof('\\',Name)").ToList();

        private It should_return_one_record = () => edgeCaseResult.Count().ShouldEqual(1);

        private It should_only_return_records_where_name_contains_escaped_slash =
            () => edgeCaseResult.ShouldEachConformTo(o => o.Name.Contains("\\"));
    }

    public class When_filtering_on_substringof_function_with_tolower : SqlFunctions
    {
        private Because of =
            () => result = testDb.ConcreteClasses.LinqToQuerystring(@"?$filter=substringof('sat',tolower(Name))").ToList();

        private It should_return_four_records = () => result.Count().ShouldEqual(4);

        private It should_only_return_records_where_name_contains_sat =
            () => result.ShouldEachConformTo(o => o.Name.Contains("Sat"));
    }

    public class When_filtering_on_substringof_function_with_toupper : SqlFunctions
    {
        private Because of =
            () => result = testDb.ConcreteClasses.LinqToQuerystring(@"?$filter=substringof('SAT',toupper(Name))").ToList();

        private It should_return_four_records = () => result.Count().ShouldEqual(4);

        private It should_only_return_records_where_name_contains_sat =
            () => result.ShouldEachConformTo(o => o.Name.Contains("Sat"));
    }
}
