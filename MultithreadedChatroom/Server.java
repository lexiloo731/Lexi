/*
Author: Alexia Nguyen
Date: February 20, 2024
*/

import java.net.*;
import java.io.*;
import java.util.ArrayList;
import java.util.List;

public class Server {
    private ServerSocket server = null;
    private List<ClientHandler> clients = new ArrayList<>();

    public Server(int port) {
        try {
            // Start server with set port
            server = new ServerSocket(port);

            // Connect new clients to server
            while (true) {
                Socket socket = server.accept();

                // Creates client handler for new client
                ClientHandler clientHandler = new ClientHandler(socket, clients);

                // Add new client username to client list
                clients.add(clientHandler);

                // Start a new thread for this client handler
                Thread clientThread = new Thread(clientHandler);
                clientThread.start();
            }
        } catch (IOException e) {
            System.out.println("Error: " + e.getMessage());
        }
    }

    private static class ClientHandler implements Runnable {
        private Socket socket;
        private DataInputStream in;
        private DataOutputStream out;
        private List<ClientHandler> clients;
        private String username;

        public ClientHandler(Socket socket, List<ClientHandler> clients) {
            this.socket = socket;
            this.clients = clients;
            try {
                // Initializes input and output streams
                in = new DataInputStream(new BufferedInputStream(socket.getInputStream()));
                out = new DataOutputStream(socket.getOutputStream());

                // Read the username from the client
                username = in.readUTF();
            } catch (IOException e) {
                System.out.println("Error: " + e.getMessage());
            }
        }

        @Override
        public void run() {
            try {
                // Broadcast welcome message
                broadcast("Server: Welcome " + username);

                // Handle messages from the client
                while (true) {
                    String message = in.readUTF();

                    if (message.equals("AllUsers")) {
                        // Send list of connected usernames to the client who requested
                        StringBuilder userList = new StringBuilder("Server: Connected Users\n");

                        // Prints out each connected user from clients list
                        for (ClientHandler client : clients) {
                            userList.append(client.username).append("\n");
                        }
                        out.writeUTF(userList.toString());
                    } else {
                        broadcast(username + ": " + message);
                    }
                }
            } catch (IOException e) {
                // ignore
            } finally {
                try {
                    // Closes client connection
                    socket.close();
                } catch (IOException e) {
                    System.out.println("Error: " + e.getMessage());
                }
                // Remove client from connection list
                clients.remove(this);

                broadcast("Server: Goodbye " + username);
            }
        }

        private void broadcast(String message) {
            // Print message on server side
            System.out.println(message);
            // Print message to all connected clients
            for (ClientHandler client : clients) {
                try {
                    client.out.writeUTF(message);
                } catch (IOException e) {
                    System.out.println("Error: " + e.getMessage());
                }
            }
        }

    }

    public static void main(String[] args) {
        if (args.length != 1) {
            System.out.println("Usage: java Server <port>");
            return;
        }

        try {
            // Get port number from terminal input
            int port = Integer.parseInt(args[0]);

            // Create server with inputted port number
            Server server = new Server(port);
        } catch (NumberFormatException e) {
            System.out.println("Invalid port number: " + args[0]);
        }
    }
}
