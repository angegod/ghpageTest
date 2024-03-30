import React,{useState} from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';
import '../style/Login.css';
import { ReactSession }  from 'react-client-session';
import { Button } from 'react-bootstrap';
import axios from 'axios';

const Login=()=>{
    const [account,setAccount]=useState();
    const [password,setPassword]=useState();

    function LoginProcess(){
        //console.log(account);
        if(account!==undefined&&password!==undefined){
            
            var sendstring={AccountName:account,Password:password};
            axios.post(`https://localhost:44345/api/Account/`,sendstring)
            .then(res=>{
                console.log(res.data);
                if(res.data==="OK"){
                    alert("Login Success");
                    ReactSession.set("Login",true);
                    ReactSession.set("User",account);
                    window.location.replace(process.env.PUBLIC_URL+'/Menu');
                }

                else
                    alert("Login Failed");
            }).catch(err=>{
                console.log(err);
            })  
        }
    }
    return(
        <>
            <div className='LoginForm'>
                <div className='account'>
                    <label>帳號:</label>
                    <input required type='text' name='account' onChange={e=>setAccount(e.target.value)} maxLength="5" pattern='[A-Za-z]{5}' title='請輸入指定格式'  />
                </div>
                <div className='password'>
                    <label>密碼:</label>
                    <input required type='password' name='password' onChange={e=>setPassword(e.target.value)} maxLength="10" pattern='[A-Za-z0-9]{10}' title='請輸入指定格式'/>
                </div>
                <div className='LoginBtn' style={{marginTop:'10px'}}>
                    <Button variant='primary'  onClick={LoginProcess}>登入</Button>
                </div>
            </div>
        </>
    )
}

export default Login;