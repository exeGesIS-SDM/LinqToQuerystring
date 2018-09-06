﻿namespace LinqToQueryString.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using LinqToQueryString.Tests;

    using LinqToQuerystring;
    using LinqToQuerystring;

    using Machine.Specifications;

    public abstract class CollectionAggregates
    {
        protected static Exception ex;

        protected static IQueryable<ComplexClass> result;

        protected static IQueryable<NullableClass> nullableResult;

        protected static List<ComplexClass> complexCollection;

        protected static List<NullableClass> nullableCollection;

        private Establish context = () =>
        {
            complexCollection = new List<ComplexClass>
                                         {
                                             new ComplexClass
                                                 {
                                                     Title = "Charles", StringCollection = new List<string> { "Apple" },
                                                     IntCollection = new List<int> { 1 },
                                                     Concrete = new ConcreteClass { StringCollection = new List<string> { "Apple", "Banana" } }, 
                                                     ConcreteCollection = new List<ConcreteClass>
                                                        {
                                                            InstanceBuilders.BuildConcrete("Apple", 5, new DateTime(2005, 01, 01), true)
                                                        }
                                                 },
                                             new ComplexClass
                                                 {
                                                     Title = "Andrew", StringCollection = new List<string> { "Apple", "Banana" }, 
                                                     IntCollection = new List<int> { 1, 2 },
                                                     Concrete = new ConcreteClass { StringCollection = new List<string> { "Apple", "Banana", "Custard" } },
                                                     ConcreteCollection = new List<ConcreteClass>
                                                        {
                                                            InstanceBuilders.BuildConcrete("Apple", 5, new DateTime(2005, 01, 01), true),
                                                            InstanceBuilders.BuildConcrete("Banana", 2, new DateTime(2003, 01, 01), false)
                                                        }
                                                 },
                                             new ComplexClass
                                                 {
                                                     Title = "David", StringCollection = new List<string> { "Apple", "Banana", "Custard" }, 
                                                     IntCollection = new List<int> { 1, 2, 3 },
                                                     Concrete = new ConcreteClass { StringCollection = new List<string> { "Apple", "Custard", "Dogfood", "Eggs" } },
                                                     ConcreteCollection = new List<ConcreteClass>
                                                        {
                                                            InstanceBuilders.BuildConcrete("Apple", 5, new DateTime(2005, 01, 01), true),
                                                            InstanceBuilders.BuildConcrete("Banana", 2, new DateTime(2003, 01, 01), false),
                                                            InstanceBuilders.BuildConcrete("Custard", 3, new DateTime(2007, 01, 01), true)
                                                        }
                                                 },
                                             new ComplexClass
                                                 {
                                                     Title = "Edward", StringCollection = new List<string> { "Apple", "Custard", "Dogfood", "Eggs" }, 
                                                     IntCollection = new List<int> { 1, 3, 4, 5 },
                                                     Concrete = new ConcreteClass { StringCollection = new List<string> { "Apple", "Dogfood", "Eggs" } },
                                                     ConcreteCollection = new List<ConcreteClass>
                                                        {
                                                            InstanceBuilders.BuildConcrete("Apple", 5, new DateTime(2005, 01, 01), true),
                                                            InstanceBuilders.BuildConcrete("Custard", 3, new DateTime(2007, 01, 01), true),
                                                            InstanceBuilders.BuildConcrete("Dogfood", 4, new DateTime(2009, 01, 01), false),
                                                            InstanceBuilders.BuildConcrete("Eggs", 1, new DateTime(2000, 01, 01), true)
                                                        }
                                                 },
                                             new ComplexClass
                                                 {
                                                     Title = "Boris", StringCollection = new List<string> { "Apple", "Dogfood", "Eggs" }, 
                                                     IntCollection = new List<int> { 1, 4, 5 },
                                                     Concrete = new ConcreteClass { StringCollection = new List<string> { "Apple" } },
                                                     ConcreteCollection = new List<ConcreteClass>
                                                        {
                                                            InstanceBuilders.BuildConcrete("Apple", 5, new DateTime(2005, 01, 01), true),
                                                            InstanceBuilders.BuildConcrete("Dogfood", 4, new DateTime(2009, 01, 01), false),
                                                            InstanceBuilders.BuildConcrete("Eggs", 1, new DateTime(2000, 01, 01), true)
                                                        }
                                                 }
                                         };

            nullableCollection = new List<NullableClass>
                                 {
                                     new NullableClass { NullableInts = new List<int?> { null } },
                                     new NullableClass { NullableInts = new List<int?> { 1 } },
                                     new NullableClass { NullableInts = new List<int?> { 1, 2 } },
                                     new NullableClass { NullableInts = new List<int?> { null, 1, 2 } },
                                 };
        };
    }

    #region Int Collections

    public class When_filtering_on_a_simple_collection_property_using_max_against_an_int : CollectionAggregates
    {
        private Because of = () => result = complexCollection.AsQueryable().LinqToQuerystring("$filter=IntCollection/max() eq 5");

        private It should_return_two_records = () => result.Count().ShouldEqual(2);

        private It should_only_return_records_where_max_value_is_5 = () => result.ShouldEachConformTo(o => o.IntCollection.Max() == 5);
    }

    public class When_filtering_on_a_simple_collection_property_using_min_against_an_int : CollectionAggregates
    {
        private Because of = () => result = complexCollection.AsQueryable().LinqToQuerystring("$filter=IntCollection/min() eq 1");

        private It should_return_five_records = () => result.Count().ShouldEqual(5);

        private It should_only_return_records_where_min_value_is_1 = () => result.ShouldEachConformTo(o => o.IntCollection.Min() == 1);
    }

    public class When_filtering_on_a_simple_collection_property_using_sum_against_an_int : CollectionAggregates
    {
        private Because of = () => result = complexCollection.AsQueryable().LinqToQuerystring("$filter=IntCollection/sum() gt 6");

        private It should_return_two_records = () => result.Count().ShouldEqual(2);

        private It should_only_return_records_where_sum_of_values_is_greater_than_6 = () => result.ShouldEachConformTo(o => o.IntCollection.Sum() > 6);
    }

    public class When_filtering_on_a_simple_collection_property_using_average_against_an_int : CollectionAggregates
    {
        private Because of = () => result = complexCollection.AsQueryable().LinqToQuerystring("$filter=IntCollection/average() ge 2");

        private It should_return_three_records = () => result.Count().ShouldEqual(3);

        private It should_only_return_records_where_average_of_values_is_greater_than_or_equal_to_2 = () => result.ShouldEachConformTo(o => o.IntCollection.Average() >= 2);
    }

    #endregion

    #region Nullable Int Collections

    public class When_filtering_on_a_simple_collection_property_using_max_against_a_nullable_int : CollectionAggregates
    {
        private Because of = () => nullableResult = nullableCollection.AsQueryable().LinqToQuerystring("$filter=NullableInts/max() eq 2");

        private It should_return_two_records = () => nullableResult.Count().ShouldEqual(2);

        private It should_only_return_records_where_max_value_is_2 = () => nullableResult.ShouldEachConformTo(o => o.NullableInts.Max() == 2);
    }

    public class When_filtering_on_a_simple_collection_property_using_min_against_a_nullable_int : CollectionAggregates
    {
        private Because of = () => nullableResult = nullableCollection.AsQueryable().LinqToQuerystring("$filter=NullableInts/min() eq 1");

        private It should_return_three_records = () => nullableResult.Count().ShouldEqual(3);

        private It should_only_return_records_where_min_value_is_1 = () => nullableResult.ShouldEachConformTo(o => o.NullableInts.Min() == 1);
    }

    public class When_filtering_on_a_simple_collection_property_using_sum_against_a_nullable_int : CollectionAggregates
    {
        private Because of = () => nullableResult = nullableCollection.AsQueryable().LinqToQuerystring("$filter=NullableInts/sum() gt 2");

        private It should_return_two_records = () => nullableResult.Count().ShouldEqual(2);

        private It should_only_return_records_where_sum_of_values_is_greater_than_2 = () => nullableResult.ShouldEachConformTo(o => o.NullableInts.Sum() > 2);
    }

    public class When_filtering_on_a_simple_collection_property_using_average_against_a_nullable_int : CollectionAggregates
    {
        private Because of = () => nullableResult = nullableCollection.AsQueryable().LinqToQuerystring("$filter=NullableInts/average() ge 1");

        private It should_return_three_records = () => nullableResult.Count().ShouldEqual(3);

        private It should_only_return_records_where_average_of_values_is_greater_than_or_equal_to_1 = () => nullableResult.ShouldEachConformTo(o => o.NullableInts.Average() >= 1);
    }

    public class When_filtering_on_a_nullable_int_collection_property_using_any_checking_for_nulls : CollectionAggregates
    {
        private Because of = () => nullableResult = nullableCollection.AsQueryable().LinqToQuerystring("$filter=NullableInts/any(int: int eq null)");

        private It should_return_two_records = () => nullableResult.Count().ShouldEqual(2);

        private It should_only_return_records_where_string_collection_contains_banana = () => nullableResult.ShouldEachConformTo(o => o.NullableInts.Any(s => s == null));
    }

    #endregion

    #region String collections

    public class When_filtering_on_a_simple_collection_property_using_any : CollectionAggregates
    {
        private Because of = () => result = complexCollection.AsQueryable().LinqToQuerystring("$filter=StringCollection/any(tag: tag eq 'Banana')");

        private It should_return_two_records = () => result.Count().ShouldEqual(2);

        private It should_only_return_records_where_string_collection_contains_banana = () => result.ShouldEachConformTo(o => o.StringCollection.Any(s => s == "Banana"));
    }

    public class When_filtering_on_a_simple_collection_property_using_any_with_an_or : CollectionAggregates
    {
        private Because of = () => result = complexCollection.AsQueryable().LinqToQuerystring("$filter=StringCollection/any(tag: tag eq 'Banana' or tag eq 'Eggs')");

        private It should_return_four_records = () => result.Count().ShouldEqual(4);

        private It should_only_return_records_where_string_collection_contains_banana_or_eggs = () => result.ShouldEachConformTo(o => o.StringCollection.Any(s => s == "Banana" || s == "Eggs"));
    }

    public class When_filtering_on_a_simple_collection_property_using_any_with_functions : CollectionAggregates
    {
        private Because of = () => result = complexCollection.AsQueryable().LinqToQuerystring("$filter=StringCollection/any(tag: startswith(tag,'Dog'))");

        private It should_return_two_records = () => result.Count().ShouldEqual(2);

        private It should_only_return_records_where_string_collection_contains_value_starting_with_dog = () => result.ShouldEachConformTo(o => o.StringCollection.Any(s => s.StartsWith("Dog")));
    }

    public class When_filtering_on_a_simple_collection_property_using_all : CollectionAggregates
    {
        private Because of = () => result = complexCollection.AsQueryable().LinqToQuerystring("$filter=StringCollection/all(tag: tag eq 'Apple')");

        private It should_return_one_records = () => result.Count().ShouldEqual(1);

        private It should_only_return_records_where_all_string_collection_records_are_apple = () => result.ShouldEachConformTo(o => o.StringCollection.All(s => s == "Apple"));
    }

    public class When_filtering_on_a_simple_collection_property_using_all_with_an_or : CollectionAggregates
    {
        private Because of = () => result = complexCollection.AsQueryable().LinqToQuerystring("$filter=StringCollection/all(tag: tag eq 'Apple' or tag eq 'Banana')");

        private It should_return_two_records = () => result.Count().ShouldEqual(2);

        private It should_only_return_records_where_all_string_collection_records_are_apple_or_banana = () => result.ShouldEachConformTo(o => o.StringCollection.All(s => s == "Apple" || s == "Banana"));
    }

    public class When_filtering_on_a_simple_collection_property_using_all_with_functions : CollectionAggregates
    {
        private Because of = () => result = complexCollection.AsQueryable().LinqToQuerystring("$filter=StringCollection/all(tag: startswith(tag,'App'))");

        private It should_return_one_record = () => result.Count().ShouldEqual(1);

        private It should_only_return_records_where_all_string_collection_records_start_with_app = () => result.ShouldEachConformTo(o => o.StringCollection.All(s => s.StartsWith("App")));
    }

    public class When_filtering_on_a_simple_collection_property_using_count : CollectionAggregates
    {
        private Because of = () => result = complexCollection.AsQueryable().LinqToQuerystring("$filter=StringCollection/count() ge 3");

        private It should_return_three_records = () => result.Count().ShouldEqual(3);

        private It should_only_return_records_where_string_collection_count_is_greater_than_or_equal_to_3 = () => result.ShouldEachConformTo(o => o.StringCollection.Count() >= 3);
    }

    public class When_filtering_on_a_simple_collection_property_using_max_against_a_string : CollectionAggregates
    {
        private Because of = () => result = complexCollection.AsQueryable().LinqToQuerystring("$filter=StringCollection/max() eq 'Eggs'");

        private It should_return_two_records = () => result.Count().ShouldEqual(2);

        private It should_only_return_records_where_max_value_of_string_collection_is_eggs = () => result.ShouldEachConformTo(o => o.StringCollection.Max() == "Eggs");
    }

    public class When_filtering_on_a_simple_collection_property_using_min_against_a_string : CollectionAggregates
    {
        private Because of = () => result = complexCollection.AsQueryable().LinqToQuerystring("$filter=StringCollection/min() eq 'Apple'");

        private It should_return_five_records = () => result.Count().ShouldEqual(5);

        private It should_only_return_records_where_min_value_of_string_collection_is_apple = () => result.ShouldEachConformTo(o => o.StringCollection.Min() == "Apple");
    }

    #endregion

    #region Complex collections

    public class When_filtering_on_a_complex_collection_property_using_any : CollectionAggregates
    {
        private Because of = () => result = complexCollection.AsQueryable().LinqToQuerystring("$filter=ConcreteCollection/any(concrete: concrete/Name eq 'Banana')");

        private It should_return_two_records = () => result.Count().ShouldEqual(2);

        private It should_only_return_records_where_concrete_collection_contains_value_with_name_banana = () => result.ShouldEachConformTo(o => o.ConcreteCollection.Any(s => s.Name == "Banana"));
    }

    public class When_filtering_on_a_complex_collection_property_using_any_with_an_or : CollectionAggregates
    {
        private Because of = () => result = complexCollection.AsQueryable().LinqToQuerystring("$filter=ConcreteCollection/any(concrete: concrete/Name eq 'Banana' or concrete/Name eq 'Eggs')");

        private It should_return_four_records = () => result.Count().ShouldEqual(4);

        private It should_only_return_records_where_concrete_collection_contains_value_with_name_banana_or_eggs = () => result.ShouldEachConformTo(o => o.ConcreteCollection.Any(s => s.Name == "Banana" || s.Name == "Eggs"));
    }

    public class When_filtering_on_a_complex_collection_property_using_any_with_functions : CollectionAggregates
    {
        private Because of = () => result = complexCollection.AsQueryable().LinqToQuerystring("$filter=ConcreteCollection/any(concrete: startswith(concrete/Name,'Dog'))");

        private It should_return_two_records = () => result.Count().ShouldEqual(2);

        private It should_only_return_records_where_concrete_collection_contains_value_with_name_starting_with_dog = () => result.ShouldEachConformTo(o => o.ConcreteCollection.Any(s => s.Name.StartsWith("Dog")));
    }

    public class When_filtering_on_a_complex_collection_property_using_all : CollectionAggregates
    {
        private Because of = () => result = complexCollection.AsQueryable().LinqToQuerystring("$filter=ConcreteCollection/all(concrete: concrete/Name eq 'Apple')");

        private It should_return_one_records = () => result.Count().ShouldEqual(1);

        private It should_only_return_records_where_all_concrete_collection_values_have_name_apple = () => result.ShouldEachConformTo(o => o.ConcreteCollection.All(s => s.Name == "Apple"));
    }

    public class When_filtering_on_a_complex_collection_property_using_all_with_an_or : CollectionAggregates
    {
        private Because of = () => result = complexCollection.AsQueryable().LinqToQuerystring("$filter=ConcreteCollection/all(concrete: concrete/Name eq 'Apple' or concrete/Name eq 'Banana')");

        private It should_return_two_records = () => result.Count().ShouldEqual(2);

        private It should_only_return_records_where_all_concrete_collection_values_have_name_apple_or_banana = () => result.ShouldEachConformTo(o => o.ConcreteCollection.All(s => s.Name == "Apple" || s.Name == "Banana"));
    }

    public class When_filtering_on_a_complex_collection_property_using_all_with_functions : CollectionAggregates
    {
        private Because of = () => result = complexCollection.AsQueryable().LinqToQuerystring("$filter=ConcreteCollection/all(concrete: startswith(concrete/Name,'App'))");

        private It should_return_one_record = () => result.Count().ShouldEqual(1);

        private It should_only_return_records_where_all_concrete_collection_values_have_name_starting_with_app = () => result.ShouldEachConformTo(o => o.ConcreteCollection.All(s => s.Name.StartsWith("App")));
    }

    public class When_filtering_on_a_complex_collection_property_using_count : CollectionAggregates
    {
        private Because of = () => result = complexCollection.AsQueryable().LinqToQuerystring("$filter=ConcreteCollection/count() ge 3");

        private It should_return_three_records = () => result.Count().ShouldEqual(3);

        private It should_only_return_records_where_concrete_collection_count_is_greater_than_3 = () => result.ShouldEachConformTo(o => o.ConcreteCollection.Count() >= 3);
    }

    #endregion

    #region Nested Simple collections

    public class When_filtering_on_a_nested_simple_collection_property_using_any : CollectionAggregates
    {
        private Because of = () => result = complexCollection.AsQueryable().LinqToQuerystring("$filter=Concrete/StringCollection/any(string: string eq 'Banana')");

        private It should_return_two_records = () => result.Count().ShouldEqual(2);

        private It should_only_return_records_where_concrete_stringcollection_contains_banana = () => result.ShouldEachConformTo(o => o.Concrete.StringCollection.Any(s => s == "Banana"));
    }

    #endregion
}
