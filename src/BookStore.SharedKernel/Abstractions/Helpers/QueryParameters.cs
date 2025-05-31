namespace BookStore.SharedKernel.Abstractions.Helpers;
public class QueryParameters
{
    private DateTime _fromDate = new(DateTime.Now.Year, DateTime.Now.Month, 1);


    public DateTime FromDate
    {
        get
        {
            var date = _fromDate;
            var result = date;

            return result;
        }
        set => _fromDate = value;
    }




    private DateTime _toDate = DateTime.Now.Date.AddMonths(1).AddDays(-1);
    public DateTime ToDate
    {
        get
        {
            var date = _toDate;
            var result = date;

            return result;
            //return _toDate.ToArabiaStandardTimeInDatetime();
        }
        set => _toDate = value;
    }



    private const int MaxPageSize = 100;
    private int _pageNumber;

    public int PageNumber
    {
        get
        {
            if (_pageNumber <= 0)
            {
                _pageNumber = 1;
            }

            return _pageNumber;
        }
        set => _pageNumber = value;
    }

    private int? _pageSize ;

    public int PageSize
    {
        get => _pageSize ?? 12;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }

    private string? _orderBy;
    public string OrderBy
    {

        get => string.IsNullOrEmpty(_orderBy) ? "" : _orderBy;


        set
        {
            if (string.IsNullOrEmpty(value))
            {
                _orderBy = "";
            }

            _orderBy = value;
        }

    }

    // it is used filed shaping (selecting specific fields);
    //public string? Fields { get; set; }


    private string? _searchTerm;

    public string SearchTerm
    {
        get => string.IsNullOrEmpty(_searchTerm) ? "" : _searchTerm.ToLower().Trim();
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                _searchTerm = "";
            }

            _searchTerm = value;
        }
    }


}
