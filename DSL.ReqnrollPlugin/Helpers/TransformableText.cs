﻿namespace DSL.ReqnrollPlugin.Helpers
{
    public struct TransformableText
    {
        public byte TransformerId { get; set; }
        public string Text { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
    }
}
