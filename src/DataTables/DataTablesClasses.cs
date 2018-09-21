using System.Collections.Generic;
using System.Text.RegularExpressions;

// ReSharper disable InconsistentNaming

namespace Utilities.DataTables
{
    // Borrowed from https://goo.gl/MYza9n

    // Properties are not capital due to json mapping
    public class DataTablesAjaxPostModel
    {
        public int draw { get; set; }
        public int start { get; set; }
        public int length { get; set; }
        public List<Column> columns { get; set; }
        public Search search { get; set; }
        public List<Order> order { get; set; }
        public CustomFilter customFilter { get; set; }
    }

    public class Column
    {
        public string data { get; set; }
        public string name { get; set; }
        public bool searchable { get; set; }
        public bool orderable { get; set; }
        public Search search { get; set; }
    }

    public class Search
    {
        public string value { get; set; }
        public string regex { get; set; }

        public string SearchAnywhereExpression => string.IsNullOrWhiteSpace(value) ? "" : $"%{value.Trim().Replace(" ", "%")}%";
        public string SearchStartExpression => string.IsNullOrWhiteSpace(value) ? "" : $"{value.Trim()}%";

        public string SearchFormattedName
        {
            get
            {
                if (string.IsNullOrEmpty(value)) return "";
                var search = value.Replace(",", "%, ") + "%";
                search = Regex.Replace(search, @"\s+", " ");
                return search;
            }
        }
    }

    public class Order
    {
        public int column { get; set; }
        public string dir { get; set; }
    }

    public class CustomFilter
    {
        public string name { get; set; }
        public object[] args { get; set; }
    }

    public class DataTablesQueryResult<T> where T : class
    {
        public int FilteredRecordCount { get; set; }
        public int TotalRecordCount { get; set; }
        public IEnumerable<T> Data { get; set; }
    }


    public class DataTablesAjaxResult
    {
        public int Draw { get; set; }
        public int RecordsTotal { get; set; }
        public int RecordsFiltered { get; set; }
        public IEnumerable<object> Data { get; set; }

        public DataTablesAjaxResult(int draw, dynamic result)
        {
            Draw = draw;
            RecordsTotal = result.TotalRecordCount;
            RecordsFiltered = result.FilteredRecordCount;
            Data = result.Data;
        }
    }
}
