<?php
require_once 'hlt.inc.php';
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

//组参函数 
function createStrParam ($paramArr) {
	$strParam = '';
	foreach ($paramArr as $key => $val) {
		if ($key != '' && $val !='') {
			$strParam .= $key.'='.urlencode($val).'&';
		}
	}
	//$strParam =str_replace("&Timestamp", "&imestamp", $strParam);
	return $strParam;
}

//解析xml函数
function getXmlData ($strXml) {
	$pos = strpos($strXml, 'xml');
	if ($pos) {
		$xmlCode=simplexml_load_string($strXml,'SimpleXMLElement', LIBXML_NOCDATA);
		$arrayCode=get_object_vars_final($xmlCode);
		return $arrayCode ;
	} else {
		return '';
	}
}

function get_object_vars_final($obj){
	if(is_object($obj)){
		$obj=get_object_vars($obj);
	}
	if(is_array($obj)){
		foreach ($obj as $key=>$value){
			$obj[$key]=get_object_vars_final($value);
		}
	}
	return $obj;
}

function getDisPlays($seller,$radeArray,$hltDatas){
	// add or from cache
	foreach ((array)$radeArray as $key => $val) {
		
		//有该买家
		if($hltDatas[$val['buyer_nick']]){
			$hltDatas[$val['buyer_nick']]->addBuyNum(1);
			$hltDatas[$val['buyer_nick']]->addTotalPrice($val['total_fee']);
			//sort
			uasort($hltDatas, "hltDataSort");
		}
		else{//没有
			if(sizeof($hltDatas) >= 100){
				if($val['total_fee'] >$hltDatas[sizeof($hltDatas)-1]->totalPrice){
					$hltData = new HltData;
					$hltData->add_Data($val['buyer_nick'],1,$val['total_fee']);
					$hltDatas[$val['buyer_nick']] = $hltData;
					//sort
					uasort($hltDatas, "hltDataSort");
					//DELETE
					array_pop($hltDatas);
				}else{
					$hltData = new HltData;
					$hltData->add_Data($val['buyer_nick'],1,$val['total_fee']);
					$hltDatas[$val['buyer_nick']] = $hltData;
					//sort
					uasort($hltDatas, "hltDataSort");
				}
			} else{
				$hltData = new HltData;
				$hltData->add_Data($val['buyer_nick'],1,$val['total_fee']);
				$hltDatas[$val['buyer_nick']] = $hltData;
				//sort	
				uasort($hltDatas, "hltDataSort");
			}
		}
	}
	return $hltDatas;
}
function hltDataSort($a, $b){
  if ($a->totalPrice == $b->totalPrice) return 0;
  	return ($a->totalPrice > $b->totalPrice) ? -1 : 1;
 }

?>