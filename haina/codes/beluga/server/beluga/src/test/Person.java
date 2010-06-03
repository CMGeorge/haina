package test;

import java.util.List;

import net.sf.json.JSONSerializer;



public class Person {
	
	private int a;
	private boolean b;
	private String c;
	private Address addr;
	private List<Address> addrList;
	
	public int getA() {
		return a;
	}
	public void setA(int a) {
		this.a = a;
	}
	public boolean isB() {
		return b;
	}
	public void setB(boolean b) {
		this.b = b;
	}
	public String getC() {
		return c;
	}
	public void setC(String c) {
		this.c = c;
	}
	public Address getAddr() {
		return addr;
	}
	public void setAddr(Address addr) {
		this.addr = addr;
	}
	public List<Address> getAddrList() {
		return addrList;
	}
	public void setAddrList(List<Address> addrList) {
		this.addrList = addrList;
	}
	
	public static void main( String[] args){
		 JSONSerializer serializer = new JSONSerializer();
		 Person p = new Person();
		 p.setC("\"");
//		 System.out.println(serializer.deepSerialize(p));
	}

}
