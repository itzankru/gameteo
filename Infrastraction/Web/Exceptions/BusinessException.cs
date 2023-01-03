
using System.Text.Json;

namespace ItZnak.Infrastruction.Web.Exceptions
{
/*======================================================================================================================================================= 
    Класс:BusinessException
    Назначение:ошибка генерируемая при наршении бизнес логики программы
    Сценарий использования:
        1. IWebApiControllerHandler обрабатывает запрос и сталкивается с ошибкой бизнес логики.
        2. IWebApiControllerHandler генерирует ошибку. Сообщение о ошибке упаковывается в коллекуии ключ-значение
        3. Глобальный обработчик ошибок GlobalExceptionMdl перехватывает ошибку и формирует сообщение клиентской 
        части с кодом ответа 422.
        4. Клиентская корректно обрабатывает сообщение с кодом 422 

    Пример использования.
    Ошибка в заполнении обязательного поля адрес
    _errList={address:'неверно заполнен адрес'} 
======================================================================================================================================================= */

    public class BusinessException : Exception
    {
        private readonly Dictionary<string,string> _errList=new();
        public BusinessException(Dictionary<string, string> errList) => _errList = errList;

        public BusinessException() : base()
        {
        }

        public BusinessException(string message) : base(message)
        {
        }

        public BusinessException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public string ToJson(){
          return  JsonSerializer.Serialize(_errList);
        }
    }
}