using System.Collections.Generic;

namespace Suyeong.Lib.Text.Diff
{
    public struct DiffResultView
    {
        public DiffResultView(int index, int indexMain, int indexSub, DiffType typeMain, DiffType typeSub, string textMain, string textSub, List<string> modifiedsMain, List<string> modifiedsSub)
        {
            this.Index = index;
            this.IndexMain = indexMain;
            this.IndexSub = indexSub;
            this.TypeMain = typeMain;
            this.TypeSub = typeSub;
            this.TextMain = textMain;
            this.TextSub = textSub;
            this.ModifiedsMain = modifiedsMain;
            this.ModifiedsSub = modifiedsSub;
        }

        public int Index { get; private set; }
        public int IndexMain { get; private set; }
        public int IndexSub { get; private set; }
        public DiffType TypeMain { get; private set; }
        public DiffType TypeSub { get; private set; }
        public string TextMain { get; private set; }
        public string TextSub { get; private set; }
        public List<string> ModifiedsMain { get; private set; }
        public List<string> ModifiedsSub { get; private set; }
    }

    public class DiffResultViews : List<DiffResultView>
    {
        public DiffResultViews()
        {
        }

        public DiffResultViews(IEnumerable<DiffResultView> diffResultViews) : base()
        {
            this.AddRange(diffResultViews);
        }
    }
}
