using System;

namespace WeSave.Data
{
    public abstract class AOutput<TData>
    {
        public abstract AOutput<TData> FromData(TData data);
    }
}