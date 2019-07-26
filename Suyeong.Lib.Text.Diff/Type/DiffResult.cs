using System.Collections.Generic;

namespace Suyeong.Lib.Text.Diff
{
    public struct DiffResult
    {
        public DiffResult(int index, DiffType diffType, Sentence main, Sentence sub)
        {
            this.Index = index;
            this.DiffType = diffType;
            this.Main = main;
            this.Sub = sub;
        }

        public int Index { get; private set; }
        public DiffType DiffType { get; private set; }
        public Sentence Main { get; private set; }
        public Sentence Sub { get; private set; }
        public List<string> Modified
        {
            get
            {
                List<string> list = new List<string>();

                if (this.DiffType == DiffType.Modified)
                {
                    string main, sub;
                    bool isEqual;
                    int lastIndex = -1;

                    for (int i = 0; i < this.Main.Texts.Length; i++)
                    {
                        isEqual = false;
                        main = this.Main.Texts[i];

                        for (int j = lastIndex + 1; j < this.Sub.Texts.Length; j++)
                        {
                            sub = this.Sub.Texts[j];

                            if (string.Equals(main, sub))
                            {
                                isEqual = true;
                                lastIndex = i;
                                break;
                            }
                        }

                        if (!isEqual)
                        {
                            list.Add(main);
                        }
                    }
                }

                return list;
            }
        }
    }

    public class DiffResults : List<DiffResult>
    {
        public DiffResults()
        {
        }

        public DiffResults(IEnumerable<DiffResult> diffResults) : base()
        {
            this.AddRange(diffResults);
        }
    }

    public class DiffResultDic : Dictionary<int, DiffResult>
    {
        public DiffResultDic()
        {

        }

        public DiffResultDic(IEnumerable<KeyValuePair<int, DiffResult>> dic) : base()
        {
            foreach (KeyValuePair<int, DiffResult> kvp in dic)
            {
                this.Add(kvp.Key, kvp.Value);
            }
        }

        public DiffResults GetValues()
        {
            return new DiffResults(this.Values);
        }
    }
}
