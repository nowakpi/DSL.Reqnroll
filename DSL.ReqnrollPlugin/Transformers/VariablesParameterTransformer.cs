﻿using DSL.ReqnrollPlugin;
using Reqnroll;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace DSL.ReqnrollPlugin
{
    public abstract class VariablesParameterTransformer : BaseParameterTransformer, IParameterTransformer
    {
        protected readonly List<Func<string, string>> _bespokeTransformers = new List<Func<string, string>>();
        
        public void ClearBespokeTransformers() => _bespokeTransformers.Clear();

        public IParameterTransformer AddBespokeTransformer(in Func<string, string> transformer)
        {
            _bespokeTransformers.Add(transformer);
            return this;
        }

        protected string ApplyBespokeTransformers(string pattern)
        {
            foreach (var transformer in _bespokeTransformers) pattern = transformer.Invoke(pattern);
            return pattern;
        }

        protected static string TryGetValueFromScenarioContext(string pattern, Dictionary<string, object> scenarioContext)
        {
            return scenarioContext.TryGetValue(pattern, out var value)
                ? value as string
                : throw new KeyNotFoundException("[DSL.ReqnrollPlugin] Can't find key:" + pattern + " in scenario context");
        }
    }
}