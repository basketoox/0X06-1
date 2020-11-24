using System.Crosscutting.Adapter;

namespace System.Crosscutting.Extensions
{
    public static class TypeAdapterExtension
    {
        public static void Adapter(this object target, ITypeAdapter adapter, object source)
        {
            adapter.Adapt(source, target);
        }
    }
}
