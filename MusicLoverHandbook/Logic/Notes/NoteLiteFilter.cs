﻿using MusicLoverHandbook.Models.NoteAlter;
using System.Data;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace MusicLoverHandbook.Logic.Notes
{
    public class NoteLiteFilter
    {
        #region Public Properties

        public string[] DescriptionCompareStrings { get; }

        public string[] NameCompareStrings { get; }

        #endregion Public Properties

        #region Public Constructors + Destructors

        public NoteLiteFilter(string byName, string byDesc)
        {
            var andSplitting = (string by) =>
                by != "" ? by.Split(";").Select(x => x.ToLower().Trim()).ToArray() : new[] { "" };
            NameCompareStrings = andSplitting(byName);
            DescriptionCompareStrings = andSplitting(byDesc);
        }

        #endregion Public Constructors + Destructors

        #region Public Methods

        public List<LiteNote> ApplyOn(List<LiteNote> lites)
        {
            var filtered = lites.ToList();
            filtered = MultiComparisonApplicator((NameCompareStrings, NameFiltering, filtered));
            filtered = MultiComparisonApplicator(
                (DescriptionCompareStrings, DescFiltering, filtered)
            );

            return filtered.Distinct().ToList();
        }

        #endregion Public Methods

        #region Private Methods

        private List<LiteNote> DescFiltering(List<LiteNote> inputLites, string rawcomp)
        {
            var lites = inputLites.ToList();

            if (rawcomp == "")
                return lites;
            bool exceptionMode = false;
            var comparer = rawcomp;

            if (rawcomp[0] == '!')
            {
                exceptionMode = true;
                comparer = rawcomp.Substring(1);
            }

            if (comparer == "")
                return lites;

            List<LiteNote> result = new();
            do
            {
                if (comparer[0] != '#')
                {
                    result = (
                        from lite in lites
                        where lite.NoteDescription.ToLower().Trim().Contains(comparer)
                        select lite
                    ).ToList();
                }
                else
                {
                    var data = (
                        from lite in lites
                        let rawtagdata = StringTagTools.GetTagged(lite.NoteDescription, '#')
                        let taggedinfo = (
                            from tagdata in rawtagdata
                            select (Name: tagdata.Key, tagdata.Value, NoteLite: lite)
                        )
                        from taggedcluster in taggedinfo
                        group taggedcluster by taggedcluster.Name.GetHashCode() into tagged_groupped
                        select (from tg in tagged_groupped group tg by tg.Value)
                    ).ToDictionary(
                        key => key.First().First().Name,
                        value =>
                            value.ToDictionary(
                                key => key.First().Value,
                                value => (from tagdata in value select tagdata.NoteLite).ToArray()
                            )
                    );
                    if (data.Count == 0)
                    {
                        break;
                    }
                    var tagname = Regex.Match(comparer, @"[^\s]+").Value;
                    var value = string.Join(tagname, comparer.Split(tagname).Skip(1))
                        .Trim()
                        .ToLower();
                    tagname = tagname.ToLower().Trim();

                    var queryTagNames = data.Select(x => x);
                    if (tagname != "#")
                        queryTagNames =
                            from pair in queryTagNames
                            where
                                pair.Key.ValueType == StringTagTools.TagDataType.Valued
                                && pair.Key.Value!
                                    .ToLower()
                                    .Trim()
                                    .Contains(tagname.Substring(1, tagname.Length - 1))
                            select pair;

                    if (queryTagNames.Count() == 0)
                    {
                        break;
                    }

                    var queryTagValues = queryTagNames.SelectMany(x => x.Value);
                    if (value != "")
                    {
                        var splittedValue = value.Split(' ');
                        foreach (var subvalue in splittedValue)
                            queryTagValues =
                                from tagvalue in queryTagValues
                                where
                                    tagvalue.Key.ValueType == StringTagTools.TagDataType.Valued
                                    && tagvalue.Key.Value!.ToLower().Trim().Contains(subvalue)
                                select tagvalue;
                    }
                    result = queryTagValues.SelectMany(x => x.Value).ToList();
                }
            } while (false);
            return exceptionMode ? lites.Except(result).ToList() : result;
        }

        private List<LiteNote> MultiComparisonApplicator(
            (string[] CompStrings, Func<
                List<LiteNote>,
                string,
                List<LiteNote>
            > FilterFunc, List<LiteNote> ApplyOn) applicationInfo
        )
        {
            var filtered = applicationInfo.ApplyOn;
            foreach (var compStr in applicationInfo.CompStrings)
            {
                var descPartOrCheck = compStr.Replace(@"\|", "\n");
                if (descPartOrCheck.Count(x => x == '|') == 0)
                    filtered = applicationInfo.FilterFunc(filtered, compStr.Replace(@"\|", "|"));
                else
                {
                    var forOr = descPartOrCheck
                        .Split('|')
                        .Select(x => x.Replace("\n", @"|").Trim());
                    Debug.WriteLine(String.Join(',', forOr));

                    var combFiltered = forOr
                        .Select(x => applicationInfo.FilterFunc(filtered, x))
                        .Aggregate((c, n) => c.Concat(n).ToList());
                    filtered = combFiltered;
                }
            }
            return filtered;
        }

        private List<LiteNote> NameFiltering(List<LiteNote> lites, string nameCompare)
        {
            if (nameCompare == "")
                return lites;
            var name = nameCompare;
            bool exceptionMode = false;
            if (nameCompare[0] == '!')
            {
                name = nameCompare.Substring(1);
                exceptionMode = true;
            }
            if (name == "")
                return lites;

            var result = new List<LiteNote>();
            result = lites.Where(x => x.NoteName.ToLower().Trim().Contains(name)).ToList();
            return exceptionMode ? lites.Except(result).ToList() : result;
        }

        #endregion Private Methods
    }
}
