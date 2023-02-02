import { useEffect, useState } from "react";
import { useLazyGetRatesQuery, useAddRateMutation } from "../rates-api";
import { TDayRates } from "../models/rates-model";
import "./rate-line.css";
export default () => {
  const CURRENCY_OBSERV_LIST = ["EUR", "USD", "GBP"];

  const [currencyObservList, setCurrencyObservList] = useState({
    USD: { rate: 0, cnt: 1 },
    GBP: { rate: 0, cnt: 1 },
    EUR: { rate: 0, cnt: 1 },
  });

  const [getRates, { data: dayRate }] = useLazyGetRatesQuery();

  useEffect(() => {
    getRates(new Date().toISOString().substring(0, 10));
  }, []);

  useEffect(() => {
    let currencyObservList = {
      USD: { rate: 0, cnt: 1 },
      GBP: { rate: 0, cnt: 1 },
      EUR: { rate: 0, cnt: 1 },
    };
    dayRate?.rates
      .filter((i) => CURRENCY_OBSERV_LIST.includes(i.code))
      .forEach(
        (p) =>
          (currencyObservList[p.code as keyof typeof currencyObservList] = {
            rate: p.rate,
            cnt: 1,
          })
      );
    setCurrencyObservList(currencyObservList);
  }, [dayRate]);

  const calculate = (currencyCode: string) => {
    const v =
      currencyObservList[currencyCode as keyof typeof currencyObservList];
    return Math.round(v.cnt * v.rate * 100) / 100;
  };

  const setCurrencyCount = (currencyCode: string, cnt: string) => {
    !currencyCode && (currencyCode="0");

    if (!isNaN(Number(cnt)) && Number(cnt) > -1 && Number(cnt) < 100001) {
      currencyObservList[currencyCode as keyof typeof currencyObservList].cnt =
        Number(cnt);
      setCurrencyObservList({ ...currencyObservList });
    }
  };

  const getCurrencyCount = (currencyCode: string) => {
    return currencyObservList[currencyCode as keyof typeof currencyObservList]
      .cnt;
  };

  if (!dayRate) return <></>;

  return (
    <div className="reate-line__body">
      <div className="reate-line__row">
        <label>FX rates {new Date(dayRate.ratedate).toDateString()}</label>
        <div className="reate-line__frame">
          <div className="reate-line__flag reate-line__flag_eur"></div>
          <input
            type="number"
            min="1"
            max="999999"
            value={getCurrencyCount("EUR")==0?"":getCurrencyCount("EUR")}
            onChange={(p) => setCurrencyCount("EUR", p.target.value)}
          ></input>
          {"EUR-" + calculate("EUR") + " CZK"}
        </div>
        <div className="reate-line__frame">
          <div className="reate-line__flag reate-line__flag_usd"></div>
          <input
            type="number"
            min="1"
            max="999999"
            value={getCurrencyCount("USD")==0?"":getCurrencyCount("USD")}
            onChange={(p) => setCurrencyCount("USD", p.target.value)}
          ></input>
          {"USD-" + calculate("USD") + " CZK"}
        </div>
        <div className="reate-line__frame">
          <div className="reate-line__flag reate-line__flag_gbp"></div>
          <input
            type="number"
            min="1"
            max="999999"
            value={getCurrencyCount("GBP")==0?"":getCurrencyCount("GBP")}
            onChange={(p) => setCurrencyCount("GBP", p.target.value)}
          ></input>
          {"GBP-" + calculate("GBP") + " CZK"}
        </div>
      </div>
    </div>
  );
};
