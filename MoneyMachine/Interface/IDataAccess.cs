﻿using System;
using MoneyMachine.Entities;
using MoneyMachine.Enums;

namespace MoneyMachine.Interface
{
	public interface IDataAccess
	{
        PricesEntity GetHistoricalPrices(string epic, Resolution? resolution = null, int? max = null, DateTime? from = null, DateTime? to = null);
        List<OpenPosition> GetAllPositions();
        string ClosePosition(string dealId);
        string CreatePosition(PositionCreateEntity positionSetup);
        double GetBalance();
    }
}

