#pragma once

#include <exception>
#include <string>

using namespace std;

class CustomException : public exception { // Defines the custom exception class that will inherit from the default exception class
private:
    string errmessage; //My Custom error message which will be held in this variable

public:
    //A  Constructor to initialise the exception message and the explicit means that the constructor will now be used in implicit conversions.
    explicit CustomException(const string& msg) : errmessage(msg) {}

    // Overrides the what() function from exception. This is needed as it allows for proper inheritance from the exception class
    //and allows for the error message to be returned with noexcept meaning that the function won't throw an exception
    const char* what() const noexcept override {
        return errmessage.c_str(); //This converts the string to a C-style string and then return it.
    }
};