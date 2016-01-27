package tfidf;

import java.io.BufferedReader;
import java.io.FileReader;
import java.io.IOException;
import java.util.ArrayList;
import java.util.HashMap;

class myClass
{
	int cnt;
	double tf;
	double idf;
	double tfidf;
	myClass()
	{
		this.cnt=-1;
		this.tf=-1;
		this.idf=-1;
		this.tfidf=-1;
	}
	myClass(int _cnt)
	{
		this.cnt=cnt;
		this.tf=-1;
		this.idf=-1;
		this.tfidf=-1;
	}
	myClass add1(){this.cnt++;return this;}
	myClass updateval(double a,double b){this.idf=a;this.tfidf=b;return this;}
}

public class step2 {

	public static void main(String[] args) throws IOException {
		FileReader fr;
		BufferedReader br;
		String line;
		String term[];
		HashMap map_calculate=new HashMap();
		ArrayList <HashMap> mylist=new 	ArrayList <HashMap> ();
		int total = 0;
		
		String ids[] = { "596","689", "777"};
		fr = new FileReader("review/all noun.txt");
		br = new BufferedReader(fr);
		
		line=br.readLine();
		total = Integer.parseInt(line.split(" ")[0]);
		
		while ((line = br.readLine()) != null) {
			term = line.split(" ");
			for (int i = 2; i < term.length; i++) {
				if(map_calculate.get(term[i])!=null)
					map_calculate.put(term[i], ((myClass)map_calculate.get(term[i])).add1());
				else
					map_calculate.put(term[i], new myClass(1));
			}
			
		}
		
		//tf
		for(Object temp:map_calculate.keySet())
		{
			myClass mytemp=(myClass)map_calculate.get(temp);
			mytemp.tf=(Math.round(((((myClass)map_calculate.get(temp)).cnt*1.0)/total)*1000)*1.0)/1000;
			map_calculate.put(temp, mytemp);
		}
		//tf end
		
 		//idf and tfidf
		int doc_cnt=ids.length;
		for(Object temp:map_calculate.keySet())
		{
			int temp_cnt=0;
			myClass mytemp=(myClass)map_calculate.get(temp);
			double temp_tf=mytemp.tf;
			
			for (String id : ids) {
	 			fr = new FileReader("review/" + id + "_auto_anns/noun.txt");
				br = new BufferedReader(fr);

				line = br.readLine();

				while ((line = br.readLine()) != null) {
					if(line.contains(temp.toString()))
					{
						temp_cnt++;
						break;
					}
				}
			}
			double temp_idf=Math.round(Math.log10(doc_cnt/temp_cnt)*1000)*1.0/1000;
			if(temp_idf<0){System.out.println("temp="+temp+"\t"+doc_cnt+"\t"+temp_cnt);}
			map_calculate.put(temp, mytemp.updateval(temp_idf,temp_tf*temp_idf));
		}
		//idf and tfidf end
    	
		//show all

		System.out.println("=========== hashmap start ============");
		for(Object temp:map_calculate.keySet())
		{

			myClass mytemp=(myClass)map_calculate.get(temp);
			System.out.println(temp+"\t"+mytemp.tf+"\t"+mytemp.idf+"\t"+mytemp.tfidf);
			
		}

		System.out.println("======================================");
	}
}
	