package com.haina.beluga.webservice.json;

import java.util.HashMap;
import java.util.Map;

public class Test {

    public static void main(String[] args) throws JSONException {
	String json = "{\"name\":\"reiz\"}";
	JSONObject jsonObj = new JSONObject(json);
	String name = jsonObj.getString("name");

	jsonObj.put("initial", name.substring(0, 1).toUpperCase());

	String[] likes = new String[] { "JavaScript", "Skiing", "Apple Pie" };
	jsonObj.put("likes", likes);

	Map<String, String> ingredients = new HashMap<String, String>();
	ingredients.put("apples", "3kg");
	ingredients.put("sugar", "1kg");
	ingredients.put("pastry", "2.4kg");
	ingredients.put("bestEaten", "outdoors");
	jsonObj.put("ingredients", ingredients);
	System.out.println(jsonObj);

	System.out.println(jsonObj);
	String chatS = "{\"sender\":\"sender\",\"playerId\":0,\"recver\":null,\"type\":1,\"channel\":\"2\",\"contant\":\"contant\"}";
//	final ChatDto chat = new ChatDto();
//	chat.setContant("contant");
//	chat.setSender("sender");
//	chat.setChannel("2");
	JSONObject object = new JSONObject();
	System.out.println(object);
//	System.out.println((ChatDto)JSONObject.serializer(chatS,ChatDto.class));
	}

}
