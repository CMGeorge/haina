package com.sihus.core.exception;

/**
 * Exception level enum: WARN, ERROR and FATAL
 * 
 * @author X_FU.
 *
 */
public final class ErrorLevel {

    private final String level;
    
    private ErrorLevel(String level) {
        this.level = level;
    }
    
    /**
     * Returns the description of ErrorLevel
     */
    public String toString() {
        return level;
    }
    
    public static final ErrorLevel WARN = new ErrorLevel("WARN");
    public static final ErrorLevel ERROR = new ErrorLevel("ERROR");
    public static final ErrorLevel FATAL = new ErrorLevel("FATAL");

}
