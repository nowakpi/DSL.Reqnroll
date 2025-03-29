using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSL.ReqnrollPlugin
{
    public interface IFunctionTransformer : ITransformer
    {
        string[] GetSupportedFunctions();
    }
}
