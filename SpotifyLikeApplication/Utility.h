#pragma once
#include <string>
#include <algorithm>
#include <cctype>

using namespace std;

// A utility function which will convert a string to lowercase
inline string toLower(const string& str) {
    string lowerStr = str;
    transform(lowerStr.begin(), lowerStr.end(), lowerStr.begin(),
        [](unsigned char c) { return tolower(c); });
    return lowerStr;
}

// A utility function to remove whitespace from inputs
inline string trim(const string& s) {
    auto start = s.find_first_not_of(" \t\n\r\f\v");
    auto end = s.find_last_not_of(" \t\n\r\f\v");
    return (start == string::npos) ? "" : s.substr(start, end - start + 1);
}