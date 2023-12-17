using Stateless;

using Trading.Application.BLL.CapitalIntegrationEntities;

using Trading.Application.TelegramConstants;
using Trading.Application.UserInputPipeline;

namespace Trading.Application.UserContext;

public class Context : IUserContext
{
    private readonly StateMachine<States, Triggers> _machine = new StateMachine<States, Triggers>(States.Start);
    private string _userMessage;
    private string _userCallbackData;
    private TradeData _orderData;
    private InputPipeline _userInputPipeline;
    private PositionData _positionData;
    private WorkingOrder _workingOrder;

    public bool HasPipelineError { get; set; }

    public Context() {
        _machine.Configure(States.Start).Permit(Triggers.AddOrder, States.CreationOrder);
        _machine.Configure(States.Start).Permit(Triggers.AddPosition, States.CreationPosition);
        _machine.Configure(States.Start).Permit(Triggers.ChooseOrder, States.ChoosingOrder);
        _machine.Configure(States.Start).Permit(Triggers.ChoosePosition, States.ChoosingPosition);

        _machine.Configure(States.CreationOrder).Permit(Triggers.Start, States.Start);
        _machine.Configure(States.CreationPosition).Permit(Triggers.Start, States.Start);

        _machine.Configure(States.ChoosingOrder).Permit(Triggers.Start, States.Start);
        _machine.Configure(States.ChoosingOrder).Permit(Triggers.SelectOrder, States.OrderSelected);

        _machine.Configure(States.OrderSelected).Permit(Triggers.EditOrder, States.UpdatingOrder);
        _machine.Configure(States.OrderSelected).Permit(Triggers.Start, States.Start);
        _machine.Configure(States.OrderSelected).Permit(Triggers.CloseOrder, States.ClosingOrder);

        _machine.Configure(States.ChoosingPosition).Permit(Triggers.Start, States.Start);
        _machine.Configure(States.ChoosingPosition).Permit(Triggers.SelectPosition, States.PositionSelected);

        _machine.Configure(States.PositionSelected).Permit(Triggers.EditPosition, States.UpdatingPosition);
        _machine.Configure(States.PositionSelected).Permit(Triggers.Start, States.Start);
        _machine.Configure(States.PositionSelected).Permit(Triggers.ClosePosition, States.ClosingPosition);

        _machine.Configure(States.UpdatingPosition).Permit(Triggers.Start, States.Start);
        _machine.Configure(States.UpdatingOrder).Permit(Triggers.Start, States.Start);

        _machine.Configure(States.ClosingPosition).Permit(Triggers.Start, States.Start);
        _machine.Configure(States.ClosingOrder).Permit(Triggers.Start, States.Start);
    }

    public States State => _machine.State;

    public string InputString {
        get => _userMessage;
        set => _userMessage = value;
    }

    public string InputCallback
    {
        get => _userCallbackData;
        set => _userCallbackData = value;
    }

    public TradeData OrderData
    {
        get => _orderData;
        set => _orderData = value;
    }

    bool IUserContext.IsMessageExpected
        => _userInputPipeline != null && (_machine.State == States.UpdatingPosition || _machine.State == States.UpdatingOrder
            || _machine.State == States.CreationPosition
            || _machine.State == States.CreationOrder);

    public InputPipeline UserInputPipeline
    {
        get => _userInputPipeline;
        set => _userInputPipeline = value;
    }

    public PositionData PositionData
    {
        get => _positionData;
        set => _positionData = value;
    }

    public WorkingOrder WorkingOrder
    {
        get => _workingOrder;
        set => _workingOrder = value;
    }

    public void CatchEvent(Triggers trigger)
    {
        _machine.Fire(trigger);
    }

    public bool ExecuteUserInputPipeline(string input)
    {
        _userInputPipeline.ExecutePipeline(input);

        var hasPipelineError = HasPipelineError;

        HasPipelineError = false;
        _userInputPipeline = null;
        _userMessage = string.Empty;
        _userCallbackData = string.Empty;

        return hasPipelineError;
    }
}