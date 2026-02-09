// WinsockServer.cpp & solution - Implementation of the Winsock server
// This server will store the weapon data which is chosen and sent to it by the player.

#include <iostream>
#include <winsock2.h>
#include <ws2tcpip.h>

#pragma comment(lib, "ws2_32.lib") // Links with Winsock library to allow for an easier time linking 
//the correct library without needing to add the dependencies yourself

constexpr auto SERVER_PORT = 42068;
constexpr auto BUFFER_SIZE = 512;

using namespace std;

int main() {
	WSADATA wsaData;
	SOCKET serverSocket, clientSocket;
	sockaddr_in serverAddr{}, clientAddr{};
	int clientAddrSize = sizeof(clientAddr);
	char buffer[BUFFER_SIZE];

	// Initialise the Winsock server
	if (WSAStartup(MAKEWORD(2, 2), &wsaData) != 0) {
		cerr << "WSAStartup failed." << endl;
		return EXIT_FAILURE;
	}

	// Creates a TCP socket
	serverSocket = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);
	if (serverSocket == INVALID_SOCKET) {
		cerr << "Socket creation failed. Error: " << WSAGetLastError() << endl;
		WSACleanup();
		return EXIT_FAILURE;
	}
	cout << "Socket created successfully!" << endl;

	// Binds the socket to the LoopBack address and port
	serverAddr.sin_family = AF_INET;
	serverAddr.sin_port = htons(SERVER_PORT);
	if (inet_pton(AF_INET, "127.0.0.1", &serverAddr.sin_addr) <= 0) {
		cerr << "Invalid address/ Address not supported" << endl;
		closesocket(serverSocket);
		WSACleanup();
		return EXIT_FAILURE;
	}

	if (bind(serverSocket, (struct sockaddr*)&serverAddr, sizeof(serverAddr)) == SOCKET_ERROR) {
		cerr << "Binding failed. Error: " << WSAGetLastError() << endl;
		closesocket(serverSocket);
		WSACleanup();
		return EXIT_FAILURE;
	}

	// Starts listening for clients to connect to
	if (listen(serverSocket, SOMAXCONN) == SOCKET_ERROR) {
		cerr << "Listening failed. Error: " << WSAGetLastError() << endl;
		closesocket(serverSocket);
		WSACleanup();
		return EXIT_FAILURE;
	}
	cout << "The Server is listening for a port " << SERVER_PORT << "..." << endl;

	// Accepts an incoming connection from a client
	clientSocket = accept(serverSocket, (struct sockaddr*)&clientAddr, &clientAddrSize);
	if (clientSocket == INVALID_SOCKET) {
		cerr << "Accept failed. Error: " << WSAGetLastError() << endl;
		closesocket(serverSocket);
		WSACleanup();
		return EXIT_FAILURE;
	}
	cout << "Client connected!" << endl;

	// Receives the weapon data from the client
	int bytesReceived = recv(clientSocket, buffer, BUFFER_SIZE - 1, 0);
	if (bytesReceived > 0) {
		buffer[bytesReceived] = '\0'; // Null-terminate the string
		cout << "Received weapon data: " << buffer << endl;

		// Echoes back the received data to confirm reception with the client
		send(clientSocket, buffer, bytesReceived, 0);
	}

	// Cleans up the client socket and the server connection
	closesocket(clientSocket);
	closesocket(serverSocket);
	WSACleanup();
	cout << "Server shut down." << endl;
	return 0;
}