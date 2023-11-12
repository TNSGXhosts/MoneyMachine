using Trading.Application.DAL.Data;

namespace Trading.Application.DAL.DataAccess
{
    public class PriceDataAccess : IPriceDataAccess
    {
        private readonly DataContext _dataContext;

        public PriceDataAccess(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void TestDb()
        {
            var count = _dataContext.Prices.Count();
        }
    }
}