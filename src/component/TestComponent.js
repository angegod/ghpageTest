import React,{useState,useRef} from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';
import { Button } from 'react-bootstrap';
import '../style/Test.css';


function TestComponent(){
    
   

    function handleClick(num){
        const pointedSections=array1[num].current;
        
        //pointedSections.current.style.backgroundColor='red';
        console.log(pointedSections.className);
        
        if(pointedSections.classList.contains('red'))
            pointedSections.classList.remove('red');
        else
            pointedSections.classList.add('red');
       
    }

    const array1=[];
    
    function Sections(props){//製造一個參考節點
        const parText=props.par;
        const num=array1.length;

        const ref=useRef(); 
        array1.push(ref);
        
        return(<>
            <div className='sections' ref={ref} onClick={()=>handleClick(num)}>
                <label>{parText}</label>
            </div>
        </>)
    }

    
    return(
        <>
            <Sections par={"孫袁"}/>            
            <Sections par={"金嚳"}/>
            <Sections par={"顓行"}/>
            <Sections par={"方薰"}/>
            <Sections par={"李順"}/>
        </>

    )
}

export default TestComponent; 