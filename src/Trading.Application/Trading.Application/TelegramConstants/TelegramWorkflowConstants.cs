using Trading.Application.TelegramConstants;

namespace Trading.Application;

public static class TelegramWorkflowConstants
{
    public readonly static ICollection<States> MessageExpectedStatuses = [
        States.UpdatingPosition,
        States.UpdatingOrder,
        States.CreationPosition,
        States.CreationOrder
    ];
}
