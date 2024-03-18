namespace Trading.Application.BLL;

public interface ICoinsSeeker
{
    IEnumerable<string> ChooseCoinsToTrade(DateTime dateTime);
}
