/*!
 * Ext JS Library 3.2.1
 * Copyright(c) 2006-2010 Ext JS, Inc.
 * licensing@extjs.com
 * http://www.extjs.com/license
 */
Ext.chart.Chart.CHART_URL = 'charts.swf';
  var c = 
[{"buyer":"xiaowei","buyNum":5,"totalPrice":100185.52},
            {"buyer":"sandbox_c_2","buyNum":47,"totalPrice":19640.38},
                {"buyer":"sandbox_c_18","buyNum":11,"totalPrice":12124.4},
                    {"buyer":"sandbox_c_8","buyNum":4,"totalPrice":11588.12},
                        {"buyer":"magict","buyNum":20,"totalPrice":6898.9},
                            {"buyer":"alipublic28","buyNum":10,"totalPrice":2526.32},
                                {"buyer":"sandbox_c_17","buyNum":16,"totalPrice":2406.62},
                                    {"buyer":"tree_san","buyNum":3,"totalPrice":2191},
                                        {"buyer":"alipublic05","buyNum":3,"totalPrice":1599.06},
                                            {"buyer":"sandbox_c_27","buyNum":4,"totalPrice":1279.06},
                                                {"buyer":"sandbox_c_3","buyNum":4,"totalPrice":1271.22},
                                                    {"buyer":"alipublic04","buyNum":3,"totalPrice":1259.06},
                                                        {"buyer":"alipublic22","buyNum":3,"totalPrice":1259.06},
                                                            {"buyer":"sandbox_c_12","buyNum":3,"totalPrice":1000},
                                                                {"buyer":"alipublic14","buyNum":3,"totalPrice":1000},
                                                                    {"buyer":"alipublic27","buyNum":3,"totalPrice":867.12}
                                                                        ];
Ext.onReady(function(){

    var store = new Ext.data.JsonStore({
        fields:['buyer', 'buyNum', 'totalPrice'],
        data:c
    });
  


    
    new Ext.Panel({
        width: 600,
        height: 400,
        renderTo: 'container1',
        title: 'Stacked Bar Chart - Movie Takings by Genre',
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
        width: 600,
        height: 400,
        renderTo: 'container2',
        title: 'Stacked Bar Chart - Movie Takings by Genre',
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