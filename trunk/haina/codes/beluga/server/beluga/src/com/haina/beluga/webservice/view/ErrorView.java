package com.haina.beluga.webservice.view;

import java.lang.reflect.UndeclaredThrowableException;
import java.util.Map;

import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import com.haina.beluga.webservice.IStatusCode;
import com.haina.beluga.webservice.data.Returning;
import com.sihus.core.exception.AppException;

public class ErrorView extends JsonView{

	private static final String errorKey = "exception";
	@Override
	protected void renderMergedOutputModel(Map model, HttpServletRequest request,
			HttpServletResponse response) throws Exception{
//			Map<String,Returning> rMap = new HashMap<String,Returning>();
				Returning returning = new Returning();
				returning.setStatusCode(IStatusCode.ERROR);
				Object error = model.get(errorKey);
				
				returning.setValue(error.getClass().getSimpleName());
				
				if(error instanceof UndeclaredThrowableException){
					UndeclaredThrowableException appException = (UndeclaredThrowableException) error;
					Object app = appException.getCause();
					if(app instanceof AppException){
						returning.setStatusText(((AppException)app).getExcepitonId());
						returning.setValue(app.getClass().getSimpleName());
					}
				}
				model.put("returning", returning);
				response.setContentType( getContentType() );
				writeJSON(  model, request, response );
	}

}
