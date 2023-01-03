PRAGMA foreign_keys = ON;

CREATE TABLE CURRENCY (
    Id      TEXT    PRIMARY KEY,
    LotSize NUMERIC
);

CREATE TABLE CURRENCY_RATE (
    Id         TEXT    PRIMARY KEY,
    CurrencyId TEXT    NOT NULL,
    Day        INTEGER NOT NULL,
    Year       INTEGER NOT NULL,
    Value      REAL    NOT NULL,
    FOREIGN KEY(CurrencyId) REFERENCES CURRENCY(Id)
);

