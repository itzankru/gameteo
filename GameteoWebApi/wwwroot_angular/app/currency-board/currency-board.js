import './currency-board.css'

export default function($http){  
    let me=this;
    me.rates=[];
    me.currencyName=""
    me.date=new Date()
    
    me.onChangeDate=()=>{
        populateRateRs()
    }

    me.getRates=(p)=>{
        let s=me.currencyName.toUpperCase(); 
        return me.rates
                  .filter(x=>(x.name.toUpperCase(s).indexOf(s)>-1) || (x.code.toUpperCase(s).indexOf(s)>-1) );
    }

    const populateRateRs=()=>{
        console.log('/api/currencyrate/getratesbyday/?day='+me.date.toDateString("yyyyMMdd"))
        $http.get('/api/currencyrate/getratesbyday/?day='+me.date.toDateString("yyyyMMdd"))
        .then(success=>{
                    me.rates=success.data.rates;
                    me.date=new Date(success.data.ratedate);
                    
            }, error=>{
                alert(error)
            });
            
    }
    
    populateRateRs();
 }