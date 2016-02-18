package cmts_score.cmts_score.src.cmts_score;
import java.util.*;

import maxlike.maxlike.src.maxlike.maxlike_main;

import java.io.*;


public class cmtsscore_main {

ArrayList TESTSETPOINTER, FREQDATACASELIST;

    public cmtsscore_main(String filename,int foldint) {
    	
    	double[] temp_result = new double[foldint];
		double result = 0.0;
		for (int i = 0; i < temp_result.length; i++) {
			temp_result[i] = new cmtsscore_main("output/output" + (i + 1) + ".txt",
					"ten-fold/" + filename + "/fold_" + (i + 1) + "/" + filename + "_train" + (i + 1) + ".txt")
							.scoring();
			result += temp_result[i];
			System.out.println("fold " + (i + 1) + ": " + temp_result[i]);
		}

		System.out.println("Ten fold �������G: " + result / foldint);
    }


  public cmtsscore_main(String patternfile, String testfile) {
    FREQDATACASELIST = loadPatternToDB(patternfile);
    TESTSETPOINTER = loadDataToDB(testfile);//�Ntest set�x�s��memory��
   // scoring();
  }

//**********�N���load��memory��(�O�o�n�N���Pclass���})**********
  public ArrayList loadDataToDB(String inputfile){
    ArrayList db = new ArrayList(); // db.get(0):class label��1���Ҧ�datacase�����X(�t�@��arraylist),...�̦�����
    //�}��training dataset�ɮ�(���Pclass label��data�Q��b���P�ɮ׸�)
    FileReader file_in;
    BufferedReader buff_reader;
    StringTokenizer str_token;
    String line_data;

    try
     {
       file_in = new FileReader(inputfile);
       buff_reader = new BufferedReader(file_in);
       ArrayList subdb = new ArrayList();
       short classlabel=-1;
       while((line_data = buff_reader.readLine())!=null)
       {
         str_token = new StringTokenizer(line_data.trim());
         if (str_token.countTokens()==1){ //��token�ƥج��@,��ܦ��欰class label
             classlabel = Short.valueOf(str_token.nextToken()).shortValue();
             subdb = new ArrayList();
             db.add(subdb);
         }
         else { //��ܬ���Ʀ�,���U�i��db���ظm
           datacase tempdatacase = new datacase(classlabel);
           short tempid = Short.valueOf(str_token.nextToken()).shortValue();
             while(str_token.hasMoreTokens())
             {
               event e = new event();
               short attr = 1;
               short attrvalue = Short.valueOf(str_token.nextToken()).shortValue();
               item tempitem = new item(attr, attrvalue);
               e.addItemToEvent(tempitem);
               tempdatacase.addEventToDatacase(e);
             }
             //showDatacase(tempdatacase);
             subdb.add(tempdatacase);
         }
       }

       file_in.close();
     }
     catch(IOException e){
       System.out.println(e);
     }
    return db;
  }

  public ArrayList loadPatternToDB(String patterndir){
    ArrayList ptndb = new ArrayList(); // db.get(0):class label��1���Ҧ�datacase�����X(�t�@��arraylist),...�̦�����
    //�}��training dataset�ɮ�(���Pclass label��data�Q��b���P�ɮ׸�)
    FileReader file_in;
    BufferedReader buff_reader;
    StringTokenizer str_token;
    String line_data;

    try
     {
       file_in = new FileReader(patterndir);
       buff_reader = new BufferedReader(file_in);
       datacase tempdatacase=new datacase();
       candatacase tempcandatacase = new candatacase(tempdatacase);
       freqdatacase tempfreqdatacase = new freqdatacase(tempcandatacase);
       ArrayList subdb = new ArrayList();
       short classlabel=-1, id=1;
       while((line_data = buff_reader.readLine())!=null)
       {
         str_token = new StringTokenizer(line_data.trim());
         if (str_token.countTokens()==1){ //��token�ƥج��@,��ܦ��欰class label
           if (classlabel==-1){
             classlabel = Short.valueOf(str_token.nextToken()).shortValue();
             subdb = new ArrayList();
             ptndb.add(subdb);
           }
           else{
             tempdatacase.setClassLabel(classlabel);
          //   showFreqDatacase(tempfreqdatacase);
             subdb.add(tempfreqdatacase);
             classlabel = Short.valueOf(str_token.nextToken()).shortValue();
             subdb = new ArrayList();
             ptndb.add(subdb);
             id=1;
             tempdatacase = new datacase(classlabel);
             tempcandatacase = new candatacase(tempdatacase);
             tempfreqdatacase = new freqdatacase(tempcandatacase);
           }
         }
         else { //��ܬ���Ʀ�,���U�i��db���ظm
           short tempid = Integer.valueOf(str_token.nextToken()).shortValue();
           short time = Short.valueOf(str_token.nextToken()).shortValue(); //time���S����,�u�ΨӰϤ�����,���ȫ᪽������
           if (id == tempid){//�P�@��id��ܱ�������Ʀ��ݩ�P�@datacase,�G���ݭnnew datacase
             if (time==0){
               tempcandatacase.setCount(Integer.valueOf(str_token.nextToken()).intValue());
               tempfreqdatacase.setConfidence(Double.valueOf(str_token.nextToken()).doubleValue());
               tempcandatacase.setMinsup(Double.valueOf(str_token.nextToken()).doubleValue());
             }
             else{
               event e = new event();
               while(str_token.hasMoreTokens())
               {
                 short attr = Short.valueOf(str_token.nextToken()).shortValue();
                 short attrvalue = Short.valueOf(str_token.nextToken()).shortValue();
                 item tempitem = new item(attr, attrvalue);
                 e.addItemToEvent(tempitem);
               }
               tempdatacase.addEventToDatacase(e);
             }
           }
           else{//���Pid��ܤw�gŪ��U�@��datacase,���ɭn�N�W�@��datacase�s�����class label��subDB
             id=tempid;
             tempdatacase.setClassLabel(classlabel);
         //    showFreqDatacase(tempfreqdatacase);
             subdb.add(tempfreqdatacase);
             tempdatacase = new datacase(classlabel);
             tempcandatacase = new candatacase(tempdatacase);
             tempfreqdatacase = new freqdatacase(tempcandatacase);
             if (time==0){
               tempcandatacase.setCount(Integer.valueOf(str_token.nextToken()).intValue());
               tempfreqdatacase.setConfidence(Double.valueOf(str_token.nextToken()).doubleValue());
               tempcandatacase.setMinsup(Double.valueOf(str_token.nextToken()).doubleValue());
             }
             else{
               event e = new event();
               while(str_token.hasMoreTokens())
               {
                 short attr = Short.valueOf(str_token.nextToken()).shortValue();
                 short attrvalue = Short.valueOf(str_token.nextToken()).shortValue();
                 item tempitem = new item(attr, attrvalue);
                 e.addItemToEvent(tempitem);
               }
               tempdatacase.addEventToDatacase(e);
             }
           }
         }
       }
    //   showFreqDatacase(tempfreqdatacase);
       subdb.add(tempfreqdatacase);
       file_in.close();
     }
     catch(IOException e){
       System.out.println(e);
     }
      return ptndb;
  }

//**********���w�@��freqdatacase���Ѧ�,�N��freqdatacase�Ψ�sup,minsup�C�L�X��
  public void showFreqDatacase(freqdatacase showcase){
    datacase tmpdc = (datacase)(showcase.getDatacase());
    System.out.print("{");
    for (int i=0;i<tmpdc.getEventSize();i++){
      tmpdc.showevent(i);
    }
    System.out.print("}");
    System.out.print(" -> C" + tmpdc.getClassLabel() + ". sup/conf: " + showcase.getSup() + "/" + showcase.getConfidence() + ". minsup: " + showcase.getMinsup() + "\n");
  }

//**********Ū��Testing data�íp�⥿�T�v**********
  public double scoring(){
    int testsetcounter=0, accuracy=0;
    for(int i=0;i<TESTSETPOINTER.size();i++){
      ArrayList subtestdb = (ArrayList)TESTSETPOINTER.get(i);
      for(int j=0;j<subtestdb.size();j++){
        testsetcounter++;
        short testdclabel=((datacase)subtestdb.get(j)).getClassLabel();//���o�ثetest datacase��class label
        if (decideClassLabelByScoring((datacase)subtestdb.get(j))==testdclabel){
          accuracy++;
        }
      }
    }    
//    System.out.println( (double)accuracy/testsetcounter);
    return (double)accuracy/testsetcounter;
  }

//�M�w�@��test datacase��label�O����æ^��
  public short decideClassLabelByScoring(datacase testdc){
    //showDatacase(testdc);
    short finalclass=-1;
    double finalscore=0;
    for(int i=0;i<FREQDATACASELIST.size();i++){
      double score=0;
      int counter=0;
      ArrayList subdb = (ArrayList)FREQDATACASELIST.get(i);
      short tempclass = ((freqdatacase)subdb.get(0)).getDatacase().getClassLabel();
      for(int j=0;j<subdb.size();j++){
        freqdatacase freqdc = (freqdatacase)subdb.get(j);
        if (isContain(freqdc.getDatacase(), testdc)){
          counter++;
          score += freqdc.getConfidence()*freqdc.getSup()/freqdc.getMinsup();
        }
      }
      score = score/counter;
     // System.out.println("class: " + tempclass + ". score: " + score);
      if (score>finalscore){
        finalscore=score;
        finalclass=tempclass;
      }
    }
    //showDatacase(testdc);
    //System.out.println("predict class: " + finalclass);
    return finalclass;
  }

//**********�P�_�@��candidate datacase�O�_�X�{�b�@��datacase��**********
  public boolean isContain(datacase cdc, datacase dbc){
    int iter=0;
    for(int i=0;i<cdc.getEventSize();i++){
      iter=getIter(cdc.getEvent(i), dbc, iter);//���ocandidate����i��event�bdb datacase������m
      if(iter==dbc.getEventSize()){
        return false;
      }
      iter++;//���candidate���U�@��event�n�qdb��match���U�@��event�}�l��
    }
    return true;
  }

//***********
  public int getIter(event ce, datacase dbc, int iter){
    for(int i=iter;i<dbc.getEventSize();i++){
      if(eventIsContain(ce,dbc.getEvent(i))){
        return i;
      }
    }
    return dbc.getEventSize();
  }

//**********compare two items in two different events**********
  public boolean compareItems(item a, item b){
    if(a.getItemAttrName()==b.getItemAttrName()&&a.getItemAttrValue()==b.getItemAttrValue()){
      return true;
    }
    else{
      return false;
    }
  }

//**********�ˬd�@��event ce�O�_�]�t�bevent dbe�̭�**********
  public boolean eventIsContain(event ce, event dbe){
    for(int i=0;i<ce.getItemSize();i++){
      boolean a=false;
      for(int j=0;j<dbe.getItemSize();j++){
        if(compareItems(ce.getItem(i),dbe.getItem(j))){
          a=true;
          break;
        }
      }
      if(a==false){
        return false;
      }
    }
    return true;
  }

//**********���w�@��datacase�Ѧ�,�Ndatacase�C�L�X��
  public void showDatacase(datacase showcase){
    System.out.print("{");
    for (int i=0;i<showcase.getEventSize();i++){
      showcase.showevent(i);
    }
    System.out.print("}");
    System.out.print(" -> C" + showcase.getClassLabel() + "\n");
  }
}