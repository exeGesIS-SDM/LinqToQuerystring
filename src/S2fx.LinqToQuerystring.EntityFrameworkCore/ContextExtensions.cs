namespace LinqToQuerystring
{
    using LinqToQuerystring.EntityFrameworkCore;
    using LinqToQuerystring.Utils;

    public static class ContextExtensions
    {
        public static void WithEntityFrameworkCore(this Context context)
        {
            if (!context.CustomNodes.ContainsKey(WellknownConstants.EFCoreCustomNodesKey))
            {
                context.CustomNodes.Add(WellknownConstants.EFCoreCustomNodesKey, new CustomNodeMappings());
            }

            var objectQueryNodes = context.CustomNodes[WellknownConstants.EFCoreCustomNodesKey];
            if (!objectQueryNodes.ContainsKey(LinqToQuerystringLexer.EXPAND))
            {
                objectQueryNodes.Add(
                    LinqToQuerystringLexer.EXPAND, (type, token, factory) => new ExpandNode(type, token, factory));
            }
        }
    }
}
