using System;

namespace DSL.ReqnrollPlugin.Transformers
{
    public interface IUserVariableTransformer : ITransformer
    {
        IUserVariableTransformer AddBespokeTransformer(in Func<string, string> transformer);
        void ClearBespokeTransformers();
    }
}
