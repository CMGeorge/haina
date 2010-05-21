package com.haina.beluga.webservice;

import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.io.PrintWriter;

import javax.servlet.ServletException;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

public class HelloServlet extends HttpServlet {
	/**
	 * 
	 */
	private static final long serialVersionUID = -5774807377339779327L;

	public void doGet(HttpServletRequest request, HttpServletResponse res)
			throws ServletException, IOException {
		testSpring();
		res.setStatus(500);
		PrintWriter out = res.getWriter();
		res.setContentType("text/html");
		out.println("<h1>UUCenter - Requires POST</h1>");
		return;
	}

	public void doPost(HttpServletRequest request, HttpServletResponse response)
			throws ServletException, IOException {
		testSpring();
		InputStream is = request.getInputStream();
		OutputStream os = response.getOutputStream();

		// ��ȡ�ӿͻ��˴���4�����
		MicroHessianInput hessianInput = new MicroHessianInput(is);
		String user = hessianInput.readString();
		String password = hessianInput.readString();

		// ��������ͻ���
		MicroHessianOutput hessianOutput = new MicroHessianOutput(os);
		if (user.compareTo("admin") == 0
				&& password.compareTo("123456789") == 0) {
			hessianOutput.writeBoolean(true);
			hessianOutput.writeString("suc");
		} else {
			hessianOutput.writeBoolean(false);
			hessianOutput.writeString("fail");
		}
	}

	public void testSpring() {
	}
}
