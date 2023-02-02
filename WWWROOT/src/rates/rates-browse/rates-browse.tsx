import { useEffect, useState } from "react";
import { useLazyGetRatesQuery, useAddRateMutation } from "../rates-api";
import "./rates-browse.css";
export default () => {
  const [day, setDay] = useState<string>(
    new Date().toISOString().substring(0, 10)
  );

  const [currencyCode, setCurrencyCode] = useState<string>("");
  const [
    getRates,
    { data: dayRatesData, isLoading: isLoading, isError: isError },
  ] = useLazyGetRatesQuery();

  useEffect(() => {
    let dayMs = Date.parse(day);
    !isNaN(dayMs) && getRates(new Date(dayMs).toISOString().substring(0, 10));
  }, [day]);

  const getRowsHtml = () => {
      if (!dayRatesData) return <></>;
      
      const code = currencyCode.toLocaleUpperCase();
      return dayRatesData.rates
        .filter(
          (x) =>
            x.name.toUpperCase().indexOf(code) > -1 ||
            x.code.toUpperCase().indexOf(code) > -1
        )
        .map((d) => {
          return (
            <tr key={d.code} ng-repeat="itm in me.getRates()">
              <td className="cb-table__col_left">{d.name}</td>
              <td className="cb-table__col_centr">{d.lotsize}</td>
              <td className="cb-table__col_centr">{d.code}</td>
              <td className="cb-table__col_centr">{d.rate}</td>
            </tr>
          );
        });
  };

  if (isLoading) return <div>...</div>;

  return (
    <div className="rates-browse__body">
      <div className="rates-browse__left-side"></div>
      <div className="rates-browse__right-side">
        <div className="right-side">
          <h2>Central bank exchange rate fixing</h2>
          <p>
            Exchange rates of commonly traded currencies are declared every
            working day after 2.30 p.m. and are valid for the current working
            day and, where relevant, the following Saturday, Sunday or public
            holiday (for example, an exchange rate declared on Tuesday 23
            December is valid for Tuesday 23 December, the public holidays 24–26
            December, and Saturday 27 December and Sunday 28 December).
          </p>
        </div>
        <div className="right-side__header">
          <input
            className="right-side__input-currency"
            type="text"
            placeholder="type name or currency code"
            value={currencyCode}
            onChange={(d) => {
              setCurrencyCode(d.target.value);
            }}
          ></input>
          <input
            className="right-side_input__date"
            type="date"
            id="exampleInput"
            name="input"
            ng-model="me.date"
            onKeyDown={() => {
              return false;
            }}
            value={day}
            placeholder="yyyy-MM-dd"
            min="2000-01-01"
            max="2025-12-31"
            onChange={(p) => setDay(p.target.value)}
            required
          />
        </div>
        <table className="cb-table">
          <thead>
            <tr>
              <th className="cb-table__col_left">Currency</th>
              <th className="cb-table__col_centr">Amount</th>
              <th className="cb-table__col_centr">Code</th>
              <th className="cb-table__col_centr">Rate</th>
            </tr>
          </thead>
          <tbody>{getRowsHtml()}</tbody>
        </table>
      </div>
    </div>
  );
};
