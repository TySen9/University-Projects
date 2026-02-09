#pragma once

#include <iostream>
#include <string>
#include <winsock2.h>
#include "Weapon.h"

#pragma comment(lib, "ws2_32.lib") // Links with Winsock library to allow for an easier time linking 
//the correct library without needing to add the dependencies yourself

// constexpr is a way to declare a constant at compile time and 
// is another way of doing const but can also be used with it.
constexpr auto SERVER_PORT = 42068; 
constexpr auto SERVER_IP = "127.0.0.1";

class WinsockClient {
private:
	SOCKET clientSocket;
	sockaddr_in serverAddr;

public:
	WinsockClient();
	~WinsockClient();

	bool ConnectToServer();
	void SendWeaponData(const Weapon& weapon);
	string ReceiveMessage();
	void CloseConnection();
};