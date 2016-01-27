/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package pingipprogram;

import java.io.*;

public class PingIpProram {

    static private void checkStatus() throws IOException {
        Process process = Runtime.getRuntime().exec("ping 140.123.174.161 -t");
        BufferedReader reader = new BufferedReader(new InputStreamReader(process.getInputStream()));
        String line = reader.readLine();
        while ((line = reader.readLine()) != null) {
            if (!line.contains("Ping")) {
                if (line.contains("TTL")) {
                    System.out.println(line.contains("TTL"));
                } else {
                    //send mail
                    sendMail();
                    break;
                }
            }
        }
    }

    static private void sendMail() throws IOException {

        try {
            Thread.sleep(3 * 60 * 60 * 1000);
        } catch (Exception e) {
        }
        checkStatus();
    }
}
