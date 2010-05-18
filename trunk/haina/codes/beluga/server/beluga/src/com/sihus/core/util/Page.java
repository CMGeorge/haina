package com.sihus.core.util;
/**
 * 分页设计. 
 * @author X_FU
 *
 */
public class Page {
	/*当前页数 默认为1*/
	private Integer realPage  = 1;
	/*页面记录数 默认为10*/
	private Integer pageSize  = 10;
	/*总页面数*/
	private Integer totalPage = 0;
	/*总记录数*/
	private Integer totalSize = 0;
	
	/**
	 * 默认构造.
	 */
	public Page(){
	}
	/**
	 * 
	 * @param realPage
	 * @param pageSize
	 */
	public Page(int realPage, int pageSize ){
		this.realPage = realPage;
		this.pageSize = pageSize;
	}
	/**
	 * 
	 * @return
	 */
	public Integer getPageSize() {
		return pageSize;
	}
	public void setPageSize(Integer pageSize) {
		this.pageSize = pageSize;
	}
	public Integer getRealPage() {
		return realPage;
	}
	public void setRealPage(Integer realPage) {
		this.realPage = realPage;
	}
	/**
	 * 总页面数计算.
	 * @return Integer.
	 */
	public Integer getTotalPage() {
		if(totalSize%pageSize==0){
			totalPage=totalSize/pageSize;
		}else{
			totalPage=totalSize/pageSize+1;
		}
		return totalPage;
	}
	public void setTotalPage(Integer totalPage) {
		this.totalPage = totalPage;
	}
	public Integer getTotalSize() {
		
		return totalSize;
	}
	public void setTotalSize(Integer totalSize) {
		this.totalSize = totalSize;
	}

}
