using System;
using System.Collections.Generic;

public class Result<T> {
    public T? Value { get; }
    public string ErrorMessage { get; }
    public bool showMessage {get; }

    private Result(T value) {
        Value = value; 
        showMessage = false;
        ErrorMessage = String.Empty;
    }

    private Result(string errorMessage) {
        showMessage = true;
        ErrorMessage = errorMessage;
    }

    public static Result<T> Success(T value) {
        return new Result<T>(value);
    }

    public static Result<T> Failure(string errorMessage) {
        return new Result<T>(errorMessage);
    }
}