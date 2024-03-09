using Stateless;

using Trading.Application.TelegramConstants;

namespace Trading.Application.UserContext;

public class StateProcessor : IStateProcessor
{
    private readonly StateMachine<States, Triggers> _machine = new StateMachine<States, Triggers>(States.Start);

    public States State => _machine.State;

    public StateProcessor()
    {
        _machine.Configure(States.Start).Permit(Triggers.AddOrder, States.CreationOrder);
        _machine.Configure(States.Start).Permit(Triggers.AddPosition, States.CreationPosition);
        _machine.Configure(States.Start).Permit(Triggers.ChooseOrder, States.ChoosingOrder);
        _machine.Configure(States.Start).Permit(Triggers.ChoosePosition, States.ChoosingPosition);
        _machine.Configure(States.Start).Permit(Triggers.TestStrategy, States.TestingStrategy);

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

        _machine.Configure(States.TestingStrategy).Permit(Triggers.Start, States.Start);
    }

    public bool IsMessageExpectedState => TelegramWorkflowConstants.MessageExpectedStatuses.Contains(State);

    public void CatchEvent(Triggers trigger)
    {
        _machine.Fire(trigger);
    }
}