﻿
 <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
 	<script src="ext-base.js"></script>
    <script src="ext-all.js"></script>
	<title>统计回头客</title>
    
    
<?php
//header("Content-Type:text/html;charset=UTF-8");

require_once 'util.php';

$appKey = '12081007';
$appSecret = '361df3380f1dadbf9c4e381ad729b365';

?>

<?php
if ($_GET['top_session']) {
	//通过授权码获取session
	//$url = 'http://container.open.taobao.com/container?authcode='.$_GET['authcode'];
	//@$sessionStr = file_get_contents($url);
	//$sessArr = explode('&', $sessionStr);
	//$sessArr1 = array();
	//$sessArr2 = array();
	//foreach ($sessArr as $key => $val) {
	//	$sessArr1 = explode('=', $val);
	//	$sessArr2[$sessArr1[0]] = $sessArr1[1];
	//}
	$sessionKey = $_GET['top_session'];
	$page_no = 1;
	$total_results = 101;
	//显示数据集合
	$hltDatas = array();
	//拉取分页数据
	for ($i = 0; $i <= $total_results/100; $i++) {
		//参数数组
		$paramArr = array(
		'timestamp' => date('Y-m-d H:i:s'), 
	    'app_key' => $appKey,
	    'method' => 'taobao.trades.sold.get',
	    'format' => 'xml',
		'session' => $sessionKey,
	    'fields' => 'tid,buyer_nick,seller_nick,status,price,num,total_fee,end_time',
		"page_size" => 100,
		"page_no"=>($i+1),
		'status' => 'TRADE_FINISHED',
		'v' => '2.0'
		);

		//生成签名
		$sign = createSign($paramArr,$appSecret);

		//组织参数
		$strParam = createStrParam($paramArr);
		$strParam .= 'sign='.$sign;

		//访问服务
		$url = 'http://gw.api.taobao.com/router/rest?'.$strParam;
		//echo $url;
		$result = @file_get_contents($url);

		$result = getXmlData($result);
		if (!$result) {
			echo '没有交易记录！';
			exit;
		}
		$total_results = $result['total_results'];
		$radeArray = $result['trades']['trade'];

		$hltDatas = getDisPlays('ss',$radeArray,$hltDatas);

	}//end for
	
	$PHP_var = json_encode(array_values($hltDatas));
	
	if (!$radeArray) {
		echo '没有交易数据！';
		exit;
	}
	?>


<h1>统计回头客</h1>
下面图表显示您店铺消费排行前100名的买家，以及此100名买家的购买次数。<br>
你可以通过图表看到您的最忠实客户。和最大买家。
<p>
<div id="container1" style="width:40%;float:left;height:1500px;border:1px dashed #990033;" >
    
</div>

<div id="container2" style="width:40%;float:left;height:1500px;border:1px dashed #990033;" >
    
</div>

 <script >

     
    /*!
     * Ext JS Library 3.2.1
     * Copyright(c) 2006-2010 Ext JS, Inc.
     * licensing@extjs.com
     * http://www.extjs.com/license
     */
    Ext.chart.Chart.CHART_URL = 'charts.swf';
    <?php echo "eval('var Java_var=".$PHP_var.";');"; ?>
      var c = Java_var;
   
    Ext.onReady(function(){

        var store = new Ext.data.JsonStore({
            fields:['buyer', 'buyNum', 'totalPrice'],
            data:c
        });
      
      
        new Ext.Panel({
            width: 450,
            height: Ext.get("container1").getHeight(),
			autoScroll:true,
			resizable:true,
            renderTo: 'container1',
            title: '您的店铺购买总额TOP100',
            items: {
                xtype: 'stackedbarchart',
                store: store,
                yField: 'buyer',
                xAxis: new Ext.chart.NumericAxis({
                    stackingEnabled: true,
                    labelRenderer: Ext.util.Format.numberRenderer('0,0')
                }),
                series: [{
                    xField: 'totalPrice',
                    displayName: '购买总额'
                }]
            }
        });
     new Ext.Panel({
            width: 450,
			height:Ext.get("container2").getHeight(),
			autoScroll:true,
			resizable:true,
            renderTo: 'container2',
            title: '总额TOP100的顾客购买次数',
            items: {
                xtype: 'stackedbarchart',
                store: store,
                yField: 'buyer',
                xAxis: new Ext.chart.NumericAxis({
                    stackingEnabled: true,
                    labelRenderer: Ext.util.Format.numberRenderer('0,0')
                }),
                series: [{
                    xField: 'buyNum',
                    displayName: '购买总次数'
                }]
            }
        });
       
    });
    
    </script>
	<?php }?>