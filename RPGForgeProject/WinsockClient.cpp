#include "WinsockClient.h"
#include "CustomException.h"
#include <iostream>
#include <winsock2.h>
#include <ws2tcpip.h>

using namespace std;

WinsockClient::WinsockClient() {
	//Winsock Client Technique:
	// Initialises the Winsock Client so that the user can eventually 
	// send the data of a weapon of their own choice that they have made.
	WSADATA wsaData;
	if (WSAStartup(MAKEWORD(2, 2), &wsaData) != 0) {
		cerr << "WSAStartup failed." << endl;
		exit(EXIT_FAILURE);
	}

	// Creates the socket which will be needed to bind to the server
	clientSocket = socket(AF_INET, SOCK_STREAM, 0);
	if (clientSocket == INVALID_SOCKET) {
		cerr << "Socket creation failed." << endl;
		WSACleanup();
		exit(EXIT_FAILURE);
	}

	// Setups the server address for the connection to work
	serverAddr.sin_family = AF_INET;
	serverAddr.sin_port = htons(SERVER_PORT);
	inet_pton(AF_INET, SERVER_IP, &serverAddr.sin_addr);
}

// De constructor for the Winsock Client
WinsockClient::~WinsockClient() {
	CloseConnection(); // Ensures proper cleanup of the connection
	WSACleanup();
}

//connects to the server
bool WinsockClient::ConnectToServer() {
	if (connect(clientSocket, (struct sockaddr*)&serverAddr, sizeof(serverAddr)) == SOCKET_ERROR) {
		cerr << "Connection to server failed." << endl;
		return false;
	}
	cout << "Connected to server." << endl;
	return true;
}

//sends the weapons damage and name over
void WinsockClient::SendWeaponData(const Weapon& weapon) {
	string weaponData = weapon.GetName() + " " + to_string(weapon.GetAttackModifier());
	send(clientSocket, weaponData.c_str(), weaponData.length(), 0);
	cout << "Sent over weapon data: " << weaponData << endl;
}

// The message that is shown when the weapon has actually been received. This is from the server
string WinsockClient::ReceiveMessage() {
	char buffer[512];
	int bytesReceived = recv(clientSocket, buffer, sizeof(buffer) - 1, 0);
	if (bytesReceived > 0) {
		buffer[bytesReceived] = '\0';
		return string(buffer);
	}
	return "";
}

// closes the connection once finished.
void WinsockClient::CloseConnection() {
	closesocket(clientSocket);
	cout << "Connection Terminated." << endl;
}