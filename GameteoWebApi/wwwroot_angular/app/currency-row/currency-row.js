import './currency-row.css'

window.clearInvalidChars = function (inputElement) {
    if (inputElement.value && inputElement.value.length <= inputElement.maxLength) {
        lastValidValue = inputElement.value;
    }
    if (inputElement.value && inputElement.value.length > inputElement.maxLength) {
        inputElement.value = lastValidValue;
    }
};

export default (scope,element, attrs)=>{
    scope.date =null;
    scope.observeList=['EUR','USD','GBP']
    scope.currencyList={}
   
    scope.calculate=(p)=>{
            if(scope.currencyList[p])
                if(Number.isInteger(scope.currencyList[p].cnt)){
                    let s=scope.currencyList[p].cnt * scope.currencyList[p].rate; 
                    return Math.round(s*100)/100;
                }
           return 0;    
        }

    fetch('/api/currencyrate/getratesbyday/?day='+new Date().toDateString("yyyyMMdd"))
    .then((response) => response.json())
    .then((data) => {
        scope.date= new Date(data.ratedate);
        data.rates.filter(i=>scope.observeList.includes(i.code))
                         .forEach(p=>scope.currencyList[p.code]={...p, cnt:1,CZK:p.rate});        
    });

}
       
       
    
    