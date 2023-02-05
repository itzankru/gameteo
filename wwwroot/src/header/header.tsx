import React from "react";
import "./header.css";

export default () => {
  const isCurrentPage = (pageName: string) => {
    pageName = pageName.replace("/", "");
    const url = document.URL;
    let page = url.substring(url.lastIndexOf("/") + 1);
    if (page.indexOf("?") !== -1) {
      page = page.substring(0, page.indexOf("?"));
    }
    return page === pageName;
  };
 
  return (
    <div className="header">
      <div className="header__logo"></div>

      <ul className="header__menu">
        <li>
          <a
            className={isCurrentPage("") ? "header__menu_selected" : ""}
            href="/"
          >
            Financial markets
          </a>
        </li>
        <li>
          <a
            className={isCurrentPage("/archive") ? "header__menu_selected" : ""}
            href="/archive"
          >
            News archive
          </a>
        </li>
        <li>
          <a
            className={
              isCurrentPage("/contacts") ? "header__menu_selected" : ""
            }
            href="/contacts"
          >
            Contact us{" "}
          </a>
        </li>
      </ul>
    </div>
  );
};
