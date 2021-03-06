package cmts.src.cmts;

import java.util.*;
import java.io.*;

public class CMTSMAIN {

	public static double MIN = 0.02; // 輸入MIN值
	public static double SIGMA = 0.2; // 輸入Sigma值

	String TRAINING, TRAININGOUTPUT;
	ArrayList DBPOINTER, CURRENTL1SET;
	ArrayList FREQDATACASELIST = new ArrayList();// 儲存全部freq
													// datacase,FREQDATACASE.get(0)為label=1的所有freq
													// datacase

	public CMTSMAIN(String filename, int foldint) {
		for (int i = 0; i < foldint; i++) {
			new CMTSMAIN("ten-fold/" + filename + "/fold_" + (i + 1) + "/" + filename + "_test" + (i + 1) + ".txt",
					"output/output" + (i + 1) + ".txt");

			System.out.println("******folder*****");
			try{
				
			Thread.sleep(5000);}catch(Exception e){}

		}
	}

	public CMTSMAIN(String traindir, String trainoutput) {
		TRAINING = traindir;
		TRAININGOUTPUT = trainoutput;
		DBPOINTER = loadDataToDB(TRAINING);

		for (int i = 0; i < DBPOINTER.size(); i++) { // 一次只將同一class的data放進演算法運算
			System.out.println("********************start mining rules for CLASS" + (i + 1) + "********************");
			ArrayList oneclassdb = (ArrayList) DBPOINTER.get(i);
			cmtssp((ArrayList) (DBPOINTER.get(i)));
		}
		stage3();
		outputTraining(TRAININGOUTPUT, FREQDATACASELIST);// 將所產生的所有freqent
															// pattern紀錄在檔案中
	}

	// **********找出所有frequent pattern前半部的support (必須再scan DB一次)********
	public void stage3() {
		for (int i = 0; i < FREQDATACASELIST.size(); i++) {
			ArrayList subdb = (ArrayList) FREQDATACASELIST.get(i);
			for (int j = 0; j < subdb.size(); j++) {
				ArrayList subdbleng = (ArrayList) subdb.get(j);
				for (int k = 0; k < subdbleng.size(); k++) {
					freqdatacase fdc = (freqdatacase) subdbleng.get(k);
					datacase dc = fdc.getDatacase();
					int globalcount = getGlobalCount(dc);
					fdc.setConfidence((double) fdc.getSup() / globalcount);
				}
			}
		}
	}

	public int getGlobalCount(datacase dc) {
		int counter = 0;
		for (int i = 0; i < DBPOINTER.size(); i++) {
			ArrayList oneclassdb = (ArrayList) DBPOINTER.get(i);
			for (int j = 0; j < oneclassdb.size(); j++) {
				datacase dbdc = (datacase) oneclassdb.get(j);
				if (isContain(dc, dbdc)) {
					counter++;
				}
			}
		}
		return counter;
	}

	// **********利用L1產生C2**********
	public ArrayList candGenC2(ArrayList L1list, ArrayList C2list) {
		for (int i = 0; i < L1list.size(); i++) {
			freqdatacase fda = (freqdatacase) L1list.get(i);
			datacase da = fda.getDatacase();
			event ea = da.getEvent(0);
			item ia = ea.getItem(0);
			double minsupa = fda.getMinsup();
			double minsup;

			for (int j = i; j < L1list.size(); j++) {
				freqdatacase fdb = (freqdatacase) L1list.get(j);
				datacase db = fdb.getDatacase();
				event eb = db.getEvent(0);
				item ib = eb.getItem(0);
				double minsupb = fdb.getMinsup();

				if (minsupa <= minsupb) {
					minsup = minsupa;
				} else {
					minsup = minsupb;
				}
				// 產生candidate

				if (ia.getItemAttrValue() != ib.getItemAttrValue()) {
					datacase newdc1 = new datacase(da.getClassLabel());
					newdc1.addEventToDatacase(ea);
					newdc1.addEventToDatacase(eb);
					candatacase newcandc1 = new candatacase(newdc1);
					newcandc1.setMinsup(minsup);
					C2list.add(newcandc1);
					// showCandatacase(newcandc1);

					datacase newdc2 = new datacase(da.getClassLabel());
					newdc2.addEventToDatacase(eb);
					newdc2.addEventToDatacase(ea);
					candatacase newcandc2 = new candatacase(newdc2);
					newcandc2.setMinsup(minsup);
					C2list.add(newcandc2);
					// showCandatacase(newcandc2);
				}
			}
		}
		candPrun(C2list);
		return C2list;
	}

	// **********利用L(k-1)產生C(k)**********
	public ArrayList candGenCk(ArrayList Lklist, ArrayList Cklist) {// Lklist是存L(k-1)
		for (int i = 0; i < Lklist.size(); i++) {
			freqdatacase p = (freqdatacase) Lklist.get(i);
			datacase pd = p.getDatacase();
			for (int j = i; j < Lklist.size(); j++) {
				freqdatacase q = (freqdatacase) Lklist.get(j);
				datacase qd = q.getDatacase();
				// p與q的MIN_Item相同才繼續比
				if (p.getMinsup() == q.getMinsup()) {// p與q的minsup相同,才有可能會有Min_item
					// 看minItem在那
					int cksize = Cklist.size();
					joinType(pd, qd, Cklist, p.getMinsup());
				}
			}
		}
		candPrun(Cklist);
		return Cklist;
	}

	// **********將重複產生的candidate prun掉(datacase完全相同)**********
	public void candPrun(ArrayList clist) {
		ArrayList removelist = new ArrayList();
		for (int i = 0; i < clist.size(); i++) {
			for (int j = i + 1; j < clist.size(); j++) {
				datacase da = ((candatacase) clist.get(i)).getDatacase();
				datacase db = ((candatacase) clist.get(j)).getDatacase();

				if (compareDatacases(da, db)) {
					removelist.add((candatacase) clist.get(j));
				}
			}
		}
		for (int i = 0; i < removelist.size(); i++) {
			clist.remove((candatacase) removelist.get(i));
		}
	}

	// **********cand join type **********
	public void joinType(datacase p, datacase q, ArrayList Cklist, double minsup) {
		// System.out.println("join type :");
		datacase cp = p.cloneDatacase();
		datacase cq = q.cloneDatacase();
		item pi, qi;
		// 將最後一個event的最後一個item移除,如果最後一個event只有一個item,則移除最後一個event
		if (cp.getEvent(cp.getEventSize() - 1).getItemSize() == 1) {
			pi = cp.getEvent(cp.getEventSize() - 1).getItem(0);
			cp.removeEvent(cp.getEventSize() - 1);
		} else {
			pi = cp.getEvent(cp.getEventSize() - 1).getItem(cp.getEvent(cp.getEventSize() - 1).getItemSize() - 1);
			cp.getEvent(cp.getEventSize() - 1).removeItem(cp.getEvent(cp.getEventSize() - 1).getItemSize() - 1);
		}
		if (cq.getEvent(cq.getEventSize() - 1).getItemSize() == 1) {
			qi = cq.getEvent(cq.getEventSize() - 1).getItem(0);
			cq.removeEvent(cq.getEventSize() - 1);
		} else {
			qi = cq.getEvent(cq.getEventSize() - 1).getItem(cq.getEvent(cq.getEventSize() - 1).getItemSize() - 1);
			cq.getEvent(cq.getEventSize() - 1).removeItem(cq.getEvent(cq.getEventSize() - 1).getItemSize() - 1);
		}
		// 移除最後一個item後,p與q必須要完全相同才能join(&&cp與cq中必須含有相同的MIN_Item)
		if (compareDatacases(cp, cq) && checkMinItem(cp, cq, minsup)) {
			if (p.getEvent(p.getEventSize() - 1).getItemSize() == 1
					&& q.getEvent(q.getEventSize() - 1).getItemSize() == 1) {
				if (compareItems(p.getEvent(p.getEventSize() - 1).getItem(0),
						q.getEvent(q.getEventSize() - 1).getItem(0))) {
					datacase newdc1 = p.cloneDatacase();
					newdc1.addEventToDatacase(q.getEvent(q.getEventSize() - 1));
					candatacase newcandc1 = new candatacase(newdc1);
					newcandc1.setMinsup(minsup);
					Cklist.add(newcandc1);
					// showCandatacase(newcandc1);
				} else {
					datacase newdc1 = p.cloneDatacase();
					newdc1.addEventToDatacase(q.getEvent(q.getEventSize() - 1));
					candatacase newcandc1 = new candatacase(newdc1);
					newcandc1.setMinsup(minsup);
					Cklist.add(newcandc1);
					// showCandatacase(newcandc1);
					datacase newdc2 = q.cloneDatacase();
					newdc2.addEventToDatacase(p.getEvent(p.getEventSize() - 1));
					candatacase newcandc2 = new candatacase(newdc2);
					newcandc2.setMinsup(minsup);
					Cklist.add(newcandc2);
					// showCandatacase(newcandc2);
				}

			} else if (p.getEvent(p.getEventSize() - 1).getItemSize() == 1
					&& q.getEvent(q.getEventSize() - 1).getItemSize() > 1) {// p的第一個event只有一item,q超過一個
				datacase newdc1 = q.cloneDatacase();
				newdc1.addEventToDatacase(p.getEvent(p.getEventSize() - 1));
				candatacase newcandc1 = new candatacase(newdc1);
				newcandc1.setMinsup(minsup);
				Cklist.add(newcandc1);
				// showCandatacase(newcandc1);
			} else if (p.getEvent(p.getEventSize() - 1).getItemSize() > 1
					&& q.getEvent(q.getEventSize() - 1).getItemSize() == 1) {// q的第一個event只有一item,p超過一個
				datacase newdc1 = p.cloneDatacase();
				newdc1.addEventToDatacase(q.getEvent(q.getEventSize() - 1));
				candatacase newcandc1 = new candatacase(newdc1);
				newcandc1.setMinsup(minsup);
				Cklist.add(newcandc1);
				// showCandatacase(newcandc1);
			} else if (p.getEvent(p.getEventSize() - 1).getItemSize() > 1
					&& q.getEvent(q.getEventSize() - 1).getItemSize() > 1) {// p與q的第一個event都含超過一個的item
				if (!compareItems(pi, qi)) {
					if (pi.getItemAttrName() > qi.getItemAttrName()) {// 小的item要在前面
						datacase newdc1 = q.cloneDatacase();
						newdc1.getEvent(newdc1.getEventSize() - 1).addItemToEvent(pi);
						candatacase newcandc1 = new candatacase(newdc1);
						newcandc1.setMinsup(minsup);
						Cklist.add(newcandc1);
						// showCandatacase(newcandc1);
					} else {
						datacase newdc1 = p.cloneDatacase();
						newdc1.getEvent(newdc1.getEventSize() - 1).addItemToEvent(qi);
						candatacase newcandc1 = new candatacase(newdc1);
						newcandc1.setMinsup(minsup);
						Cklist.add(newcandc1);
						// showCandatacase(newcandc1);
					}
				}
			}
		}
	}

	// **********檢查兩個datacase是否包含相同的Min_Item**********
	public boolean checkMinItem(datacase cp, datacase cq, double minsup) {
		for (int i = 0; i < cp.getItemSize(); i++) {
			showDatacase(cp);
			if (getItemMinSup(cp.getItem(i), CURRENTL1SET) == minsup) {
				for (int j = 0; j < cq.getItemSize(); j++) {
					if (compareItems(cp.getItem(i), cq.getItem(i))) {
						return true;
					}
				}
			}
		}
		return false;
	}

	public double getItemMinSup(item itm, ArrayList ilist) {
		for (int i = 0; i < ilist.size(); i++) {
			freqdatacase fdc = (freqdatacase) ilist.get(i);
			if (compareItems(itm, fdc.getDatacase())) {
				return fdc.getMinsup();
			}
		}
		return -1;// 理論上不會return -1
	}

	// **********compare two datacase**********
	public boolean compareDatacases(datacase dca, datacase dcb) {
		if (dca.getEventSize() == dcb.getEventSize()) {
			for (int i = 0; i < dca.getEventSize(); i++) {
				if (!compareEvents(dca.getEvent(i), dcb.getEvent(i))) {
					return false;
				}
			}
		} else {
			return false;
		}
		return true;
	}

	// **********compare two events in two different datacases**********
	public boolean compareEvents(event ea, event eb) {
		if (ea.getItemSize() == eb.getItemSize()) {
			for (int i = 0; i < ea.getItemSize(); i++) {
				if (!compareItems(ea.getItem(i), eb.getItem(i))) {
					return false;
				}
			}
		} else {
			return false;
		}
		return true;
	}

	// **********compare two items in two different events**********
	public boolean compareItems(item a, item b) {
		if (a.getItemAttrName() == b.getItemAttrName() && a.getItemAttrValue() == b.getItemAttrValue()) {
			return true;
		} else {
			return false;
		}
	}

	// **********compare two items between an item and a countitem**********
	public boolean compareItems(item a, datacase dcb) {
		if (dcb.getItemSize() > 1) {
			System.out.print("XXXXXXXXXXXXXXXXXXXX");
		}
		item b = dcb.getItem(0);
		if (a.getItemAttrValue() == b.getItemAttrValue()) {
			return true;
		} else {
			return false;
		}
	}

	// **********給定兩個(k-1) freqdatacase找出其所有的MIN_item**********
	public ArrayList getMinItem(freqdatacase p, freqdatacase q) {
		ArrayList minitemlist = new ArrayList();
		for (int i = 0; i < p.getDatacase().getItemSize(); i++) {
			item pitem = p.getDatacase().getItem(i);
			for (int j = 0; j < q.getDatacase().getItemSize(); j++) {
				item qitem = q.getDatacase().getItem(j);
			}
		}
		return minitemlist;
	}

	// **********計算minsup的大小(其基底是global的,並非只有subdb)**********
	public double calcMinsup(int supcount, int subdbsize) {
		double tmpsup = (double) supcount / subdbsize;

		if (tmpsup * SIGMA >= MIN) {

			return tmpsup * SIGMA;
		} else {
			return MIN;

		}
	}

	// **********演算法主體(一次只做一個class label的subdb)**********
	public void cmtssp(ArrayList oneclassdb) {
		ArrayList dclistbyclass = new ArrayList(); // dclistbyclass負責儲存同屬一個class的freq
													// datacase
		FREQDATACASELIST.add(dclistbyclass);
		findOneLarge(oneclassdb, dclistbyclass);
		for (int k = 2; k == dclistbyclass.size() + 1; k++) {
			ArrayList candlist = new ArrayList();
			System.out.println("candidate join: " + k);
			if (k == 2) {
				candlist = candGenC2((ArrayList) dclistbyclass.get(k - 2), candlist);
				showAllCandatacase(candlist);
			} else {
				candlist = candGenCk((ArrayList) dclistbyclass.get(k - 2), candlist);
				showAllCandatacase(candlist);
			}
			// 計算candidate的support
			countSupport(candlist, oneclassdb);

			// 將滿足support的candidates加到frequent datacase list中
			ArrayList llist = new ArrayList();
			for (int i = 0; i < candlist.size(); i++) {
				if (((candatacase) candlist.get(i)).getCount() >= ((candatacase) candlist.get(i)).getMinsup()
						* oneclassdb.size()) {
					freqdatacase fdc = new freqdatacase((candatacase) candlist.get(i));
					llist.add(fdc);
				}
			}
			if (llist.size() > 0) {
				dclistbyclass.add(llist);
			}
		}
	}

	// **********建立完candidate之後,scan DB去得到各個candidate的support**********
	public void countSupport(ArrayList clist, ArrayList subdb) {
		for (int j = 0; j < subdb.size(); j++) {
			for (int i = 0; i < clist.size(); i++) {
				if (isContain(((candatacase) clist.get(i)).getDatacase(), (datacase) subdb.get(j))) {
					((candatacase) clist.get(i)).addCount();
				}
			}
		}
	}

	// **********判斷一個candidate datacase是否出現在一個datacase中 **********
	public boolean isContain(datacase cdc, datacase dbc) {
		int iter = 0;
		for (int i = 0; i < cdc.getEventSize(); i++) {
			iter = getIter(cdc.getEvent(i), dbc, iter);// 取得candidate中第i個event在db
														// datacase中的位置
			if (iter == dbc.getEventSize()) {
				return false;
			}
			iter++;// 表示candidate的下一個event要從db中match的下一個event開始找
		}
		return true;
	}

	// ***********
	public int getIter(event ce, datacase dbc, int iter) {
		for (int i = iter; i < dbc.getEventSize(); i++) {
			if (eventIsContain(ce, dbc.getEvent(i))) {
				return i;
			}
		}
		return dbc.getEventSize();
	}

	// **********檢查一個event ce是否包含在event dbe裡面**********
	public boolean eventIsContain(event ce, event dbe) {
		for (int i = 0; i < ce.getItemSize(); i++) {
			boolean a = false;
			for (int j = 0; j < dbe.getItemSize(); j++) {
				if (compareItems(ce.getItem(i), dbe.getItem(j))) {
					a = true;
					break;
				}
			}
			if (a == false) {
				return false;
			}
		}
		return true;
	}

	// 將整個DB中infrequent item移除
	public void removeAllInfreqItems(ArrayList db, short itmname, short itmvalue) {
		for (int i = 0; i < db.size(); i++) {
			ArrayList subdb = (ArrayList) db.get(i);
			for (int j = 0; j < subdb.size(); j++) {
				datacase dc = (datacase) subdb.get(j);
				for (int k = 0; k < dc.getEventSize(); k++) {
					event e = dc.getEvent(k);
					for (int l = 0; l < e.getItemSize(); l++) {
						if (itmname == e.getItem(l).getItemAttrName() && itmvalue == e.getItem(l).getItemAttrValue()) {
							e.removeItem(l);
						}
					}
				}
			}
		}
	}

	// **********找出所有Large 1的pattern**********
	public void findOneLarge(ArrayList subdb, ArrayList oneclassdatacase) {
		ArrayList onecounter = new ArrayList();
		// 將subdb中所有item的count算好
		for (int i = 0; i < subdb.size(); i++) {
			for (int j = 0; j < ((datacase) (subdb.get(i))).getEventSize(); j++) {
				for (int k = 0; k < ((event) (((datacase) (subdb.get(i))).getEvent(j))).getItemSize(); k++) {
					short aname = ((item) (((event) (((datacase) (subdb.get(i))).getEvent(j))).getItem(k)))
							.getItemAttrName();
					short avalue = ((item) (((event) (((datacase) (subdb.get(i))).getEvent(j))).getItem(k)))
							.getItemAttrValue();
					checkItemInSameDatacase(onecounter, aname, avalue, (i + 1));
				}
			}
		}
		// 建立L1list的連結
		ArrayList L1list = new ArrayList();// L1list是指包含一個item與classlabel的large
											// datacase
		oneclassdatacase.add(L1list);

		// 將滿足minsup的L1加到L1list
		for (int i = 0; i < onecounter.size(); i++) {
			countitem tmpitem = (countitem) (onecounter.get(i));
			double minsup = calcMinsup(tmpitem.getSupportCount(), subdb.size());

			if (tmpitem.getSupportCount() >= minsup * subdb.size()) {// 要用subdb的datacase#當做基底
				item tmpi = new item(tmpitem.getItemAttrName(), tmpitem.getItemAttrValue());
				event tmpe = new event();
				tmpe.addItemToEvent(tmpi);
				datacase tmpd = new datacase(((datacase) (subdb.get(0))).getClassLabel());
				tmpd.addEventToDatacase(tmpe);
				candatacase tmpcd = new candatacase(tmpd);
				tmpcd.setMinsup(minsup);
				tmpcd.setCount(tmpitem.getSupportCount()); // 設定L1的support
				freqdatacase tmpfd = new freqdatacase(tmpcd);
				L1list.add(tmpfd);

			}
		}
		// sortfunction(L1list);//將資料經過排序後的minsup/
		CURRENTL1SET = L1list;

	}

	// **************candlist中的item依Minsup排序***************************
	public void sortfunction(ArrayList<freqdatacase> mylist) {
		Collections.sort(mylist, new compareF());
		System.out.println("以下為排序後資料:");
		short ClassLabel = 0;
		for (freqdatacase f : mylist) {
			datacase tmpdc = (datacase) (f.getDatacase());
			System.out.print("{");
			for (int i = 0; i < tmpdc.getEventSize(); i++) {
				tmpdc.showevent(i);
				System.out.print("***" + f.getItem(0).getItemAttrValue());
			}
			// f.getItem(0).getItemAttrValue()可以取得item裡面的attr值EX:[1,6] 就找到6
			System.out.print("}");

			System.out.println("*** -> C" + tmpdc.getClassLabel() + "sup/conf: " + f.getSup() + "/" + f.getConfidence()
					+ ". minsup: " + f.getMinsup() + "\n");
			ClassLabel = tmpdc.getClassLabel();
		}
	}

	// **********檢查目前的item是否有在list中,如果在(再check是否同一pattern,如果是->不作任何事,如果不是->count++),如果不在->new
	// countitem
	public boolean checkItemInSameDatacase(ArrayList a, short an, short av, int curpat) {
		for (int i = 0; i < a.size(); i++) {
			countitem tempitem = (countitem) (a.get(i));
			short tempan = tempitem.getItemAttrName();
			short tempav = tempitem.getItemAttrValue();
			if (tempan == an && tempav == av) {
				if (curpat != tempitem.getCurrentPat()) {
					tempitem.setCurrentPat(curpat);
					tempitem.addCount();
				}
				return true;
			}
		}
		// 不在list上
		countitem newitem = new countitem(an, av);
		newitem.addCount();
		newitem.setCurrentPat(curpat);
		a.add(newitem);
		return false;
	}

	// **********顯示所有資料庫中的資料**********
	public void showAllDatacaseInDB(ArrayList db) {
		for (int i = 0; i < db.size(); i++) {
			ArrayList subdb = (ArrayList) (db.get(i));
			for (int j = 0; j < subdb.size(); j++) {
				showDatacase((datacase) (subdb.get(j)));
			}
		}
	}

	// **********顯示資料庫中某類別資料庫中(subdb)的某datacase**********
	public void showDatacase(short classlabel, short datacasenum) {
		datacase showcase;
		showcase = (datacase) (((ArrayList) (DBPOINTER.get(classlabel - 1))).get(datacasenum - 1));
		System.out.print("{");
		for (int i = 0; i < showcase.getEventSize(); i++) {
			showcase.showevent(i);
		}
		System.out.print("}");
		System.out.print(" -> C" + showcase.getClassLabel() + "\n");
	}

	// **********給定一個datacase參考,將datacase列印出來
	public void showDatacase(datacase showcase) {// ****印出資料的方法
		System.out.print("{");
		for (int i = 0; i < showcase.getEventSize(); i++) {
			showcase.showevent(i);
			System.out.print(showcase.getItem(i).getItemAttrValue());// 抓出每一個item的數字

		}
		System.out.print("}");
		System.out.print(" -> C" + showcase.getClassLabel() + "\n");
	}

	//
	public void showAllFreqDatacase(ArrayList alllist) { // 此方法沒有用到
		for (int i = 0; i < alllist.size(); i++) {// alllist存所有的freq dc
			ArrayList sublist = (ArrayList) alllist.get(i);
			for (int j = 0; j < sublist.size(); j++) {// sublist存各個class的freq dc
				ArrayList lklist = (ArrayList) sublist.get(j);
				for (int k = 0; k < lklist.size(); k++) {// lklist存某class的某長度的freq
															// dc
					showFreqDatacase((freqdatacase) lklist.get(k));
				}
			}
			System.out.println("******next class label******");
		}
	}

	// **********給定一個freqdatacase的參考,將該freqdatacase及其sup,minsup列印出來
	public void showFreqDatacase(freqdatacase showcase) {
		datacase tmpdc = (datacase) (showcase.getDatacase());
		System.out.print("{");
		for (int i = 0; i < tmpdc.getEventSize(); i++) {
			tmpdc.showevent(i);
		}
		System.out.print("}");
		System.out.print(" -> C" + tmpdc.getClassLabel() + ". sup/conf: " + showcase.getSup() + "/"
				+ showcase.getConfidence() + ". minsup: " + showcase.getMinsup() + "\n");
	}

	// **********顯示candidate datacase**********
	public void showCandatacase(candatacase showcase) {
		datacase tmpdc = (datacase) (showcase.getDatacase());
		System.out.print("{");
		for (int i = 0; i < tmpdc.getEventSize(); i++) {
			tmpdc.showevent(i);
		}
		System.out.print("}");
		System.out.print(" -> C" + tmpdc.getClassLabel() + ". minsup: " + showcase.getMinsup() + "\n");
	}

	// **********顯示所有candidate datacase***********
	public void showAllCandatacase(ArrayList candlist) {
		for (int i = 0; i < candlist.size(); i++) {
			showCandatacase((candatacase) candlist.get(i));
		}
	}

	// **********將資料load到memory中(記得要將不同class分開)**********
	public ArrayList loadDataToDB(String inputfile) {
		ArrayList db = new ArrayList(); // db.get(0):class
										// label為1的所有datacase的集合(另一個arraylist),...依此類推
		// 開啟training dataset檔案(不同class label的data被放在不同檔案裡)
		FileReader file_in;
		BufferedReader buff_reader;
		StringTokenizer str_token;
		String line_data;

		try {
			file_in = new FileReader(inputfile);
			buff_reader = new BufferedReader(file_in);
			ArrayList subdb = new ArrayList();
			short classlabel = -1;
			while ((line_data = buff_reader.readLine()) != null) {
				str_token = new StringTokenizer(line_data.trim());
				if (str_token.countTokens() == 1) { // 當token數目為一,表示此行為class
													// label
					classlabel = Short.valueOf(str_token.nextToken()).shortValue();
					subdb = new ArrayList();
					db.add(subdb);
				} else { // 表示為資料行,底下進行db的建置
					datacase tempdatacase = new datacase(classlabel);
					short tempid = Short.valueOf(str_token.nextToken()).shortValue();
					while (str_token.hasMoreTokens()) {
						event e = new event();
						short attr = 1;
						short attrvalue = Short.valueOf(str_token.nextToken()).shortValue();
						item tempitem = new item(attr, attrvalue);
						e.addItemToEvent(tempitem);
						tempdatacase.addEventToDatacase(e);
					}
					showDatacase(tempdatacase);
					subdb.add(tempdatacase);
				}
			}

			file_in.close();
		} catch (IOException e) {
			System.out.println(e);
		}
		return db;
	}

	// 將training後的所有pattern儲存到檔案裡
	public void outputTraining(String trainoutput, ArrayList alllist) {
		try {
			FileWriter output_file = new FileWriter(trainoutput, true);

			for (int i = 0; i < alllist.size(); i++) {// alllist存所有的freq dc
				ArrayList sublist = (ArrayList) alllist.get(i);
				output_file.write((i + 1) + "\n");
				int counter = 1;
				for (int j = 0; j < sublist.size(); j++) {// sublist存各個class的freq
															// dc
					ArrayList lklist = (ArrayList) sublist.get(j);
					for (int k = 0; k < lklist.size(); k++) {// lklist存某class的某長度的freq
																// dc
						freqdatacase fdc = (freqdatacase) lklist.get(k);
						datacase dc = fdc.getDatacase();
						showDatacase(dc);
						output_file.write(counter + " 0 " + fdc.getSup() + " " + fdc.getConfidence() + " "
								+ fdc.getMinsup() + "\n");
						for (int l = 0; l < dc.getEventSize(); l++) {
							output_file.write(counter + " ");
							output_file.write((l + 1) + " ");
							event dce = dc.getEvent(l);
							for (int m = 0; m < dce.getItemSize(); m++) {
								item dcei = dce.getItem(m);
								output_file.write(dcei.getItemAttrName() + " " + dcei.getItemAttrValue() + " ");
							}
							output_file.write("\n");
						}
						counter++;
					}
				}
			}
			output_file.close();
		} catch (IOException e) {
			System.out.println(e);
		}
	}
}