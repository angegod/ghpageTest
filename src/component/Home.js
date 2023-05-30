import React,{useState,useEffect} from 'react';
import axios from 'axios';
import '../style/App.css';
import Button from 'react-bootstrap/Button';
import 'bootstrap/dist/css/bootstrap.min.css';
import { ReactSession }  from 'react-client-session';


const Home=()=>{

    /* declare variable here*/
    const [records,setRecords]=useState([]);//從API抓下來的紀錄
    const [showRecords,setShowRecords]=useState([]);//實際上會顯示出來的紀錄。會做分類
    const [check,setCheck]=useState(false);//設定成一進場未抓取，未抓取的狀態下必定會去抓取，抓取完之後再設定成抓取完畢
    const [pages,setPages]=useState(1);//設定頁碼  
    const [MaxPages,setMaxPages]=useState(1);
    //const [carts,setCarts]=useState([]);//設定購物車
    

    /* 
        1.傳值方式、需求方式 GET POST
        2.地址
        3.引數 (可要可不要的東西)
    */
    

    useEffect(()=>{
        if(!check){
            axios.post(`https://localhost:44345/api/Product/`)
            .then(res=>{
                if(res.data!==""){//如果有找到資料
                    setCheck(true);
                    setPages(1);
                    setRecords(JSON.parse(res.data));
                }else{
                    setCheck(false);
                }
            }).catch(err=>{
                console.log(err);
            })
        }
        let pagesMaxNumber= Math.ceil(records.length/6);//最大頁碼
        //console.log(records.length);
        setMaxPages(pagesMaxNumber);
        var array=[];
        for(var i=1;i<=MaxPages;i++){
            array[i-1]=[];
            for(var j=0;j<=5;j++){
                var num=6*(i-1)+j;//
                //console.log(num+":"+MaxPages);
                if(records.length<(num+1))
                    break;
                else{
                    array[i-1][j]=records[num];
                }
            } 
        }
        setShowRecords(array);
        
       
    },[records,MaxPages,check])

    function Result(){//呼叫搜尋結果
        if(check){//如果有找到相關結果
            return (<><FindSearch/><PagesButton/></>);
        }
        else{//反之
            return <MissSearch/>;
        }
    }

    function MissSearch(){
        return <h2>No result Found</h2>;
    }

    function ProductDetails(props){
        const count=props.count;

        if(records[pages*6-(6-count)]!==undefined){
            const imgLink=`https://drive.google.com/uc?export=view&id=${records[pages*6-(6-count)].imgLink}`;

            return (
                <>
                    <div className='productDetails'>
                        <div className='productName'>
                            <label>{records[pages*6-(6-count)].name}</label>
                        </div>
                        <div className='productImg'>
                            <img src={imgLink} alt="not Found img"/>
                        </div>
                        <div className='productPrice'>
                            <label>{records[pages*6-(6-count)].price}元</label>
                        </div>
                        <div className='DetailsBtn'>
                            
                            <Button variant='warning' onClick={()=>addItemToCart(pages*6-(6-count))}>添加</Button>
                            <label id={"add"+count} className='add'>+1</label>
                            <Button variant='warning'>詳情</Button>
                        </div>
                    </div>
                </>
            )
        }else{
            return(<div className='productDeatils'></div>)
        }

        //https://drive.google.com/uc?export=view&id=1B4tnhkMl5kj7Swg2KrtD17wTc6JwJdEm 
    }

    function addItemToCart(id){
       
        var check=false;
        var json={itemId:(id+1),itemName:records[id].name,itemCount:1,itemPrice:records[id].price,itemTotal:records[id].price};
        //console.log(json);
        var carts=ReactSession.get("cart");
        console.log(carts);
       
        carts.forEach(item => {
            if(item.itemId===(id+1)&&!check){
                check=true;
                item.itemCount++;
                item.itemTotal+=item.itemPrice;
            }
                
        });

        if(!check)
            carts=[...carts,json];
        ReactSession.set("cart",carts);
        
        var addLabel=document.getElementById('add'+id);
        addLabel.classList.add('active');
        setInterval(()=>{
            addLabel.classList.remove('active');
        },1000);
        
            
    }
    
    function FindSearch(){
        //透過目前所在頁碼數，來顯示資料
        if(showRecords[pages-1]===undefined){
            showRecords[pages-1]=[];
        }
        const lists=(
            <>
                <div className='productMain'>
                    <div className='row'>
                        <ProductDetails count={0}/>
                        <ProductDetails count={1}/>
                        <ProductDetails count={2}/>
                        <ProductDetails count={3}/>
                        <ProductDetails count={4}/>
                        <ProductDetails count={5}/>
                    </div>
                </div>
                
            </>
        )
        

        return(
            <>
                {lists}
            </>
        );
       
    }


    
    function PagesButton(){
        return(<div className='buttons'>
                <Button onClick={()=>pagesCount(-1)} className='btn btn-warning'>上一頁</Button>
                <label>{pages}</label>
                <Button onClick={()=>pagesCount(1)} className='btn btn-warning'>下一頁</Button>
                </div>
        );
    }

    function pagesCount(num){
        let pagesMaxNumber=MaxPages;
        
        if((pages===1&&num===1)||(pages===pagesMaxNumber&&num===-1))
            setPages(count=>count+=num);
        else if(pages>1&&pages<pagesMaxNumber)
            setPages(count=>count+=num)
            
    }
   
    return(
        <div className='ProductMain'>
            <h1>商品專區</h1>
            <div className='result'>{<Result/>}</div>
        </div>
    )
}

export default Home;