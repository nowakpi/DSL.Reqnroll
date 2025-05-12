using DSL.ReqnrollPlugin.Transformers;
using System;

namespace DSL.ReqnrollPlugin
{
    [Obsolete("IParameterTransformer will be removed in the future. Please use IUserVariableTransformer")]
    public interface IParameterTransformer : ITransformer
    {
        IParameterTransformer AddBespokeTransformer(in Func<string, string> transformer);
        void ClearBespokeTransformers();
    }
}