一、淘宝客测试，目前最好直接在正式测试环境测试，沙箱环境有点问题。

二、商品搜索功能可以直接在测试环境测试。

二、API 2.0 签名算法.

参数数组中的签名方式选择

	'sign_method'=>'HmacMD5', //选择md5方式的时候,这行注释掉
	'sign_method'=>'md5', //选择Hmac方式的时候,这行注释掉

修改参数数组中的签名方式后,按照以下规则对util.php中的签名函数进行修改
	

1、md5签名： 
//签名函数 
function createSign ($paramArr) { 
    global $appSecret; 
    $sign = $appSecret; 
    ksort($paramArr); 
    foreach ($paramArr as $key => $val) { 
       if ($key !='' && $val !='') { 
           $sign .= $key.$val; 
       } 
    } 
//    $sign = strtoupper(md5($sign));  //Hmac方式
    $sign = strtoupper(md5($sign.$appSecret)); //Md5方式	
    return $sign; 
}

2、hmac签名： 

//签名函数 
function createSign ($paramArr) { 
    global $appSecret; 
    $sign = $appSecret; 
    ksort($paramArr); 
    foreach ($paramArr as $key => $val) { 
       if ($key !='' && $val !='') { 
           $sign .= $key.$val; 
       } 
    } 
    $sign = strtoupper(md5($sign));  //Hmac方式
//    $sign = strtoupper(md5($sign.$appSecret)); //Md5方式	
    return $sign; 
}

