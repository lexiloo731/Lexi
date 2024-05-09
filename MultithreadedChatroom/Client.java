/*
Author: Alexia Nguyen
Date: February 20, 2024
*/

import java.net.*;
import java.io.*;

public class Client {
    private Socket socket = null;
    private DataInputStream input = null;
    private DataOutputStream out = null;

    public Client(String address, int port) {
        try {
            // Connect to server
            socket = new Socket(address, port);
            System.out.println("Connected to server.");

            // Initializes input and output streams
            input = new DataInputStream(socket.getInputStream());
            out = new DataOutputStream(socket.getOutputStream());

            BufferedReader lineReader = new BufferedReader(new InputStreamReader(System.in));
            String username = "";

            // Continue to ask for username until correct format
            while (true) {
                System.out.print("Enter your username (ex: username = ComNet): ");
                String usernameInput = lineReader.readLine();

                // Check if the username format is correct
                if (usernameInput.startsWith("username = ")) {
                    username = usernameInput.substring("username = ".length());
                    break;
                } else {
                    System.out.println("Invalid username format. Please try again.");
                }
            }

            // Send username to server for client list
            out.writeUTF(username);

            // Create thread for listening to messages from server
            Thread listenerThread = new Thread(new Receiver());
            listenerThread.start();

            // Send messages to server
            String line = "";
            while (!line.equals("Bye")) {
                line = lineReader.readLine();
                out.writeUTF(line);
            }
        } catch(IOException e) {
            System.out.println("Error: " + e.getMessage());
        } finally {
            try {
                // Close connection and streams
                if (input != null)  {
                    input.close();
                }
                if (out != null) {
                    out.close();
                }
                if (socket != null) {
                    socket.close();
                }
            } catch(IOException i) {
                System.out.println("Error: " + i.getMessage());
            }
        }
    }

    private class Receiver implements Runnable {
        @Override
        public void run() {
            try {
                String message;
                // Continuously receive messages from server until stream is closed
                while ((message = input.readUTF()) != null) {
                    System.out.println(message);
                }
            } catch(IOException e) {
                System.out.println("Error: " + e.getMessage());
            }
        }
    }

    public static void main(String[] args) {
        if (args.length != 2) {
            System.out.println("Usage: java Client <server_address> <port>");
            return;
        }

        try {
            // Get server address and port
            String serverAddress = args[0];
            int port = Integer.parseInt(args[1]);

            // Create connection
            Client client = new Client(serverAddress, port);
        } catch (NumberFormatException e) {
            System.out.println("Invalid port number: " + args[1]);
        }
    }
}
