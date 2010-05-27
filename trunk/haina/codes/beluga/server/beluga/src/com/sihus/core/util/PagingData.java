package com.sihus.core.util;


/**
 * 分页处理类
 * @author Will
 * 2009-8-27 下午09:23:17
 */
public class PagingData {
	
	/**
	 * 默认当前页
	 */
	public static final int DEFAULT_CURRENT_PAGE=1;
	
	/**
	 * 默认每页数量
	 */
	public static final int DEFAULT_ROWS_PER_PAGE=10;
	
	// 总页数
	private int pagesCount;
	
	// 总行数
	private int rowsCount;
	
	// 当前页
	private int currentPage;
	
	// 当前行
	private int currentRow;
	
	// 每页显示行数
	private int rowsPerPage;
	// 页面偏移量
	// 例如 第一页显示9，第二页显示10，此值为1。
	private int shift;


	public int getPagesCount() {
		return pagesCount;
	}

	public void setPagesCount(int pagesCount) {
		this.pagesCount = (pagesCount<1 || pagesCount>Integer.MAX_VALUE ? 1 : pagesCount);
	}

	public int getRowsCount() {
		return rowsCount;
	}

	public void setRowsCount(int rowsCount) {
		this.rowsCount = (rowsCount<1 || rowsCount>Integer.MAX_VALUE ? 1 : rowsCount);
	}

	public int getCurrentPage() {
		return currentPage;
	}

	public void setCurrentPage(int currentPage) {
		this.currentPage = (currentPage<1 || currentPage>Integer.MAX_VALUE ? DEFAULT_CURRENT_PAGE : currentPage);
	}

	public int getCurrentRow() {
		return currentRow;
	}

	public void setCurrentRow(int currentRow) {
		this.currentRow = (currentRow<1 || currentRow>Integer.MAX_VALUE ? 1 : currentRow);
	}

	public int getRowsPerPage() {
		return rowsPerPage;
	}

	public void setRowsPerPage(int rowsPerPage) {
		this.rowsPerPage = (rowsPerPage<1 || rowsPerPage>Integer.MAX_VALUE ? DEFAULT_ROWS_PER_PAGE : rowsPerPage);
	}
	/**
	 * 获取偏移量
	 * 第一页不用偏
	 * @return
	 */
	public int getShift() {
		if(currentPage == 0)
			return 0;
		return shift;
	}

	public void setShift(int shift) {
		this.shift = shift;
	}

	/**
	 * 设置最新总页数
	 */
	public void setPagesCount() {
		// fail safe
		if(this.rowsPerPage == 0) {
			this.pagesCount = 0;
			return;
		}
		
		// 计算总页数
		this.pagesCount =
			(this.rowsCount - this.rowsCount % this.rowsPerPage) / this.rowsPerPage
			+ ((this.rowsCount % this.rowsPerPage == 0) ? 0 : 1);
		
		while(this.currentPage >= this.pagesCount) {
			this.currentPage = this.currentPage - 1;
		}
	}

	/**
	 * 首页
	 */
	public void pageTop(){
		if(pagesCount>1 && rowsPerPage>0){
			currentPage = 1;
			currentRow = 0;
		}
	}

	/**
	 * 前一页
	 */
	public void pagePrevious(){
		if(pagesCount>1 && rowsPerPage>0){
			if(currentPage>1){
				currentPage--;
				currentRow = (currentPage-1) * rowsPerPage;
			}
		}
	}

	/**
	 * 	下一页
	 */
	public void pageNext(){
		if(pagesCount>1 && rowsPerPage>0){
			if(currentPage<pagesCount){
				currentRow = currentPage * rowsPerPage;
				currentPage++;
			}
		}
	}

	/**
	 * 末页
	 */
	public void pageLast(){
		if(pagesCount>1 && rowsPerPage>0){
			if(currentPage<pagesCount){
				currentRow = (pagesCount-1) * rowsPerPage;
				currentPage = pagesCount;
			}
		}
	}
}
