import React,{useEffect, useState} from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';
import '../style/App.css';
import {Routes,Route,useLocation} from 'react-router-dom';
import Home from './Home';
import Cart from './Cart';
import Login from './Login';
import { ReactSession }  from 'react-client-session';
import Logout from './Logout';
import CartRecord from './CartRecord';


function Menu(){

    const [checkLogin,setCheckLogin]=useState(false);//檢查登入狀況
    //const [User,setUser]=useState("");
    const [isLogin,setIsLogin]=useState();
    const [isUnderlined,setUnderlined]=useState([false,false,false,false]);

    useEffect(()=>{
        ReactSession.setStoreType("localStorage");//設定Session
        if(ReactSession.get("cart")===undefined)
            ReactSession.set("cart", []);
        if(ReactSession.get("Login")===true){
            setCheckLogin(true);
            setIsLogin("您好，"+ReactSession.get("User"));
        }
        if(ReactSession.get("Login")===false){
            setIsLogin("你尚未登入，請先登入!");
            setCheckLogin(false);
            //window.location.href='http://localhost:3000/Login';
        }

        
            
    },[setCheckLogin])
   
    function AddUnderlined(){//偵測當前路徑，第三個索引值為是否渲染過，避免造成效能問題
        const nowLocation=useLocation();
        if(!isUnderlined[3]){
            if(nowLocation.pathname==='/Home'){
                setUnderlined([true,false,false,true]);
            }else if(nowLocation.pathname==='/Cart'){
                setUnderlined([false,true,false,true]);
            }else if(nowLocation.pathname==='/CartRecord'){
                setUnderlined([false,false,true,true]);
            }
            console.log(isUnderlined);
        }
        
        return <></>
    }
    
    
    return(
        <>
            <AddUnderlined/>
            <ul className='mainmenu'>
                <li className={`menuOptions ${isUnderlined[0]? 'active' :'' }`} style={{display:(!checkLogin ? 'none': 'block')}} ><a href='Home'>主頁</a></li>
                <li className={`menuOptions ${isUnderlined[1]? 'active' :'' }`} style={{display:(!checkLogin ? 'none': 'block')}} ><a href='Cart'>購物車</a></li>
                <li className={`menuOptions ${isUnderlined[2]? 'active' :'' }`} style={{display:(!checkLogin ? 'none': 'block')}} ><a href='CartRecord'>購物紀錄</a></li>
                <li className='menuOptions' style={{display:(checkLogin ? 'none': 'block')}}  ><a href='Login'>登入</a></li>
                <li className='menuOptions' style={{display:(!checkLogin ? 'none': 'block')}} ><a href='Logout'>登出</a></li>
                <li className='labelOnly'><label>{isLogin}</label></li>
            </ul>
            
            <Routes>
                <Route path='/Home'  element={<Home />}/>
                <Route path='/Cart'  element={<Cart />}/>
                <Route path='/CartRecord'  element={<CartRecord />}/>
                <Route path='/Login' element={<Login />}/>
                <Route path='/Logout' element={<Logout />}/>
            </Routes>
            
        </>
    )
}

export default Menu;