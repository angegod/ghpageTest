import {useEffect} from 'react';
import { ReactSession }  from 'react-client-session';

function Logout(){

    useEffect(()=>{
        
        if(window.confirm("你確定要登出嗎?")){
            ReactSession.set("Login",false);
            ReactSession.set("User",undefined);
    
            window.location.replace(process.env.PUBLIC_URL+"/Login");
        }else{
            window.location.replace(process.env.PUBLIC_URL+"/Home");
        }
       
    },[])

}

export default Logout;