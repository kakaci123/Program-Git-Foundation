package tfidf;

import java.io.BufferedReader;
import java.io.FileReader;
import java.io.FileWriter;
import java.io.IOException;
import java.util.ArrayList;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

public class step1 {

	public static void main(String[] args) throws IOException {

		String ids[] = { "596","689", "777"};
		FileReader fr;
		BufferedReader br;
		FileWriter fw;
		int alltotal=0,alltotalNoun=0;
		ArrayList<String> allsentence = new ArrayList<String>();
		for (String id : ids) {

			// 1.Ū��pos�ɡA�^���W���A�p��y�l�r��
			fr = new FileReader("review/" + id + "_auto_anns/gate_default");
			br = new BufferedReader(fr);
			String noun = "", ch = ""; // ch���r�šAnoun���W��
			String line = "";
			int total = 0, sent = 0, totalNoun = 0, sentNoun = 0; // �r��
			ArrayList<String> sentence = new ArrayList<String>();
			Matcher matcher;

			br.readLine(); // �h���Ĥ@�� GATE_Sentence
			while ((line = br.readLine()) != null) {
				if (line.contains("GATE_Sentence") == false) {
					matcher = Pattern.compile("lemma=\"(.*?)\"", Pattern.CASE_INSENSITIVE).matcher(line);
					matcher.find();
					ch = matcher.group(1);

					// �P�_�r�ŬO�_�����I�Ÿ���html tag
					if (Pattern.compile("<[^<]*>").matcher(ch).find()) {
						System.out.println(id + "��tag");
						System.exit(0);
					} else {
						if (!Pattern.compile("\\W").matcher(ch).find()) {
							sent++;
							if (line.contains("category=\"NN\"") || line.contains("category=\"NNP\"")
									|| line.contains("category=\"NNS\"")) {
								noun += ch.toLowerCase() + " ";
								sentNoun++;
							}
						}
					}

				} else {
					if (sent != 0) {
						noun = sent + " " + sentNoun + " " + noun;
						sentence.add(noun);
						total += sent;
						totalNoun += sentNoun;
						
						allsentence.add(noun);
						alltotal +=sent;
						alltotalNoun += sentNoun;
					}
					noun = "";
					sent = 0;
					sentNoun = 0;
				}
			}

			br.close();
			
			if (sent != 0) {
				noun = sent + " " + sentNoun + " " + noun;
				sentence.add(noun);
				total += sent;
				totalNoun += sentNoun;

				allsentence.add(noun);
				alltotal +=sent;
				alltotalNoun += sentNoun;
			}
			noun = "";
			sent = 0;
			sentNoun = 0;

			// 2.�g�J�ɮ�
			fw = new FileWriter("review/" + id + "_auto_anns/noun.txt");
			fw.write(total + " " + totalNoun + " " + "\r\n");
			for (String st : sentence) {
				fw.write(st + "\r\n");
			}

			fw.flush();
			fw.close();
			System.out.println(id + "����");
		}

		fw = new FileWriter("review/all noun.txt");
		fw.write(alltotal + " " + alltotalNoun + " " + "\r\n");
		for (String st : allsentence) {
			fw.write(st + "\r\n");
		}

		fw.flush();
		fw.close();
		System.out.println("��X�Ҧ��ɮק���");
	}
}
