/*
Author: Alexia Nguyen
Date: March 26, 2024
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
                ClientHandler clientHandler = new ClientHandler(socket);
                clients.add(clientHandler);

                // Start a new thread for this client handler
                Thread clientThread = new Thread(clientHandler);
                clientThread.start();
            }
        } catch (IOException e) {
            System.out.println("Error: " + e.getMessage());
        }
    }

    private class ClientHandler implements Runnable {
        private Socket socket;
        private DataInputStream in;
        private DataOutputStream out;
        private String username;
        private String choice;

        public ClientHandler(Socket socket) {
            this.socket = socket;
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

                // Check if both clients have connected
                if (clients.size() == 2) {
                    broadcast("Both players have connected.");
                }

                // Handle messages from the client
                while (true) {
                    choice = in.readUTF();
                    // Process the choice
                    gameHandler(choice);
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

                // Broadcast goodbye message
                broadcast("Server: Goodbye " + username);
            }
        }

        private void gameHandler(String choice) {
            // Determine the opponent
            ClientHandler opponent = null;
            // Loops through clients and chooses first client that is not current client
            for (ClientHandler client : clients) {
                if (client != this) {
                    opponent = client;
                    break;
                }
            }

            // Returns early if there is no opponent connected
            if (opponent == null) {
                return;
            }

            // Retrieve opponent choice
            String opponentChoice = opponent.choice;

            // Check if opponent has made a choice
            if (opponentChoice == null) {
                return;
            }

            // Check if choice is valid
            if (!choice.equals("rock") && !choice.equals("paper") && !choice.equals("scissors")) {
                String invalidMessage = "Invalid choice";
                try {
                    out.writeUTF(invalidMessage);
                } catch (IOException e) {
                    System.out.println("Error sending message to client: " + e.getMessage());
                }
                return;
            }

            // Determine the winner
            String result;
            // If players have same choice then it's a tie
            if (choice.equals(opponentChoice)) {
                result = "It's a Tie!";
            // The last client input is choice while the other client is opponent
            // If any of this critertia is met, then client wins
            } else if ((choice.equals("rock") && opponentChoice.equals("scissors")) ||
                    (choice.equals("paper") && opponentChoice.equals("rock")) ||
                    (choice.equals("scissors") && opponentChoice.equals("paper"))) {
                result = username + " wins!";
           // If not, opponent wins
            } else {
                result = opponent.username + " wins!";
            }

            // Broadcast the result
            broadcast(username + " chose " + choice + ". " + opponent.username + " chose " + opponentChoice + ". " + result);

            // Reset the choices for the next game cycle
            this.choice = null;
            opponent.choice = null;
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
