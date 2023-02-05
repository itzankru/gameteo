
/*  --------------------------------endpoint /api/currencyrate--------------------------*/
export interface IRate {
  name: string;
  lotsize: number;
  code: string;
  rate: number;
}

export interface TDayRates {
  rates: IRate[];
  ratedate: number;
}

/*----------------------------------------------------------------------------------------*/