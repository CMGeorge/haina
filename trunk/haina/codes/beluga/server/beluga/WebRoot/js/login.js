/*!
 * Ext JS Library 3.2.1
 * Copyright(c) 2006-2010 Ext JS, Inc.
 * licensing@extjs.com
 * http://www.extjs.com/license
 */
Ext.onReady(function() {
    var form = new Ext.form.FormPanel({
        baseCls: 'x-plain',
        labelWidth: 35,
        url:'save-form.php',
        defaultType: 'textfield',

        items: [{
            fieldLabel: '帐户',
            name: 'loginName',
            anchor:'100%'  // anchor width by percentage
        },{
            fieldLabel: '密码',
            name: 'password',
			 inputType: 'password' ,
            anchor: '100%'  // anchor width by percentage
        }]
    });

    var window = new Ext.Window({
        title: 'Beluga',
        width: 250,
        height:140,
        minWidth: 250,
        minHeight: 140,
        layout: 'fit',
        plain:true,
        bodyStyle:'padding:5px;',
        buttonAlign:'center',
        items: form,

        buttons: [{
            text: '登录',
			handler: function(){ 
			 form.getForm().submit({  
					url:'pri.do?call=login',  
					method:'post',  
					waitMsg:"正在处理...",
					params:'',  
					success:function(form,action) {  
						Ext.MessageBox.alert('提示',"操作成功，" +action.response.responseText);  
					},  
					failure:function(form,action){  
						Ext.MessageBox.alert('提示','操作失败，' + action.response.responseText);                    
					}  
					});  
			}},
			{
				text: '注册',
				handler: function(){ 
					form.getForm().submit({  
						url:'pri.do?call=register',  
						method:'post',  
						waitMsg:"正在处理...",
						params:'mobile=15901819287',  
						success:function(form,action) {  
							Ext.MessageBox.alert('提示',"操作成功，" +action.response.responseText);  
						},  
						failure:function(form,action){  
							Ext.MessageBox.alert('提示','操作失败，' + action.response.responseText);                    
						}  
					});  
			}
        }]
    });

    window.show();
});