namespace Trading.Application.TelegramConstants;

public enum States
    {
        Start,
        CreationOrder,
        CreationPosition,
        ChoosingOrder,
        ChoosingPosition,
        OrderSelected,
        PositionSelected,
        UpdatingOrder,
        UpdatingPosition,
        ClosingOrder,
        ClosingPosition,
    }