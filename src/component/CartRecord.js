import React,{useState,useEffect} from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';
import '../style/Details.css';
import { ReactSession }  from 'react-client-session';
import { Button } from 'react-bootstrap';
import axios from 'axios';
 
function CartRecord(){
    /*  
        1.將目前登入的使用者的訂購紀錄顯示出來
        2.分為簡易跟詳細
        3.目前設定為只顯示近30天的紀錄
    */
    const [RecordList,setRecordList]=useState([]);
    
    useEffect(()=>{
        var User=ReactSession.get("User");

        /*將目前帳號的ID或名稱傳至API，再將其訂購紀錄調閱出來 */
        axios.post(`https://localhost:44345/api/SaveRecord/Send`,User)
            .then(res=>{
                console.log(JSON.parse(res.data));
                setRecordList(JSON.parse(res.data));
                
            }).catch(err=>{
                console.log(err);
        });
    },[])

    function ShowDetails(keyIndex){
        var PointTable=document.getElementById('details'+keyIndex);

        if(!PointTable.classList.contains('display'))
            PointTable.classList.add('display');
        else
            PointTable.classList.remove('display');
    }

    function ShowList(){
       const list=RecordList.map((item,i)=>
           <div className='sections' key={i}>
                <div className='descriptions simple'>
                    <div className='bookId'><label>{i+1}</label></div>
                    <div className='bookDate'><label className='nowrapLabel'>訂購日期:{item.date}</label></div>
                    <div className='bookTotal'><label className='nowrapLabel'>總金額:{item.total}</label></div>
                    <div className='button'>
                        <Button variant='danger' onClick={()=>ShowDetails(i)}>詳細內容</Button>
                    </div>
                </div>
                <div className='descriptions details' id={'details'+i}>
                    <ItemShow keyIndex={i}/>
                </div>
            </div>
       )


       function ItemShow(props){ //列出詳細訂購物品、數量及價格
            const Index=props.keyIndex;

            /*先去抓到指定筆的詳細資訊 */
            const Items=JSON.parse(RecordList[Index].bookItems);
            console.log(Items);

            /*接著用表格的方式一一顯示出來 且不顯示欄位 */
            /*欄位分別是:物品名稱 物品數量 該物品之總價 */
            const ItemsTable=Items.map((element,i)=>
                <tr key={i}>
                    <td>{element.itemName}</td>
                    <td>{element.itemCount}個</td>
                    <td>{element.itemTotal}元</td>
                </tr>
            )

            return(
                <table className='ItemsTable'>
                    <tbody>
                    {ItemsTable}
                    </tbody>
                </table>
            )


       }

        return(<>
            <div className='totalList'>
                {list}
            </div>
            
        </>)
    }
    



    return(<>
        <ShowList/>
        
    </>)


}

export default CartRecord;