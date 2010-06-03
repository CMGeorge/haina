package com.sihus.core.exception;


/**
 * The class <code>AppException</code> and its subclasses are a form of
 * application exception that indicates conditions that current system might
 * want to catch and is able to recover from this situation.
 * 
 * <P>
 * When we process exception, we can call below methods:
 * 
 * <pre>
 * <code>
 * getMessage()
 * </code>
 *  for exception ID
 * <code>
 * getErrorLevel()
 * </code>
 *  for exception severity level 
 * <code>
 * getCause()
 * </code>
 *  for root cause
 * </pre>
 * 
 * @author X_FU.
 * 
 */
public class AppException extends Exception {

    /**
     * serialVersionUID
     */
    private static final long serialVersionUID = 1L;

    protected ErrorLevel errorLevel;

    protected String excepitonId;

    public AppException() {
        super();
    }
    public AppException(String excepitonId) {
        super();
        this.excepitonId=excepitonId;
    }

    public AppException(String excepitonId,String message) {
        super(message);
        this.excepitonId=excepitonId;
    }
    

    public AppException(Throwable cause) {
        super(cause);
    }

    public AppException(String message, Throwable cause) {
        super(message, cause);
    }

    /**
     * Constructs an AppException with exception ID and error level
     * 
     * @param message
     *            The exception ID, then retrieve detailed error message based
     *            on it
     * @param errorLevel
     *            The error level used for monitoring
     */
    public AppException(String message, ErrorLevel errorLevel) {
        super(message);
        this.errorLevel = errorLevel;
    }
    /**
     * Constructs an AppException with exception ID and error level
     * 
     * @param message
     *            The exception ID, then retrieve detailed error message based
     *            on it
     * @param errorLevel
     *            The error level used for monitoring
     * @param arguments
     *            The arguments replaced the agruments of error message             
     */
    public AppException(String message, ErrorLevel errorLevel,String excepitonId) {
        super(message);
        this.errorLevel = errorLevel;
        this.excepitonId=excepitonId;
    }

    /**
     * Constructs an AppException with exception ID, root cause and error leve
     * 
     * @param message
     *            The exception ID, which is used to get detailed error message
     * @param cause
     *            The root cause (stack trace)
     * @param errorLevel
     *            The error level used for monitoring
     */
    public AppException(String message, Throwable cause, ErrorLevel errorLevel) {
        super(message, cause);
        this.errorLevel = errorLevel;
    }

    /**
     * Returns <code>ErrorLevel</code> of this exception
     * 
     * @return The error level
     */
    public ErrorLevel getErrorLevel() {
        return errorLevel;
    }

	public String getExcepitonId() {
		return excepitonId;
	}

	public void setExcepitonId(String excepitonId) {
		this.excepitonId = excepitonId;
	}

    // /**
    // * Set error level
    // * @param errorLevel The errorLevel to set for this AppException
    // */
    // public void setErrorLevel(ErrorLevel errorLevel) {
    // this.errorLevel = errorLevel;
    // }

   

}
