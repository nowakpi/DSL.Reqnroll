using System;

namespace DSL.ReqnrollPlugin
{
    public interface IParameterTransformer : ITransformer
    {
        IParameterTransformer AddBespokeTransformer(in Func<string, string> transformer);
        void ClearBespokeTransformers();
    }
}
