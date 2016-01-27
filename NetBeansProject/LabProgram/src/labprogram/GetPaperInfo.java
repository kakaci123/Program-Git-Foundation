package labprogram;

import java.io.*;

public class GetPaperInfo {

    public static void main(String[] args) throws Exception {
        BufferedReader br = new BufferedReader(new FileReader("D:/My Collection.bib"));
        String journal = "", title = "", year = "", str = "";
        StringBuffer sb = new StringBuffer();
        sb.append("Journal").append("@").append("Title").append("@").append("Year").append("@").append("FormatStringOutput").append("\n");
        
        while ((str = br.readLine()) != null) {
            if (str.contains("journal = {")) {
                journal = str.replace("journal = {", "").replace("},", "");
            } else if (str.contains("title = {{")) {
                title = str.replace("title = {{", "").replace("}},", "");
            } else if (str.contains("year = {")) {
                year = str.replace("year = {", "").replace("}", "");
                String[] temp = title.split(" ");
                sb.append(journal).append("@").append(title).append("@").append(year).append("@").append(year + "_" + temp[0] + " " + temp[1] + " " + temp[2] + " " + temp[3]).append("\n");
//                sb.append(journal).append("@").append(title).append("@").append(year).append("\n");
            }
        }
        System.out.println(sb.toString());
        br.close();
    }
}