namespace DSL.ReqnrollPlugin
{
    public interface IEnvironmentVariableTransformer : ITransformer
    {
        string GetEnvironmentVariable(string key);
    }
}