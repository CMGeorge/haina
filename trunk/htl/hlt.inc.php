<?php

// 回头率的数据结构

class Hltdata {

	var $buyer; // 买家
	var $buyNum; // 次数
	var $totalPrice; // 购买总额

	// 加入数据

	function add_Data ($buyer, $buyNum,$totalPrice) {

		$this->buyer = $buyer;
		$this->buyNum = $buyNum;
		$this->totalPrice = $totalPrice;

	}

	// 增加购买次数

	function addBuyNum ($num) {

		$this->buyNum += $num;

	}
	// 增加购买总额

	function addTotalPrice ($totalPrice) {
		$this->totalPrice += $totalPrice;

	}

}

?>