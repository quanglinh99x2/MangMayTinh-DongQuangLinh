/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package BaiTapLTM;

import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileWriter;
import java.io.IOException;
import java.net.InetAddress;
import java.net.ServerSocket;
import java.net.Socket;
import java.nio.file.Files;
import java.util.ArrayList;
import java.util.Formatter;
import java.util.Scanner;
import java.util.logging.Level;
import java.util.logging.Logger;
import sun.misc.FormattedFloatingDecimal;
import sun.security.krb5.internal.HostAddress;

/**
 *
 * @author Admin
 */
public class Server {

    private int port;

    private Formatter x;

    public static ArrayList<Socket> ListSK;

    public Server(int port) {

        this.port = port;

    }

    private void excute() throws IOException {
        int i=0;
        ServerSocket server = new ServerSocket(port);
        WriteServer write = new WriteServer();
        write.start();
        System.out.println("Server is listening...");
        while (true) {
     if(i>10)
     {
         System.out.println("503 Service Unavailable");
         server.close();
     }
            Socket socket = server.accept();
            System.out.println("Đã kết nối với" + socket);
            Server.ListSK.add(socket);
            ReadServer read = new ReadServer(socket);
            read.start(); 
            i++;
        }
       
       
   
    }

    public static void main(String[] args) throws IOException {
        Server.ListSK = new ArrayList<>();
        Server sv = new Server(15797);
        sv.excute();

    }

    class ReadServer extends Thread {

        private Socket socket;

        public ReadServer(Socket socket) {
            this.socket = socket;
        }

        @Override
        public void run() {
            DataInputStream dis = null;
             InetAddress address;
            try {
                dis = new DataInputStream(socket.getInputStream());
                while (true) {
                    String sms = dis.readUTF();
                    if (sms.equalsIgnoreCase("1")) {
                        System.out.println("one");

                    } else if (sms.equalsIgnoreCase("2")) {
                        System.out.println("two");

                    } else if (sms.equalsIgnoreCase("3")) {
                        System.out.println("three");

                    } else if (sms.equalsIgnoreCase("4")) {
                        System.out.println("four");

                    } else if (sms.equalsIgnoreCase("5")) {
                        System.out.println("five");

                    } else if (sms.equalsIgnoreCase("6")) {
                        System.out.println("Six");

                    } else if (sms.equalsIgnoreCase("7")) {
                        System.out.println("Seven");

                    } else if (sms.equalsIgnoreCase("8")) {
                        System.out.println("eight");

                    } else if (sms.equalsIgnoreCase("9")) {
                        System.out.println("Nine");

                    } else if (sms.equalsIgnoreCase("end")) {
                        Server.ListSK.remove(socket);
                        System.out.println("good bye " + socket);
                        dis.close();
                        socket.close();
                        continue;
                    } else {
                        address = InetAddress.getByName(sms);
                        System.out.println("CLient: "+sms);
                        System.out.println("Ip address: "+address.getHostAddress());
                    }
                    for (Socket item : Server.ListSK) {
                        if (item.getPort() != socket.getPort()) {
                            DataOutputStream dos = new DataOutputStream(item.getOutputStream());
                            dos.writeUTF(sms);
                        }

                    }

                }
            } catch (Exception e) {
                try {

                    socket.close();
                } catch (IOException ex) {
                    System.out.println("Ngắt kết nối Server");
                }

            }
        }
    }

    class WriteServer extends Thread {

        @Override
        public void run() {
            DataOutputStream dos = null;
            Scanner sc = new Scanner(System.in);
            while (true) {
                String sms = sc.nextLine();
                try {
                    for (Socket item : Server.ListSK) {
                        dos = new DataOutputStream(item.getOutputStream());

                        dos.writeUTF(sms);
                    }
                } catch (IOException ex) {

                }

            }

        }

    }

}
