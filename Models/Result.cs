using System;
using System.Collections.Generic;

public class Result<T> {
    public double Value { get; }
    public string ErrorMessage { get; }
    public bool showErrorMessage {get; }

    private Result(double value) {
        Value = value; 
        showErrorMessage = false;
        ErrorMessage = String.Empty;
    }

    private Result(string errorMessage) {
        showErrorMessage = true;
        ErrorMessage = errorMessage;
    }

    public static Result<T> Success(double value) {
        return new Result<T>(value);
    }

    public static Result<T> Failure(string errorMessage) {
        return new Result<T>(errorMessage);
    }
}