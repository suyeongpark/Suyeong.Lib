using System.Collections.Generic;
using System.Linq;

namespace Suyeong.Lib.Text.Diff
{
    public static class TextDiffer
    {
        const double SIMILAR_LIMIT = 0.65d;

        public static DiffResultViews DiffTexts(IEnumerable<string> mainTexts, IEnumerable<string> subTexts, out DiffResultDic resultDicMain, out DiffResultDic resultDicSub, double similarLimit = SIMILAR_LIMIT)
        {
            resultDicMain = new DiffResultDic();
            resultDicSub = new DiffResultDic();

            Sentences mainSentences = ConvertSentences(mainTexts);
            Sentences subSentences = ConvertSentences(subTexts);

            GetDiffResultDic(mainSentences: mainSentences, subSentences: subSentences, similarLimit: similarLimit, resultDicMain: out resultDicMain, resultDicSub: out resultDicSub);

            return ConvertResultToViews(mains: resultDicMain.GetValues(), subs: resultDicSub.GetValues());
        }

        static Sentences ConvertSentences(IEnumerable<string> texts)
        {
            Sentences sentences = new Sentences();

            int index = 0;

            foreach (string text in texts)
            {
                sentences.Add(new Sentence(index: index++, text: text.ToLower()));
            }

            return sentences;
        }

        static void GetDiffResultDic(Sentences mainSentences, Sentences subSentences, double similarLimit, out DiffResultDic resultDicMain, out DiffResultDic resultDicSub)
        {
            resultDicMain = new DiffResultDic();
            resultDicSub = new DiffResultDic();

            int lastIndex = -1, intersectCount;
            string[] excepts;
            Sentence sub;
            bool find;

            foreach (Sentence main in mainSentences)
            {
                find = false;

                for (int i = lastIndex + 1; i < subSentences.Count; i++)
                {
                    sub = subSentences[i];

                    // 일단 동등한지 확인
                    if (string.Equals(main.Text, sub.Text))
                    {
                        resultDicMain.Add(main.Index, new DiffResult(index: main.Index, diffType: DiffType.Same, main: main, sub: sub));
                        resultDicSub.Add(sub.Index, new DiffResult(index: sub.Index, diffType: DiffType.Same, main: sub, sub: main));

                        find = true;
                        lastIndex = i;
                        break;
                    }
                    // 동등하지 않은 상태에서 유사도 확인
                    else
                    {
                        intersectCount = main.Texts.Intersect(sub.Texts).Count();

                        if ((double)(intersectCount * 2) / (double)(main.Texts.Length + sub.Texts.Length) > similarLimit)
                        {
                            resultDicMain.Add(main.Index, new DiffResult(index: main.Index, diffType: DiffType.Modified, main: main, sub: sub));
                            resultDicSub.Add(sub.Index, new DiffResult(index: sub.Index, diffType: DiffType.Modified, main: sub, sub: main));

                            // 필요하면 어떤 단어가 바뀌었는지를 여기서 체크
                            excepts = main.Texts.Except(sub.Texts).ToArray();

                            find = true;
                            lastIndex = i;
                            break;
                        }
                    }
                }

                if (!find)
                {
                    resultDicMain.Add(main.Index, new DiffResult(index: main.Index, diffType: DiffType.Removed, main: main, sub: new Sentence()));
                }
            }

            // 위에서 처리하고 남은 right sentence는 add로 처리한다.
            foreach (Sentence sentence in subSentences)
            {
                if (!resultDicSub.ContainsKey(sentence.Index))
                {
                    resultDicSub.Add(sentence.Index, new DiffResult(index: sentence.Index, diffType: DiffType.Added, main: sentence, sub: new Sentence()));
                }
            }

            // 순서가 꼬였으므로 순서대로 정렬한다.
            resultDicSub = new DiffResultDic(resultDicSub.OrderBy(kvp => kvp.Key));
        }

        static DiffResultViews ConvertResultToViews(DiffResults mains, DiffResults subs)
        {
            DiffResultViews views = new DiffResultViews();

            int index = 1, indexMain = 0, indexSub = 0;
            int count = mains.Count + subs.Count;
            DiffResult main, sub;

            for (int i = 0; i < count; i++)
            {
                if (indexMain < mains.Count && indexSub < subs.Count)
                {
                    main = mains[indexMain];
                    sub = subs[indexSub];

                    if (main.DiffType == DiffType.Removed)
                    {
                        views.Add(new DiffResultView(index: index++, indexMain: main.Index, indexSub: -1, typeMain: main.DiffType, typeSub: DiffType.None, textMain: main.Main.Text, textSub: string.Empty, modifiedsMain: main.Modified, modifiedsSub: new List<string>()));
                        indexMain++;
                    }
                    else if (sub.DiffType == DiffType.Added)
                    {
                        views.Add(new DiffResultView(index: index++, indexMain: -1, indexSub: sub.Index, typeMain: DiffType.None, typeSub: sub.DiffType, textMain: string.Empty, textSub: sub.Main.Text, modifiedsMain: new List<string>(), modifiedsSub: sub.Modified));
                        indexSub++;
                    }
                    else
                    {
                        views.Add(new DiffResultView(index: index++, indexMain: main.Index, indexSub: sub.Index, typeMain: main.DiffType, typeSub: sub.DiffType, textMain: main.Main.Text, textSub: sub.Main.Text, modifiedsMain: main.Modified, modifiedsSub: sub.Modified));
                        indexMain++;
                        indexSub++;
                    }
                }
                else if (indexMain < mains.Count && indexSub >= subs.Count)
                {
                    main = mains[indexMain];

                    views.Add(new DiffResultView(index: index++, indexMain: main.Index, indexSub: 0, typeMain: main.DiffType, typeSub: DiffType.None, textMain: main.Main.Text, textSub: string.Empty, modifiedsMain: main.Modified, modifiedsSub: new List<string>()));
                    indexMain++;
                }
                else if (indexMain >= mains.Count && indexSub < subs.Count)
                {
                    sub = subs[indexSub];

                    views.Add(new DiffResultView(index: index++, indexMain: 0, indexSub: sub.Index, typeMain: DiffType.None, typeSub: sub.DiffType, textMain: string.Empty, textSub: sub.Main.Text, modifiedsMain: new List<string>(), modifiedsSub: sub.Modified));
                    indexSub++;
                }
                else
                {
                    break;
                }
            }

            return views;
        }
    }
}
