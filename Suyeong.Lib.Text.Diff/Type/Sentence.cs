﻿using System.Collections.Generic;

namespace Suyeong.Lib.Text.Diff
{
    public struct Sentence
    {
        public Sentence(int index, string text)
        {
            this.Index = index;
            this.Text = text;
            this.Texts = text.Split(new char[] { ' ' });
        }

        public int Index { get; private set; }
        public string Text { get; private set; }
        public string[] Texts { get; private set; }
    }

    public class Sentences : List<Sentence>
    {
        public Sentences()
        {

        }

        public Sentences(IEnumerable<Sentence> sentences) : base()
        {
            this.AddRange(sentences);
        }
    }
}
